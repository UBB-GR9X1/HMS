using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using HMS.Shared.Enums;
using System.Security.Authentication;
using HMS.Shared.Entities;
using HMS.Shared.Proxies.Implementations;
using HMS.Shared.DTOs;
using HMS.DesktopClient.APIClients;
using Microsoft.Extensions.DependencyInjection;
using HMS.DesktopClient.ViewModels;
using HMS.Shared.Services.Implementations;
using HMS.Shared.Repositories.Interfaces;
using HMS.Shared.Services.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HMS.DesktopClient.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateAccountPage : Window
    {
        private readonly UserApiClient userApiClient;

        public CreateAccountPage()
        {
            this.userApiClient = App.Services.GetRequiredService<UserApiClient>();
            this.InitializeComponent();
        }

        private async void CreateAccountButton_Click(object sender, RoutedEventArgs routed_event_args)
        {
            string password = this.password_field.Password;
            string mail = this.email_text_box.Text;
            string name = this.name_text_box.Text;
            string phone_number = this.phone_number_text_box.Text;
            string emergency_contact = this.emergency_contact_text_box.Text;

            if (this.birth_date_calendar_picker.Date.HasValue)
            {
                DateOnly birth_date = DateOnly.FromDateTime(this.birth_date_calendar_picker.Date.Value.DateTime);
                this.birth_date_calendar_picker.Date = new DateTimeOffset(birth_date.ToDateTime(TimeOnly.MinValue));

                string cnp = this.cnp_textbox.Text;

                BloodType? selected_blood_type = null;
                if (this.blood_type_combo_box.SelectedItem is ComboBoxItem selected_item)
                {
                    string? selected_tag = selected_item.Tag.ToString();
                    if (selected_tag != null)
                    {
                        switch (selected_tag.Trim())
                        {
                            case "A_POSITIVE":
                                selected_blood_type = BloodType.A_Positive;
                                break;
                            case "A_NEGATIVE":
                                selected_blood_type = BloodType.A_Negative;
                                break;
                            case "B_POSITIVE":
                                selected_blood_type = BloodType.B_Positive;
                                break;
                            case "B_NEGATIVE":
                                selected_blood_type = BloodType.B_Negative;
                                break;
                            case "AB_POSITIVE":
                                selected_blood_type = BloodType.AB_Positive;
                                break;
                            case "AB_NEGATIVE":
                                selected_blood_type = BloodType.AB_Negative;
                                break;
                            case "O_POSITIVE":
                                selected_blood_type = BloodType.O_Positive;
                                break;
                            case "O_NEGATIVE":
                                selected_blood_type = BloodType.O_Negative;
                                break;
                        }
                    }
                }

                if (selected_blood_type == null)
                {
                    var validation_dialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = "Please select a blood type.",
                        CloseButtonText = "OK",
                    };

                    validation_dialog.XamlRoot = this.Content.XamlRoot;
                    await validation_dialog.ShowAsync();
                    return;
                }

                bool weight_valid = double.TryParse(this.weight_text_box.Text, out double weight);
                bool height_valid = int.TryParse(this.height_text_box.Text, out int height);

                if (!weight_valid || !height_valid || weight <= 0 || height <= 0)
                {
                    var validation_dialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = "Please enter valid Weight (kg) and Height (cm).",
                        CloseButtonText = "OK",
                    };

                    validation_dialog.XamlRoot = this.Content.XamlRoot;
                    await validation_dialog.ShowAsync();
                    return;
                }

                try
                {
                    Patient patient = new Patient
                    {
                        Email = mail,
                        Password = password,
                        Role = UserRole.Patient,
                        Name = name,
                        CNP = cnp,
                        PhoneNumber = phone_number,
                        BloodType = selected_blood_type.Value,
                        EmergencyContact = emergency_contact,
                        Allergies = "No allergies",
                        Weight = ((float)weight),
                        Height = height,
                        BirthDate = birth_date.ToDateTime(TimeOnly.MinValue),
                        Address = "Not provided", // Address can be set later
                    };
                    IPatientRepository patientProxy = new PatientProxy(new System.Net.Http.HttpClient());
                    IPatientService patientService = new PatientService(patientProxy);
                    PatientViewModel patientViewModel = new PatientViewModel(patientService);
                    await patientViewModel.AddPatient(patient);

                    var successDialog = new ContentDialog
                    {
                        Title = "Success",
                        Content = "Account created successfully! Redirecting to login page in 3 seconds...",
                        CloseButtonText = "OK",
                        XamlRoot = this.Content.XamlRoot,
                    };

                    await successDialog.ShowAsync();

                    await Task.Delay(3000);


                    var loginPage = new LoginPage();
                    loginPage.Activate();
                    this.Close();
                    return;

                }
                catch (AuthenticationException error)
                {
                    var validation_dialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = $"{error.Message}",
                        CloseButtonText = "OK",
                        XamlRoot = this.Content.XamlRoot,
                    };
                    await validation_dialog.ShowAsync();
                }
            }
            else
            {
                var validation_dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Birth date is required.",
                    CloseButtonText = "OK",
                };

                validation_dialog.XamlRoot = this.Content.XamlRoot;
                await validation_dialog.ShowAsync();
            }
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs routed_event_args)
        {
            var logInPage = new LoginPage();
            logInPage.Activate();
            this.Close();
        }
    }
}
