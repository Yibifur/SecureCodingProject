﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDose.FirstProject.DtoLayer.DoctorDtos
{
    public class RegisterDoctorDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Age { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Specialization { get; set; }
        public string LicenseNumber { get; set; }
        public string Hospital { get; set; }
    }
}