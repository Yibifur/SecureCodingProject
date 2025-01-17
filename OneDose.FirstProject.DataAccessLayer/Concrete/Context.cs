﻿using Microsoft.EntityFrameworkCore;
using OneDose.FirstProject.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDose.FirstProject.DataAccessLayer.Concrete
{
    public class Context:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Trusted_Connection = True;TrustServerCertificate=True
            optionsBuilder.UseSqlServer(@"Server=localhost;Initial Catalog=OneDoseDb;Integrated Security=True;TrustServerCertificate=True");

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
    }
}
