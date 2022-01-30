using Microsoft.AspNetCore.Mvc;
using Yomu.Shared.Contexts;
using Yomu.Shared.Models;

namespace Yomu.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly YomuContext context;

    public UserController(YomuContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public IEnumerable<User> GetAll()
    {
        return context.Users;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get(string id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task Post(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }
    
    [HttpGet("{id}/post")]
    public IEnumerable<Post> GetPosts(string id)
    {
        return context.Posts.Where(x => x.UserId == id);
    }

    [HttpGet("{id}/comment")]
    public IEnumerable<Comment> GetComments(string id)
    {
        return context.Comments.Where(x => x.UserId == id);
    }

    [HttpPost("{id}/block")]
    public async Task PostBlock()
    {
        /* TODO: Implement this */
    }
    
    [HttpDelete("{id}/block")]
    public async Task DeleteBlock()
    {
        /* TODO: Implement this */
    }

    [HttpPost("{id}/friend")]
    public async Task PostFriend()
    {
        /* TODO: Implement this */
    }
    
    [HttpDelete("{id}/friend")]
    public async Task DeleteFriend()
    {
        /* TODO: Implement this */
    }
}
