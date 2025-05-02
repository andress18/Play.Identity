using System;

namespace Play.Identity.Service;

public static class Extensions
{
    public static UserDto AsDto(this Entities.ApplicationUser user)
    {
        return new UserDto(
            user.Id,
            user.UserName,
            user.Email,
            user.Gil,
            user.CreatedOn
        );
    }
}
