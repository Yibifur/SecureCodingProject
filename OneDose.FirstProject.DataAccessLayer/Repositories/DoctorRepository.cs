using Microsoft.EntityFrameworkCore;
using OneDose.FirstProject.DataAccessLayer.Abstract;
using OneDose.FirstProject.DataAccessLayer.Concrete;
using OneDose.FirstProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDose.FirstProject.DataAccessLayer.Repositories
{
    public class DoctorRepository : IDoctorDal
    {
        private readonly Context _context;

        public DoctorRepository(Context context)
        {
            _context = context;
        }

        public async Task DeleteAsync(int id)
        {
            var values = await _context.Doctors.FindAsync(id);
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values), "Entity not found");
            }
            _context.Doctors.Remove(values);
           await _context.SaveChangesAsync();
        }

        public async Task< List<Doctor>> GetAllAsync()
        {
            return await _context.Doctors.ToListAsync();
        }

        public async Task< Doctor> GetByIdAsync(int id)
        {
            return await _context.Doctors. FindAsync(id);
        }

        

        public async Task<Doctor> GetDoctorByEmailAsync(string Email)
        {
            var value = await  _context.Doctors.Where(x => x.Email == Email).FirstOrDefaultAsync();

            return value;
        }

        public async Task InsertAsync(Doctor entity)
        {
            await _context.Doctors.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Doctor entity)
        {
            _context.Doctors.Update(entity);
           await _context.SaveChangesAsync();
        }

        
    }
}
