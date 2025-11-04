using AutoMapper;
using Appointments.Api.DTOs;
using Appointments.Core.Entities;
using Appointments.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Appointments.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public PatientsController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatients()
    {
        var patients = await _context.Patients.ToListAsync();
        return Ok(_mapper.Map<IEnumerable<PatientDto>>(patients));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PatientDto>> GetPatient(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null) return NotFound();
        return Ok(_mapper.Map<PatientDto>(patient));
    }

    [HttpPost]
    public async Task<ActionResult<PatientDto>> CreatePatient(PatientDto dto)
    {
        var patient = _mapper.Map<Patient>(dto);
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, _mapper.Map<PatientDto>(patient));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePatient(int id, PatientDto dto)
    {
        if (id != dto.Id) return BadRequest();
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null) return NotFound();

        _mapper.Map(dto, patient);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null) return NotFound();

        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
