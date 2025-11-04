using Appointments.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Appointments.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (await context.Doctors.AnyAsync()) return;

            var doctors = new[]
            {
                new Doctor { FullName = "Dr. Ali Hassan", Specialty = "Cardiology", Email = "ali.hassan@clinic.com" },
                new Doctor { FullName = "Dr. Fatimah Zahrani", Specialty = "Dermatology", Email = "fatimah.zahrani@clinic.com" },
                new Doctor { FullName = "Dr. Khalid Almutairi", Specialty = "Pediatrics", Email = "khalid.almutairi@clinic.com" },
                new Doctor { FullName = "Dr. Sarah Alshammari", Specialty = "Ophthalmology", Email = "sarah.alshammari@clinic.com" },
                new Doctor { FullName = "Dr. Abdullah Alqahtani", Specialty = "General Surgery", Email = "abdullah.alqahtani@clinic.com" },
                new Doctor { FullName = "Dr. Noura Alotaibi", Specialty = "Dentistry", Email = "noura.alotaibi@clinic.com" },
                new Doctor { FullName = "Dr. Hassan Almalki", Specialty = "Neurology", Email = "hassan.almalki@clinic.com" },
                new Doctor { FullName = "Dr. Layla Alharbi", Specialty = "Gynecology", Email = "layla.alharbi@clinic.com" }
            };

            var patients = new[]
            {
                new Patient { FullName = "Mohammed Abdullah", Email = "mohammed.abdullah@example.com", DateOfBirth = new DateTime(1992, 5, 10).ToUniversalTime() },
                new Patient { FullName = "Reem Alharbi", Email = "reem.alharbi@example.com", DateOfBirth = new DateTime(1989, 11, 22).ToUniversalTime() },
                new Patient { FullName = "Salman Alshammari", Email = "salman.alshammari@example.com", DateOfBirth = new DateTime(1995, 1, 14).ToUniversalTime() },
                new Patient { FullName = "Layan Alqahtani", Email = "layan.alqahtani@example.com", DateOfBirth = new DateTime(1998, 8, 30).ToUniversalTime() },
                new Patient { FullName = "Abdulrahman Zahrani", Email = "abdulrahman.zahrani@example.com", DateOfBirth = new DateTime(1990, 3, 18).ToUniversalTime() },
                new Patient { FullName = "Nouf Alotaibi", Email = "nouf.alotaibi@example.com", DateOfBirth = new DateTime(1993, 9, 5).ToUniversalTime() },
                new Patient { FullName = "Huda Almutairi", Email = "huda.almutairi@example.com", DateOfBirth = new DateTime(1996, 6, 27).ToUniversalTime() },
                new Patient { FullName = "Faisal Almalki", Email = "faisal.almalki@example.com", DateOfBirth = new DateTime(1987, 12, 2).ToUniversalTime() },
                new Patient { FullName = "Rania Hassan", Email = "rania.hassan@example.com", DateOfBirth = new DateTime(1999, 4, 11).ToUniversalTime() },
                new Patient { FullName = "Mona Alotaibi", Email = "mona.alotaibi@example.com", DateOfBirth = new DateTime(1991, 7, 9).ToUniversalTime() },
                new Patient { FullName = "Ali Alharbi", Email = "ali.alharbi@example.com", DateOfBirth = new DateTime(1988, 2, 25).ToUniversalTime() },
                new Patient { FullName = "Nasser Almutairi", Email = "nasser.almutairi@example.com", DateOfBirth = new DateTime(1994, 10, 3).ToUniversalTime() },
                new Patient { FullName = "sarah Alqahtani", Email = "sarah.alqahtani@example.com", DateOfBirth = new DateTime(2000, 12, 17).ToUniversalTime() },
                new Patient { FullName = "Yousef Almalki", Email = "yousef.almalki@example.com", DateOfBirth = new DateTime(1986, 9, 21).ToUniversalTime() },
                new Patient { FullName = "Dana Zahrani", Email = "dana.zahrani@example.com", DateOfBirth = new DateTime(1997, 3, 13).ToUniversalTime() }
            };

            var users = new[]
            {
                new User { Username = "admin", PasswordHash = HashPassword("password"), Role = Role.Admin },
                new User { Username = "user1", PasswordHash = HashPassword("pass1"), Role = Role.User },
            };

            await context.Doctors.AddRangeAsync(doctors);
            await context.Patients.AddRangeAsync(patients);
            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();

            var appointments = new[]
            {
                new Appointment { DoctorId = doctors[0].Id, PatientId = patients[0].Id, Date = DateTime.UtcNow.AddDays(1), Status = AppointmentStatus.Scheduled },
                new Appointment { DoctorId = doctors[1].Id, PatientId = patients[1].Id, Date = DateTime.UtcNow.AddDays(2), Status = AppointmentStatus.Completed },
                new Appointment { DoctorId = doctors[2].Id, PatientId = patients[2].Id, Date = DateTime.UtcNow.AddDays(3), Status = AppointmentStatus.Scheduled },
                new Appointment { DoctorId = doctors[3].Id, PatientId = patients[3].Id, Date = DateTime.UtcNow.AddDays(4), Status = AppointmentStatus.Cancelled },
                new Appointment { DoctorId = doctors[4].Id, PatientId = patients[4].Id, Date = DateTime.UtcNow.AddDays(5), Status = AppointmentStatus.Scheduled },
                new Appointment { DoctorId = doctors[5].Id, PatientId = patients[5].Id, Date = DateTime.UtcNow.AddDays(6), Status = AppointmentStatus.Completed },
                new Appointment { DoctorId = doctors[6].Id, PatientId = patients[6].Id, Date = DateTime.UtcNow.AddDays(7), Status = AppointmentStatus.Scheduled },
                new Appointment { DoctorId = doctors[7].Id, PatientId = patients[7].Id, Date = DateTime.UtcNow.AddDays(8), Status = AppointmentStatus.Scheduled },
                new Appointment { DoctorId = doctors[0].Id, PatientId = patients[8].Id, Date = DateTime.UtcNow.AddDays(2), Status = AppointmentStatus.Scheduled },
                new Appointment { DoctorId = doctors[1].Id, PatientId = patients[9].Id, Date = DateTime.UtcNow.AddDays(3), Status = AppointmentStatus.Completed },
                new Appointment { DoctorId = doctors[2].Id, PatientId = patients[10].Id, Date = DateTime.UtcNow.AddDays(4), Status = AppointmentStatus.Scheduled },
                new Appointment { DoctorId = doctors[3].Id, PatientId = patients[11].Id, Date = DateTime.UtcNow.AddDays(5), Status = AppointmentStatus.Cancelled },
                new Appointment { DoctorId = doctors[4].Id, PatientId = patients[12].Id, Date = DateTime.UtcNow.AddDays(6), Status = AppointmentStatus.Scheduled },
                new Appointment { DoctorId = doctors[5].Id, PatientId = patients[13].Id, Date = DateTime.UtcNow.AddDays(7), Status = AppointmentStatus.Completed },
                new Appointment { DoctorId = doctors[6].Id, PatientId = patients[14].Id, Date = DateTime.UtcNow.AddDays(8), Status = AppointmentStatus.Scheduled }
            };

            await context.Appointments.AddRangeAsync(appointments);
            await context.SaveChangesAsync();
        }
        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }

}
