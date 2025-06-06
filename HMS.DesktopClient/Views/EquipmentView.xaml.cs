using HMS.Shared.DTOs;
using HMS.Shared.Proxies.Implementations;
using HMS.Shared.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;

namespace HMS.DesktopClient.Views
{
    public sealed partial class EquipmentView : Window
    {
        private readonly EquipmentService _equipmentService;
        private readonly UserWithTokenDto _user;

        public EquipmentView(UserWithTokenDto user)
        {
            this.InitializeComponent();
            _user = user;
            var proxy = new EquipmentProxy(user.Token);
            _equipmentService = new EquipmentService(proxy);
            LoadEquipment();
        }

        private async void LoadEquipment()
        {
            try
            {
                var equipmentList = await _equipmentService.GetAllAsync();
                EquipmentListView.ItemsSource = equipmentList;
            }
            catch (System.Exception ex)
            {
                var dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"Failed to load equipment: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };
                await dialog.ShowAsync();
            }
        }

        private async void DeleteEquipment_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                try
                {
                    await _equipmentService.DeleteAsync(id);
                    LoadEquipment();
                }
                catch (System.Exception ex)
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = $"Failed to delete equipment: {ex.Message}",
                        CloseButtonText = "OK",
                        XamlRoot = this.Content.XamlRoot
                    };
                    await dialog.ShowAsync();
                }
            }
        }

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement add new equipment dialog or navigation
            var dialog = new ContentDialog
            {
                Title = "Add New",
                Content = "Add new equipment functionality not implemented yet.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            dialog.ShowAsync();
        }
    }
}