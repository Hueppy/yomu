using Microsoft.AspNetCore.Mvc;
using Yomu.Shared.Contexts;
using Yomu.Shared.Models;

namespace Yomu.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchController : ControllerBase
{
    private readonly YomuContext context;

    public SearchController(YomuContext context)
    {
        this.context = context;
    }

    [HttpGet("community")]
    public IEnumerable<Community> GetCommunity(string query)
    {
        return context.Communities.Where(x => x.Id.Contains(query));
    }

    [HttpGet("post")]
    public IEnumerable<Post> GetPost(string? community, string query)
    {
        var results = context.Posts.Where(x => x.Text.Contains(query));
        if (community != null)
        {
            results = results.Where(x => x.CommunityId == community);
        }
        
        return results;
    }

    [HttpGet("user")]
    public IEnumerable<User> GetUser(string query)
    {
        return context.Users.Where(x => x.Id.Contains(query));
    }
}
