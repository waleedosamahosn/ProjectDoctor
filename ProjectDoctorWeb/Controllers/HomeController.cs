using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ProjectDoctorWeb.Models;
using ProjectDoctorWeb.ViewModel;
using System.Diagnostics;

namespace ProjectDoctorWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPatientRepository _patientRepository;

        public HomeController(ILogger<HomeController> logger, IPatientRepository patientRepository)
        {
            _logger = logger;
            _patientRepository = patientRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPatient(PatientViewModel model)
        {

            if (!ModelState.IsValid)
            {
                TempData["SuccessMessage"] = "error!";
                return BadRequest();
            }

            await _patientRepository.Create(model.Name, model.Email, model.Phone, model.DateRegisteration, model.Description);

            TempData["SuccessMessage"] = "submitted successfully!";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult CheckNumber(CheckNumberRequestViewModel request)
        {
            if (request.SecretNumber == "1234")
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.ErrorMessage = "The number is incorrect!";
                return View(nameof(Index));
            }
        }

           

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
