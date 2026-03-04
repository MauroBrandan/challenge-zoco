using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Models.DTOs;
using backend.Services.Interfaces;
using backend.Helpers;

namespace backend.Controllers;

[ApiController]
[Route("api/users/{userId}/direcciones")]
[Authorize]
public class DireccionesController : ControllerBase
{
    private readonly IDireccionService _direccionService;

    public DireccionesController(IDireccionService direccionService)
    {
        _direccionService = direccionService;
    }

    // GET /api/users/{userId}/direcciones
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DireccionResponseDTO>>> GetAll(int userId)
    {
        if (!AuthorizationHelper.IsOwnerOrAdmin(User, userId))
            return Forbid();

        var direcciones = await _direccionService.GetDireccionesByUserIdAsync(userId);
        return Ok(direcciones);
    }

    // GET /api/users/{userId}/direcciones/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<DireccionResponseDTO>> GetById(int userId, int id)
    {
        if (!AuthorizationHelper.IsOwnerOrAdmin(User, userId))
            return Forbid();

        var direccion = await _direccionService.GetDireccionByIdAsync(id);
        if (direccion == null || direccion.UserId != userId)
            return NotFound();

        return Ok(direccion);
    }

    // POST /api/users/{userId}/direcciones
    [HttpPost]
    public async Task<ActionResult<DireccionResponseDTO>> Create(int userId, [FromBody] CreateDireccionDTO dto)
    {
        if (!AuthorizationHelper.IsOwnerOrAdmin(User, userId))
            return Forbid();

        var direccion = await _direccionService.CreateDireccionAsync(userId, dto);
        return CreatedAtAction(nameof(GetById), new { userId, id = direccion.Id }, direccion);
    }

    // PUT /api/users/{userId}/direcciones/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<DireccionResponseDTO>> Update(int userId, int id, [FromBody] UpdateDireccionDTO dto)
    {
        if (!AuthorizationHelper.IsOwnerOrAdmin(User, userId))
            return Forbid();

        var existing = await _direccionService.GetDireccionByIdAsync(id);
        if (existing == null || existing.UserId != userId)
            return NotFound();

        var direccion = await _direccionService.UpdateDireccionAsync(id, dto);
        return Ok(direccion);
    }

    // DELETE /api/users/{userId}/direcciones/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int userId, int id)
    {
        if (!AuthorizationHelper.IsOwnerOrAdmin(User, userId))
            return Forbid();

        var existing = await _direccionService.GetDireccionByIdAsync(id);
        if (existing == null || existing.UserId != userId)
            return NotFound();

        await _direccionService.DeleteDireccionAsync(id);
        return NoContent();
    }
}
