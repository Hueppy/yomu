using Microsoft.AspNetCore.Mvc;
using Yomu.Api.Services;
using Yomu.Shared.Contexts;

namespace Yomu.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ImageController : ControllerBase
{
	private YomuContext context;
    private ImageService images;

	public ImageController(
        YomuContext context,
        ImageService images)
	{
		this.context = context;
        this.images = images;
	}

    [HttpGet("{id}")]
    public async Task<ActionResult> Get(string id)
    {
        using var stream = new MemoryStream();
        var contentType = await images.LoadImage(id, stream);
        if (contentType == null)
        {
            return BadRequest();
        }
                
        return new FileContentResult(stream.GetBuffer(), contentType);
    }
}
