using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OneDose.FirstProject.BusinessLayer.Abstract;
using OneDose.FirstProject.DtoLayer.UserDtos;
using OneDose.FirstProject.EntityLayer.Concrete;
using OneDose.FirstProject.WebAPI.Model;
using OneDose.FirstProject.WebAPI.Security;
using System.Collections.Generic;
using System.Net;

namespace OneDose.FirstProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : CustomBaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /* [HttpGet]
         [Authorize]

         public async Task<IActionResult> GetAllUsers()
         {


                 var entities = await _userService.TGetAllAsync();

                 return Ok(entities);

         }*/
        
        [HttpGet]
        [Authorize]
        public async Task<ReelResponse<List<User>>> GetAllUsers()
        {
            var entities = await _userService.TGetAllAsync();

            if (entities.Any())
            {
                return ReelResponse<List<User>>.Success(entities, 200);
            }
            else
            {
                return ReelResponse<List<User>>.Fail("herhangi bir kulanıcı bulunamadı",404);
            }
            
          
        }
        [HttpGet("{id}")]
        
        public async Task<ReelResponse<User>> GetUserById(int id)
        {
            var entities = await _userService.TGetAllAsync();
            if (id <= 0)
            {
              
                return ReelResponse<User>.Fail("herhangi bir kulanıcı bulunamadı", 404);
            }
            var entity = await _userService.TGetByIdAsync(id);
            return ReelResponse<User>.Success(entity, 200);  
             
        }


        [HttpPut]
        
        public async Task<ReelResponse<object>> UpdateUser(UpdateUserDto model)
        {
            if (!ModelState.IsValid||model.UserId<=0) { 
            
           return  ReelResponse<object>.Fail("User is null", 404);
                
            
        }
          
            User user = new User()
            {
                UserId = model.UserId,
                Address = model.Address,
                Age = model.Age,
                Email = model.Email,
                Name = model.Name,
               
                PasswordHash = model.PasswordHash,
                Role = model.Role,
                Surname = model.Surname
            };
            
           await _userService.TUpdateAsync(user);
           return  ReelResponse<object>.Success(200);
            

        }
        [HttpDelete]
       
        public async Task<ReelResponse<object>> DeleteUser(int id)
        {
            if (id <= 0)
            {
                return ReelResponse<object>.Fail("User not found", 404);
               
            }
            
            await _userService.TDeleteAsync(id);
            return ReelResponse<object>.Success(200);
           

        }
    }
}
