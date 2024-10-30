using System.ComponentModel.DataAnnotations;

namespace ProjectDoctorWeb.ViewModel
{
    public class PatientViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        [EmailAddress]
        public string Email { get; set; } = null!;

        [Phone]
        public string Phone { get; set; } = null!;

        public DateTime DateRegisteration { get; set; } = DateTime.Now;
        public string Description { get; set; } = null!;
    }
}
