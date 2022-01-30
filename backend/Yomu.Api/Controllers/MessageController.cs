using Microsoft.AspNetCore.Mvc;
using Yomu.Api.Extensions;
using Yomu.Shared.Contexts;
using Yomu.Shared.Models;

namespace Yomu.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    private readonly YomuContext context;

    public MessageController(YomuContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public IDictionary<string, IEnumerable<Message>> GetAllMessages()
    {
        var messages = new Dictionary<string, IEnumerable<Message>>();

        this.WithCurrentUser(context, (currentUser) =>
        {
            var raw = context.Messages
                .Where(x => x.SenderId == currentUser.Id || x.ReceiverId == currentUser.Id)
                .GroupBy(x => x.SenderId == currentUser.Id ? x.ReceiverId : x.SenderId);

            foreach (var g in raw)
            {
                messages.Add(g.Key, g);
            }
            
            return Task.FromResult<ActionResult>(Ok());
        }).RunSynchronously(); // hack
        
        return messages;
    }

    [HttpGet("userId")]
    public IEnumerable<Message> GetMessages(string userId)
    {
        var messages = Enumerable.Empty<Message>();

        this.WithCurrentUser(context, (currentUser) =>
        {
            messages = context.Messages.Where(x =>
                                               (x.SenderId == currentUser.Id && x.ReceiverId == userId) ||
                                               (x.SenderId == userId && x.ReceiverId == currentUser.Id));
            
            return Task.FromResult<ActionResult>(Ok());
        }).RunSynchronously(); // hack
        
        return messages;
     }

    [HttpPost("userId")]
    public async Task<ActionResult> PostMessages(string userId, string message)
    {
        return await this.WithCurrentUser(context, async (currentUser) =>
        {
            var msg = new Message()
            {
                Id = 0,
                SenderId = currentUser.Id,
                ReceiverId = userId,
                SendAt = DateTime.Now,
                Text = message                
            };

            await context.Messages.AddAsync(msg);
            await context.SaveChangesAsync();
            
            return Ok();
        });
    }
}
