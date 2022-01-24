using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yomu.Api.Extensions;
using Yomu.Api.Services;
using Yomu.Shared.Contexts;
using Yomu.Shared.Models;

namespace Yomu.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly YomuContext context;
    private readonly ImageService images;

    public PostController(YomuContext context, ImageService images)
    {
        this.context = context;
        this.images = images;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> GetPost(int id)
    {
        var post = await this.context.Posts.FindAsync(id);
        if (post == null)
        {
            return NotFound();
        }

        return post;
    }

    [HttpGet("{id}/comment")]
    public IEnumerable<Comment> GetComments(int id)
    {
        return this.context.Comments.Where(x => x.PostId == id);
    }
    
    [HttpPost("{id}/comment")]
    public async Task<ActionResult> PostComment(int id, Comment comment)
    {
        comment.PostId = id;
        
        await this.context.Comments.AddAsync(comment);
        await this.context.SaveChangesAsync();

        return Ok();
    }
    
    [HttpGet("{id}/rating")]
    public IEnumerable<PostRating> GetRating(int id)
    {
        return this.context.PostRatings.Where(x => x.PostId == id);
    }
    
    [HttpPost("{id}/rating")]
    public async Task<ActionResult> PostRating(int id, Rating rating)
    {
        return await this.WithCurrentUser(context, async (currentUser) =>
        {
            var rtng = new PostRating()
            {
                PostId = id,
                UserId = currentUser.Id,
                Rating = rating
            };

            await context.PostRatings.AddAsync(rtng);
            await context.SaveChangesAsync();

            return Ok();
        });
    }
    
    [HttpDelete("{id}/rating")]
    public async Task<ActionResult> DeleteRating(int id)
    {
        return await this.WithCurrentUser(context, async (currentUser) =>
        {
            var rating = context.PostRatings.FirstOrDefault(x => x.PostId == id && x.UserId == currentUser.Id);
            if (rating == null)
            {
                return NotFound();
            }

            context.PostRatings.Remove(rating);
            await context.SaveChangesAsync();

            return Ok();
        });
    }

    [HttpGet("{id}/image")]
    public async Task<ActionResult<IEnumerable<Image>>> GetImages(int id)
    {
        var post = await this.context.Posts
            .Include(x=>x.Images)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (post == null)
        {
            return NotFound();
        }

        return Ok(post.Images);
    }
    
    [HttpPost("{id}/image")]
    public async Task<ActionResult> PostImage(int id, IFormFile file)
    {
        string? name = null;
        using (var s = file.OpenReadStream())
        {
            name = await images.SaveImage(s, file.ContentType);
        }

        if (name == null)
        {
            return BadRequest();
        }
        
        var image = new Image()
        {
            Id = name,
            PostId = id,
        };

        await this.context.Images.AddAsync(image);
        await this.context.SaveChangesAsync();

        return Ok();
    }
}
