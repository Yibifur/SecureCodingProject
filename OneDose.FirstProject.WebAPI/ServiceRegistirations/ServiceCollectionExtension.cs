using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OneDose.FirstProject.BusinessLayer.Abstract;
using OneDose.FirstProject.BusinessLayer.Concrete;
using OneDose.FirstProject.DataAccessLayer.Abstract;
using OneDose.FirstProject.DataAccessLayer.Concrete;
using OneDose.FirstProject.DataAccessLayer.EntityFramework;
using OneDose.FirstProject.WebAPI.Caching;
using OneDose.FirstProject.WebAPI.Model;
using OneDose.FirstProject.WebAPI.Security;
using OneDose.FirstProject.WebAPI.Security.Abstract;
using OneDose.FirstProject.WebAPI.Security.Handlers;
using System.Text;

namespace OneDose.FirstProject.WebAPI.ServiceRegistirations
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {

            services.AddDbContext<Context>();
           
            services.AddScoped<IUserDal, EfUserDal>();
            services.AddScoped<IUserService, UserManager>();
            services.AddScoped<IDoctorDal,EfDoctorDal>();
            services.AddScoped<IDoctorService, DoctorManager>();
            services.AddScoped<ITokenHandler, Security.Handlers.TokenHandler>();
            services.AddScoped<SessionValidationService>();
            
            
            services.AddHttpContextAccessor();
            services.AddSingleton<IAuthorizationHandler,SessionHandler>();
         





            return services;
        }
    }
}
