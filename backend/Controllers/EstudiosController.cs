using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Models.DTOs;
using backend.Services.Interfaces;
using backend.Helpers;

namespace backend.Controllers;

[ApiController]
[Route("api/users/{userId}/estudios")]
[Authorize]
public class EstudiosController : ControllerBase
{
    private readonly IEstudioService _estudioService;

    public EstudiosController(IEstudioService estudioService)
    {
        _estudioService = estudioService;
    }

    // GET /api/users/{userId}/estudios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EstudioResponseDTO>>> GetAll(int userId)
    {
        if (!AuthorizationHelper.IsOwnerOrAdmin(User, userId))
            return Forbid();

        var estudios = await _estudioService.GetEstudiosByUserIdAsync(userId);
        return Ok(estudios);
    }

    // GET /api/users/{userId}/estudios/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<EstudioResponseDTO>> GetById(int userId, int id)
    {
        if (!AuthorizationHelper.IsOwnerOrAdmin(User, userId))
            return Forbid();

        var estudio = await _estudioService.GetEstudioByIdAsync(id);
        if (estudio == null || estudio.UserId != userId)
            return NotFound();

        return Ok(estudio);
    }

    // POST /api/users/{userId}/estudios
    [HttpPost]
    public async Task<ActionResult<EstudioResponseDTO>> Create(int userId, [FromBody] CreateEstudioDTO dto)
    {
        if (!AuthorizationHelper.IsOwnerOrAdmin(User, userId))
            return Forbid();

        try
        {
            var estudio = await _estudioService.CreateEstudioAsync(userId, dto);
            return CreatedAtAction(nameof(GetById), new { userId, id = estudio.Id }, estudio);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT /api/users/{userId}/estudios/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<EstudioResponseDTO>> Update(int userId, int id, [FromBody] UpdateEstudioDTO dto)
    {
        if (!AuthorizationHelper.IsOwnerOrAdmin(User, userId))
            return Forbid();

        var existing = await _estudioService.GetEstudioByIdAsync(id);
        if (existing == null || existing.UserId != userId)
            return NotFound();

        try
        {
            var estudio = await _estudioService.UpdateEstudioAsync(id, dto);
            return Ok(estudio);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE /api/users/{userId}/estudios/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int userId, int id)
    {
        if (!AuthorizationHelper.IsOwnerOrAdmin(User, userId))
            return Forbid();

        var existing = await _estudioService.GetEstudioByIdAsync(id);
        if (existing == null || existing.UserId != userId)
            return NotFound();

        await _estudioService.DeleteEstudioAsync(id);
        return NoContent();
    }
}
