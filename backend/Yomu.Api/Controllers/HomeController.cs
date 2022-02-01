using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yomu.Api.Extensions;
using Yomu.Shared.Contexts;
using Yomu.Shared.Models;

namespace Yomu.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    private readonly YomuContext context;

    public HomeController(YomuContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        return await this.WithCurrentUser(context, async currentUser =>
        {
            return Ok(context.Posts
                .Include(x => x.Community)
                .Include(x => x.Community.UserCommunities)
                .Where(x => x.Community.UserCommunities.Any(x => x.UserId == currentUser.Id))
                .OrderByDescending(x => x.Id)
                .Take(50));
        });
    }
}
