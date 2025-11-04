using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Appointments.Api.Controllers;
using Appointments.Core.Entities;
using Appointments.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;
using Appointments.Core.DTOs;  

namespace Appointments.Tests.Controllers
{
    public class AppointmentsControllerTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
                .Options;

            var context = new AppDbContext(options);

            context.Doctors.Add(new Doctor { Id = 1, FullName = "Dr. Smith" });
            context.Patients.Add(new Patient { Id = 1, FullName = "John Doe" });

            context.Appointments.Add(new Appointment
            {
                Id = 1,
                DoctorId = 1,
                PatientId = 1,
                Date = DateTime.UtcNow,
                Status = AppointmentStatus.Scheduled
            });

            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task GetAppointments_ReturnsAllAppointments()
        {
            var context = GetInMemoryDbContext();
            var controller = new AppointmentsController(context);

            var result = await controller.GetAppointments(null, null, null);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var appointments = Assert.IsAssignableFrom<List<AppointmentDto>>(okResult.Value);
            appointments.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetAppointment_ById_ReturnsAppointment()
        {
            var context = GetInMemoryDbContext();
            var controller = new AppointmentsController(context);

            var result = await controller.GetAppointment(1);

            var okResult = Assert.IsType<AppointmentDto>(result.Value);
            okResult.Id.Should().Be(1);
            okResult.DoctorName.Should().Be("Dr. Smith");
            okResult.PatientName.Should().Be("John Doe");
        }

        [Fact]
        public async Task CreateAppointment_AddsNewAppointment()
        {
            var context = GetInMemoryDbContext();
            var controller = new AppointmentsController(context);

            var dto = new CreateAppointmentDto
            {
                DoctorId = 1,
                PatientId = 1,
                Date = DateTime.UtcNow.AddDays(1),
                Status = AppointmentStatus.Scheduled.ToString()
            };

            var result = await controller.CreateAppointment(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var appointment = Assert.IsType<Appointment>(createdResult.Value);
            appointment.Id.Should().BePositive();
        }

        [Fact]
        public async Task UpdateAppointment_ChangesAppointment()
        {
            var context = GetInMemoryDbContext();
            var controller = new AppointmentsController(context);

            var dto = new AppointmentDto
            {
                Id = 1,
                DoctorId = 1,
                PatientId = 1,
                Date = DateTime.UtcNow.AddDays(2),
                Status = AppointmentStatus.Completed.ToString()
            };

            var result = await controller.UpdateAppointment(1, dto);
            Assert.IsType<NoContentResult>(result);

            var updated = await context.Appointments.FindAsync(1);
            updated.Status.Should().Be(AppointmentStatus.Completed);
        }

        [Fact]
        public async Task DeleteAppointment_RemovesAppointment()
        {
            var context = GetInMemoryDbContext();
            var controller = new AppointmentsController(context);

            var result = await controller.DeleteAppointment(1);
            Assert.IsType<NoContentResult>(result);

            var deleted = await context.Appointments.FindAsync(1);
            deleted.Should().BeNull();
        }
    }
}
