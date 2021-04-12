using System.Threading.Tasks;
using BrainstormSessions.Core.Interfaces;
using BrainstormSessions.Services;
using BrainstormSessions.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BrainstormSessions.Controllers
{
    public class SessionController : Controller
    {
        private readonly IBrainstormSessionRepository _sessionRepository;
        private readonly ILogger _logger;

        public SessionController(IBrainstormSessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
            _logger = LoggerManager.GetLogger<SessionController>();
        }

        public async Task<IActionResult> Index(int? id)
        {
            _logger.Debug("Session controller index call");

            if (!id.HasValue)
            {
                _logger.Warn($"Invalid id.");
                return RedirectToAction(actionName: nameof(Index),
                    controllerName: "Home");
            }

            var session = await _sessionRepository.GetByIdAsync(id.Value);
            if (session == null)
            {
                _logger.Warn($"Session not found. Id: {id.Value}");
                return Content("Session not found.");
            }

            var viewModel = new StormSessionViewModel()
            {
                DateCreated = session.DateCreated,
                Name = session.Name,
                Id = session.Id
            };

            _logger.Debug("All flow is correct");

            return View(viewModel);
        }
    }
}
