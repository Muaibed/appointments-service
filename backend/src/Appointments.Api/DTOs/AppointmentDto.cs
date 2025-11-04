using Appointments.Core.Entities;

namespace Appointments.Api.DTOs;

public class AppointmentDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public AppointmentStatus Status { get; set; }
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
}
