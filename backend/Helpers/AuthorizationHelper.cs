using System.Security.Claims;

namespace backend.Helpers;

public static class AuthorizationHelper
{
    public static int GetUserId(ClaimsPrincipal user)
    {
        var claim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return claim != null ? int.Parse(claim) : 0;
    }

    public static bool IsAdmin(ClaimsPrincipal user)
    {
        return user.IsInRole("Admin");
    }

    public static bool IsOwnerOrAdmin(ClaimsPrincipal user, int resourceUserId)
    {
        return IsAdmin(user) || GetUserId(user) == resourceUserId;
    }
}