using Microsoft.AspNetCore.Mvc;
using TheBlog.Common.Constants;
using TheBlog.Interfaces;

namespace TheBlog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceManager _resourceManager;

        public ResourceController(IResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        [HttpGet("Image")]
        [ResponseCache(Duration = 30, VaryByQueryKeys = new[] { "id" })]
        public IActionResult Image([FromQuery] string id)
        {
            var image = _resourceManager.GetImage(id);

            if (image == null || image.Bytes!.Length == 0)
            {
                return NotFound(ResourceErrors.NotFound);
            }

            return File(image.Bytes!, "image/png", image.NameInStorage);
        }
    }
}
