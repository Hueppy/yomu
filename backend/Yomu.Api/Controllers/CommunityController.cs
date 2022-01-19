using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yomu.Api.Extensions;
using Yomu.Shared.Contexts;
using Yomu.Shared.Models;

namespace Yomu.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CommunityController : ControllerBase
{
    private readonly YomuContext context;

    public CommunityController(YomuContext context)
    {
        this.context = context;
    }

    private bool HasCommunityRole(string id, string userId, Role role)
    {
        var user = context.UserCommunities.FirstOrDefault(x => x.CommunityId == id && x.UserId == userId);
        return user?.Role >= role;
    }

    [HttpGet()]
    public IEnumerable<Community> GetAll()
    {
        return this.context.Communities;
    }
    
    [HttpGet("{id}")]
    public Community? Get(string id)
    {
        return this.context.Communities.FirstOrDefault(x => x.Id == id);
    }

    [HttpPost]
    public async Task Post(Community community)
    {
        await this.context.AddAsync(community);
        await this.context.SaveChangesAsync();
    }
    
    [HttpPatch("{id}")]
    public async Task Patch(string id, Community community)
    {
        community.Id = id;
        
        this.context.Update(community);
        await this.context.SaveChangesAsync();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var community = this.context.Communities.FirstOrDefault(x => x.Id == id);
        if (community == null)
        {
            return NotFound();
        }

        this.context.Communities.Remove(community);
        await this.context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("{id}/post")]
    public async Task<ActionResult<IEnumerable<Post>>> GetPosts(string id)
    {
        var community = await this.context.Communities
            .Include(x => x.Posts)
            .FirstAsync(x => x.Id == id);
        
        if (community == null)
        {
            return NotFound();
        }

        return Ok(community.Posts);
    }
    
    [HttpPost("{id}/post")]
    public async Task<ActionResult> PostPost(string id, Post post)
    {
        var community = await this.context.Communities
            .Include(x => x.Posts)
            .FirstAsync(x => x.Id == id);
        
        if (community == null)
        {
            return NotFound();
        }

        post.CommunityId = community.Id;

        await this.context.Posts.AddAsync(post);
        await this.context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("{id}/user")]
    public IEnumerable<UserCommunity> GetUser(string id)
    {
        return context.UserCommunities.Where(x => x.CommunityId == id);
    }

    [HttpPost("{id}/user")]
    public async Task<ActionResult> PostUser(string id)
    {
        return await this.WithCurrentUser(context, async (currentUser) =>
        {
            var user = new UserCommunity
            {
                CommunityId = id,
                UserId = currentUser.Id,
                Role = Role.User
            };

            await context.UserCommunities.AddAsync(user);
            await context.SaveChangesAsync();
            
            return Ok();
        });
    }

    [HttpPatch("{id}/user/{userId}")]
    public async Task<ActionResult> PatchUserRole(string id, string userId, Role role)
    {
        return await this.WithCurrentUser(context, async (currentUser) =>
        {
            if (!HasCommunityRole(id, currentUser.Id, role))
            {
                return Forbid();
            }

            var user = context.UserCommunities.FirstOrDefault(x => x.CommunityId == id && x.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            if (user.Role >= role)
            {
                return BadRequest();
            }

            user.Role = role;
            context.UserCommunities.Update(user);
            await context.SaveChangesAsync();

            return Ok();
        });
    }
}
