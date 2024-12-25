using OneDose.FirstProject.BusinessLayer.Abstract;
using OneDose.FirstProject.DataAccessLayer.Abstract;
using OneDose.FirstProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDose.FirstProject.BusinessLayer.Concrete
{
    public class DoctorManager:IDoctorService
    {
        private readonly IDoctorDal _DoctorDal;

        public DoctorManager(IDoctorDal DoctorDal)
        {
            _DoctorDal = DoctorDal;
        }

        public async Task TDeleteAsync(int id)
        {
           await _DoctorDal.DeleteAsync(id);
        }

        public async Task<List<Doctor>> TGetAllAsync()
        {
             return await _DoctorDal.GetAllAsync();
        }

        public async Task<Doctor> TGetByIdAsync(int id)
        {
              return await _DoctorDal.GetByIdAsync(id);
        }

        public async Task<Doctor> TGetDoctorByEmailAsync(string Email)
        {
             return await _DoctorDal.GetDoctorByEmailAsync(Email);
        }

        public async Task TInsertAsync(Doctor entity)
        {
            await _DoctorDal.InsertAsync(entity);
        }

        public async Task TUpdateAsync(Doctor entity)
        {
           await _DoctorDal.UpdateAsync(entity);
        }
    }
}
