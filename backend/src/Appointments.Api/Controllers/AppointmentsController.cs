using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Appointments.Infrastructure.Data;
using Appointments.Core.Entities;
using Appointments.Core.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Appointments.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AppointmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointments(
            [FromQuery] int? doctorId, 
            [FromQuery] string? status, 
            [FromQuery] DateTime? date)
        {
            var query = _context.Appointments.AsQueryable();

            if (doctorId.HasValue)
                query = query.Where(a => a.DoctorId == doctorId.Value);

            if (!string.IsNullOrEmpty(status) &&
                Enum.TryParse<AppointmentStatus>(status, true, out var parsedStatus))
            {
                query = query.Where(a => a.Status == parsedStatus);
            }

            if (date.HasValue) {
                var utcDate = DateTime.SpecifyKind(date.Value.Date, DateTimeKind.Utc);
    
                // Filter by date range: start of day <= Date < start of next day
                var nextDay = utcDate.AddDays(1);
                query = query.Where(a => a.Date >= utcDate && a.Date < nextDay);
            }
                // query = query.Where(a => a.Date.Date == date.Value.Date);

            var appointments = await query
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    DoctorName = a.Doctor.FullName,
                    DoctorId = a.DoctorId,
                    PatientName = a.Patient.FullName,
                    PatientId = a.PatientId,
                    Date = a.Date,
                    Status = a.Status.ToString()
                })
                .ToListAsync();

            return Ok(appointments);
        }

        // GET: api/appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDto>> GetAppointment(int id)
        {
            var appointment = await _context.Appointments
                                .Include(a => a.Doctor)
                                .Include(a => a.Patient)
                                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
                return NotFound();

            return new AppointmentDto
            {
                Id = appointment.Id,
                DoctorName = appointment.Doctor.FullName,
                DoctorId = appointment.DoctorId,
                PatientName = appointment.Patient.FullName,
                PatientId = appointment.PatientId,
                Date = appointment.Date,
                Status = appointment.Status.ToString()
            };
        }

        // POST: api/appointments
        [HttpPost]
        public async Task<ActionResult<AppointmentDto>> CreateAppointment(CreateAppointmentDto dto)
        {
            var appointment = new Appointment
            {
                DoctorId = dto.DoctorId,
                PatientId = dto.PatientId,
                Date = dto.Date.ToUniversalTime(),
                Status = Enum.Parse<AppointmentStatus>(dto.Status, true)
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointment);
        }

        // PUT: api/appointments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, AppointmentDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return NotFound();

            appointment.DoctorId = dto.DoctorId;
            appointment.PatientId = dto.PatientId;
            appointment.Date = dto.Date.ToUniversalTime();
            appointment.Status = Enum.Parse<AppointmentStatus>(dto.Status, true);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/appointments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return NotFound();

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
