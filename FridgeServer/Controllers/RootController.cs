using Entities.LinkModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;

namespace FridgeServer.Controllers
{
    [Route("api")]
    [ApiController]
    public class RootController : ControllerBase
    {
        private readonly LinkGenerator _linkGenerator;

        public RootController(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }

        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
        {
            if (mediaType.Contains("application/vnd.fridge.apiroot"))
            {
                var list = new List<Link>
                {
                    new Link
                    {
                        Href = _linkGenerator.GetUriByName(HttpContext, nameof(GetRoot),
                            new {}),
                        Rel = "self",
                        Method = "GET"
                    },

                    new Link
                    {
                        Href = _linkGenerator.GetUriByName(HttpContext, "GetFridges", new {}),
                        Rel = "fridges",
                        Method = "GET"
                    },

                    new Link
                    {
                        Href = _linkGenerator.GetUriByName(HttpContext, "CreateFridge", new {}),
                        Rel = "create_fridge",
                        Method = "POST"
                    }
                };

                return Ok(list);
            }

            return NoContent();
        }
    }
}
