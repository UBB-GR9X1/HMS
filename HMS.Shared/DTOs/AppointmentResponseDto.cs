using System.Collections.Generic;

namespace HMS.Shared.DTOs
{
    public class AppointmentResponseDto
    {
        public IEnumerable<AppointmentDto> Records { get; set; } = new List<AppointmentDto>();
    }
} 