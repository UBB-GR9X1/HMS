using System.Collections.Generic;
using System.Linq;
using System;
using System.Globalization;

namespace HMS.Shared.DTOs
{
    public class ShiftDto
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public List<int> DoctorIds { get; set; } = new List<int>();

        public string FormattedDoctorIds => DoctorIds != null && DoctorIds.Any() 
            ? string.Join(", ", DoctorIds) 
            : "None";

        // Helper string property for DateOnly binding in DataGrid
        public string DateString
        {
            get => Date.ToString("MM/dd/yyyy"); // Format for display/editing
            set
            {
                if (DateOnly.TryParseExact(value, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly dateOnly))
                {
                    Date = dateOnly;
                }
                // else: Handle parsing error if needed (e.g., notify user, keep old value)
            }
        }

        // Helper string property for TimeOnly binding in DataGrid
        public string StartTimeString
        {
            get => StartTime.ToString("HH:mm"); // Format for display/editing
            set
            {
                if (TimeOnly.TryParseExact(value, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out TimeOnly timeOnly))
                {
                    StartTime = timeOnly;
                }
                // else: Handle parsing error if needed
            }
        }

        // Helper string property for TimeOnly binding in DataGrid
        public string EndTimeString
        {
            get => EndTime.ToString("HH:mm"); // Format for display/editing
            set
            {
                if (TimeOnly.TryParseExact(value, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out TimeOnly timeOnly))
                {
                    EndTime = timeOnly;
                }
                // else: Handle parsing error if needed
            }
        }

        // Although we removed DoctorIds column, keeping FormattedDoctorIds as it might be used elsewhere.
        // If DoctorIds editing is needed in the grid later, a similar helper property would be required, or a custom column.
    }
}
