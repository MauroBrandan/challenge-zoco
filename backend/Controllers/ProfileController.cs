using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Models.DTOs;
using backend.Services.Interfaces;
using backend.Helpers;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IUserService _userService;

    public ProfileController(IUserService userService)
    {
        _userService = userService;
    }

    // GET /api/profile
    [HttpGet]
    public async Task<ActionResult<UserDetailResponseDTO>> GetProfile()
    {
        var userId = AuthorizationHelper.GetUserId(User);
        var user = await _userService.GetUserDetailByIdAsync(userId);
        if (user == null) return NotFound();
        return Ok(user);
    }

    // PUT /api/profile
    [HttpPut]
    public async Task<ActionResult<UserDetailResponseDTO>> UpdateProfile([FromBody] UpdateUserDTO dto)
    {
        var userId = AuthorizationHelper.GetUserId(User);

        try
        {
            var user = await _userService.UpdateUserAsync(userId, dto);
            if (user == null) return NotFound();

            var detail = await _userService.GetUserDetailByIdAsync(userId);
            return Ok(detail);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}
