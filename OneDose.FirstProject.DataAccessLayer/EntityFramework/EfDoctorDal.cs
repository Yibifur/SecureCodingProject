using OneDose.FirstProject.DataAccessLayer.Abstract;
using OneDose.FirstProject.DataAccessLayer.Concrete;
using OneDose.FirstProject.DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDose.FirstProject.DataAccessLayer.EntityFramework
{
    public class EfDoctorDal : DoctorRepository, IDoctorDal
    {
        public EfDoctorDal(Context context) : base(context)
        {
        }


    }
}
