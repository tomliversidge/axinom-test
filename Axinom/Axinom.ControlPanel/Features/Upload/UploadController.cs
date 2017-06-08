using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Axinom.ControlPanel.Features.Upload
{
    public class UploadController : Controller
    {
        private readonly IMediator _mediator;

        public UploadController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Post(UploadModel model)
        {           
            // TODO log errors
            // TODO better error handling
            if (!ModelState.IsValid) return RedirectToAction("Index", "Home", model);
            var response = await _mediator.Send(model);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);
            return Ok(response.StatusCode);
        }
    }
}
