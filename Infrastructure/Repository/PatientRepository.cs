
using Application.Interfaces;
using Domain.Entites;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;
        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(string name, string email, string phone, DateTime dateRegisteration, string description)
        {
            var patient = new Patient()
            {
                Name = name,
                Email = email, 
                Phone = phone,
                DateRegisteration = dateRegisteration,
                Description = description
            };

            await _context.Patients.AddAsync(patient);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Patient>> GetAllPatientsAsync()
        {
            return await _context.Patients.ToListAsync();
        }

    }
}
