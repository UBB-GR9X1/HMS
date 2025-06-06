using System.Collections.Generic;

namespace HMS.Shared.DTOs
{
    public class MedicalRecordResponseDto
    {
        public IEnumerable<MedicalRecordDto> Records { get; set; } = new List<MedicalRecordDto>();
    }
} 