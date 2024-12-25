using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDose.FirstProject.DtoLayer.UserDtos
{
    public class UpdateUserDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Age { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
       
        public string? PasswordHash { get; set; }
        public string? Role { get; set; }
    }
}
