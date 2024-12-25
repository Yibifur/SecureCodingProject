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
    public class UserRepository : IUserDal
    {
        private readonly Context _context;

        public UserRepository(Context context)
        {
            _context = context;
        }

       

        public async Task DeleteAsync(int id)
        {
            var values = _context.Users.Find(id);
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values), "Entity not found");
            }
            _context.Users.Remove(values);
            await _context.SaveChangesAsync();
        }

        

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

       

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public User GetUserByEmail(string Email)
        {
            var value = _context.Users.Where(x=>x.Email==Email).FirstOrDefault();

            return value;
        }

        public async Task<User> GetUserByEmailAsync(string Email)
        {
            var value =await  _context.Users.Where(x => x.Email == Email).FirstOrDefaultAsync();

            return value;
        }

        

        public async Task InsertAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
           await _context.SaveChangesAsync();
        }

        

        public async Task UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
           await _context.SaveChangesAsync();
        }
    }
}
