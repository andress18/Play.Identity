
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Play.Identity.Contracts;
using Play.Identity.Service.Entities;
using static Duende.IdentityServer.IdentityServerConstants;

namespace Play.Identity.Service.Controllers;

[ApiController]
[Route("users")]
[Authorize(Policy =LocalApi.PolicyName, Roles = Roles.Admin)]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPublishEndpoint _publishEndpoint;

    public UsersController(
        UserManager<ApplicationUser> userManager, IPublishEndpoint publishEndpoint)
    {
        _userManager = userManager;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> Get()
    {
        var users = _userManager.Users.ToList().Select(user => user.AsDto());
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetByIdAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user.AsDto());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(Guid id, UpdateUserDto userDto)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null)
        {
            return NotFound();
        }

        user.Email = userDto.Email;
        user.UserName = userDto.Email;
        user.Gil = userDto.Gil;

        await _userManager.UpdateAsync(user);
        await _publishEndpoint.Publish(new UserUpdated(user.Id, user.Email, user.Gil));

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null)
        {
            return NotFound();
        }

        await _userManager.DeleteAsync(user);
        await _publishEndpoint.Publish(new UserUpdated(user.Id, user.Email, 0));

        return NoContent();
    }

}