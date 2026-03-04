using Microsoft.AspNetCore.Mvc;
using backend.Models.DTOs;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using backend.Helpers;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]  // /api/users
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // GET /api/users
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetAll()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    // GET /api/users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDetailResponseDTO>> GetById(int id)
    {
        if (!AuthorizationHelper.IsOwnerOrAdmin(User, id))
            return Forbid();

        var user = await _userService.GetUserDetailByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    // POST /api/users
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserResponseDTO>> Create([FromBody] CreateUserDTO dto)
    {
        try
        {
            var user = await _userService.CreateUserAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // PUT /api/users/5
    [HttpPut("{id}")]
    public async Task<ActionResult<UserResponseDTO>> Update(int id, [FromBody] UpdateUserDTO dto)
    {
        if (!AuthorizationHelper.IsOwnerOrAdmin(User, id))
            return Forbid();

        try
        {
            var user = await _userService.UpdateUserAsync(id, dto);
            if (user == null) return NotFound();
            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // DELETE /api/users/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}