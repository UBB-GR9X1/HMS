public class PatientCreateDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string CNP { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string BloodType { get; set; } = null!;
    public string EmergencyContact { get; set; } = null!;
    public string Allergies { get; set; } = "";
    public float Weight { get; set; }
    public float Height { get; set; }
    public DateOnly BirthDate { get; set; }
    public string Address { get; set; } = null!;
}
