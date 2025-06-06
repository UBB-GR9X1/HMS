using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using HMS.Shared.Proxies.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.DesktopClient.Utils
{
    public class LoggerService
    {
        private readonly LoggerProxy _loggerProxy;
        private List<LogDto> _cachedLogs;
        private List<string> _cachedActionTypes;

        public LoggerService(LoggerProxy loggerProxy)
        {
            _loggerProxy = loggerProxy;
            _cachedLogs = new List<LogDto>();
            _cachedActionTypes = new List<string>();
        }

        public async Task<IEnumerable<LogDto>> GetAllLogsAsync()
        {
            try
            {
                var logs = await _loggerProxy.GetAllAsync();
                _cachedLogs = logs.Select(l => new LogDto
                {
                    Id = l.Id,
                    UserId = l.UserId,
                    Action = l.Action,
                    CreatedAt = l.CreatedAt
                }).ToList();

                UpdateCachedActionTypes();

                return _cachedLogs;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error retrieving logs: {ex.Message}");
                return new List<LogDto>();
            }
        }

        public async Task<IEnumerable<string>> GetActionTypesAsync()
        {
            if (_cachedLogs.Count == 0)
            {
                await GetAllLogsAsync();
            }
            else if (_cachedActionTypes.Count == 0)
            {
                UpdateCachedActionTypes();
            }

            return _cachedActionTypes;
        }

        private void UpdateCachedActionTypes()
        {
            _cachedActionTypes = _cachedLogs
                .Select(l => l.Action)
                .Where(a => !string.IsNullOrEmpty(a))
                .Distinct()
                .OrderBy(a => a)
                .ToList();
        }

        public async Task<IEnumerable<LogDto>> GetLogsByUserIdAsync(int userId)
        {
            if (_cachedLogs.Count == 0)
            {
                await GetAllLogsAsync();
            }

            return _cachedLogs.Where(l => l.UserId == userId).ToList();
        }

        public async Task<IEnumerable<LogDto>> GetLogsByActionAsync(string action)
        {
            if (_cachedLogs.Count == 0)
            {
                await GetAllLogsAsync();
            }

            return _cachedLogs.Where(l => l.Action != null &&
                l.Action.Equals(action, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public async Task<IEnumerable<LogDto>> GetLogsByTimestampAsync(DateTime timestamp)
        {
            if (_cachedLogs.Count == 0)
            {
                await GetAllLogsAsync();
            }

            var dateOnly = timestamp.Date;
            return _cachedLogs.Where(l => l.CreatedAt.Date == dateOnly).ToList();
        }

        public async Task<IEnumerable<LogDto>> GetLogsByMultipleFiltersAsync(
            int? userId, string action, DateTime? timestamp)
        {
            if (_cachedLogs.Count == 0)
            {
                await GetAllLogsAsync();
            }

            IEnumerable<LogDto> filteredLogs = _cachedLogs;

            if (userId.HasValue)
            {
                filteredLogs = filteredLogs.Where(l => l.UserId == userId.Value);
            }

            if (!string.IsNullOrEmpty(action))
            {
                filteredLogs = filteredLogs.Where(l => l.Action != null &&
                    l.Action.Equals(action, StringComparison.OrdinalIgnoreCase));
            }

            if (timestamp.HasValue)
            {
                var dateOnly = timestamp.Value.Date;
                filteredLogs = filteredLogs.Where(l => l.CreatedAt.Date == dateOnly);
            }

            return filteredLogs.ToList();
        }
    }
}