using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yomu.Shared.Contexts;
using Yomu.Shared.Models;

namespace Yomu.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ReportController : ControllerBase
{
    private readonly YomuContext context;

    public ReportController(YomuContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public IEnumerable<Report> GetAll()
    {
        return context.Reports;
    }

    [HttpPost]
    public async Task<int> Post(Report report)
    {
        report.Id = 0;
        var entry = await context.Reports.AddAsync(report);
        await context.SaveChangesAsync();

        return entry.Entity.Id;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {        
        var report = await context.Reports.FindAsync(id);
        if (report == null)
        {
            return NotFound();
        }
                
        context.Reports.Remove(report);
        await context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("{id}/user")]
    public async Task<ActionResult<IEnumerable<User>>> GetUser(int id)
    {        
        var report = await context.Reports
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (report == null)
        {
            return NotFound();
        }
                
        return Ok(report.Users);
    }

    [HttpGet("{id}/comment")]
    public async Task<ActionResult<IEnumerable<Comment>>> GetComments(int id)
    {        
        var report = await context.Reports
            .Include(x => x.Comments)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (report == null)
        {
            return NotFound();
        }
                
        return Ok(report.Comments);
    }

    [HttpGet("{id}/post")]
    public async Task<ActionResult<IEnumerable<Post>>> GetPosts(int id)
    {        
        var report = await context.Reports
            .Include(x => x.Posts)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (report == null)
        {
            return NotFound();
        }
                
        return Ok(report.Posts);
    }
}
