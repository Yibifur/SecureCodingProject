using OneDose.FirstProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDose.FirstProject.DataAccessLayer.Abstract
{
    public interface IUserDal:IGenericDal<User> {

        Task<User> GetUserByEmailAsync(string Email);

        }
    

    
}
