
using Domain.Entites;

namespace Application.Interfaces
{
    public interface IPatientRepository
    {
        Task Create(string name, string email, string phone, DateTime dateRegisteration, string description);
        Task<List<Patient>> GetAllPatientsAsync();
    }
}
