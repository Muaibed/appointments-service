namespace Appointments.Core.DTOs
{
    public class CreateAppointmentDto
    {
        public required int PatientId { get; set; }
        public required int DoctorId { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = "Scheduled";
    }
}
