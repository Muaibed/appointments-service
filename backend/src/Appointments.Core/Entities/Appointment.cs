namespace Appointments.Core.Entities;

public enum AppointmentStatus
{
    Scheduled,
    Completed,
    Cancelled
}

public class Appointment
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public AppointmentStatus Status { get; set; }

    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
}
