using AutoMapper;
using Appointments.Api.DTOs;
using Appointments.Core.Entities;
using Appointments.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public DoctorsController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctors()
    {
        var doctors = await _context.Doctors.ToListAsync();
        return Ok(_mapper.Map<IEnumerable<DoctorDto>>(doctors));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DoctorDto>> GetDoctor(int id)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        if (doctor == null) return NotFound();
        return Ok(_mapper.Map<DoctorDto>(doctor));
    }

    [HttpPost]
    public async Task<ActionResult<DoctorDto>> CreateDoctor(DoctorDto dto)
    {
        var doctor = _mapper.Map<Doctor>(dto);
        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, _mapper.Map<DoctorDto>(doctor));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDoctor(int id, DoctorDto dto)
    {
        if (id != dto.Id) return BadRequest();
        var doctor = await _context.Doctors.FindAsync(id);
        if (doctor == null) return NotFound();

        _mapper.Map(dto, doctor);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDoctor(int id)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        if (doctor == null) return NotFound();

        _context.Doctors.Remove(doctor);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
