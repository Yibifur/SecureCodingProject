using OneDose.FirstProject.BusinessLayer.Abstract;
using OneDose.FirstProject.DataAccessLayer.Abstract;
using OneDose.FirstProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace OneDose.FirstProject.BusinessLayer.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public async Task TDeleteAsync(int id)
        {
           await _userDal.DeleteAsync(id);
        }

        public async Task<List<User>> TGetAllAsync()
        {
            return await _userDal.GetAllAsync();
        }

        public async Task<User>TGetByIdAsync(int id)
        {
            return await _userDal.GetByIdAsync(id);
        }

        public async Task<User>TGetUserByEmailAsync(string Email)
        {
           return await _userDal.GetUserByEmailAsync(Email);
        }

        public async Task TInsertAsync(User entity)
        {
            
            await _userDal.InsertAsync(entity);
        }

        public async Task TUpdateAsync(User entity)
        {
          await _userDal.UpdateAsync(entity);
        }
    }
}
