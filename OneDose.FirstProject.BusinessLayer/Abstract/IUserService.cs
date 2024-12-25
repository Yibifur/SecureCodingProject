using OneDose.FirstProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDose.FirstProject.BusinessLayer.Abstract
{
    public interface IUserService:IGenericService<User>
    {
       Task<User> TGetUserByEmailAsync(string Email);
    }
}
