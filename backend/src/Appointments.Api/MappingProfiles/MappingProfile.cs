using AutoMapper;
using Appointments.Api.DTOs;
using Appointments.Core.Entities;

namespace Appointments.Api.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Doctor, DoctorDto>().ReverseMap();
        CreateMap<Patient, PatientDto>().ReverseMap();
        CreateMap<Appointment, AppointmentDto>().ReverseMap();
    }
}
