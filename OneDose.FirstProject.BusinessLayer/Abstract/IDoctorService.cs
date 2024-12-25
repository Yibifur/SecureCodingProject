using OneDose.FirstProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDose.FirstProject.BusinessLayer.Abstract
{
    public interface IDoctorService:IGenericService<Doctor>
    {
        Task<Doctor> TGetDoctorByEmailAsync(string Email);
    }
}
