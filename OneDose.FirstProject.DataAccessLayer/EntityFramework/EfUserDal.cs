using OneDose.FirstProject.DataAccessLayer.Abstract;
using OneDose.FirstProject.DataAccessLayer.Concrete;
using OneDose.FirstProject.DataAccessLayer.Repositories;
using OneDose.FirstProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDose.FirstProject.DataAccessLayer.EntityFramework
{
    public class EfUserDal : UserRepository, IUserDal
    {
        public EfUserDal(Context context) : base(context)
        {
        }

        
    }
}
