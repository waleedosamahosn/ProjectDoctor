using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ProjectDoctorWeb.ViewModel;

namespace ProjectDoctorWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPatientRepository _patientRepository;

        public AdminController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GetPatients()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            int pageSize = length != null ? int.Parse(length) : 0;
            int skip = start != null ? int.Parse(start) : 0;

            var patients = await _patientRepository.GetAllPatientsAsync();

            if (!string.IsNullOrEmpty(searchValue))
            {
                patients = patients
                    .Where(p => p.Name.Contains(searchValue) || p.Email.Contains(searchValue))
                    .ToList();
            }

            var recordsTotal = patients.Count;

            var data = patients.Skip(skip).Take(pageSize).ToList();

            return Json(new
            {
                draw = draw,
                recordsFiltered = recordsTotal,
                recordsTotal = recordsTotal,
                data = data
            });
        }
    }
}
