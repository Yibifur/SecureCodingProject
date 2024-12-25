using OneDose.FirstProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDose.FirstProject.DataAccessLayer.Abstract
{
    public interface IDoctorDal : IGenericDal<Doctor>
    {

        Task<Doctor> GetDoctorByEmailAsync(string Email);

    }
}
