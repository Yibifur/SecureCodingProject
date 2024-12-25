using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneDose.FirstProject.BusinessLayer.Abstract;
using OneDose.FirstProject.DtoLayer.DoctorDtos;
using OneDose.FirstProject.EntityLayer.Concrete;

namespace OneDose.FirstProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _DoctorService;

        public DoctorsController(IDoctorService DoctorService)
        {
            _DoctorService = DoctorService;
        }

        [HttpGet]
        [Authorize(Roles = "doktor")]
        public async Task<IActionResult> GetAllDoctors()
        {

            var entities = await _DoctorService.TGetAllAsync();
            return Ok(entities);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {

            var entity = await _DoctorService.TGetByIdAsync(id);
            return Ok(entity);

        }


        [HttpPut]
        public async Task<IActionResult> UpdateDoctor(UpdateDoctorDto model)
        {
            Doctor Doctor = new Doctor()
            {
                DoctorId = model.DoctorId,
                Address = model.Address,
                Age = model.Age,
                Email = model.Email,
                Name = model.Name,
                Hospital = model.Hospital,
                PasswordHash = model.PasswordHash,
                Role = model.Role,
                Surname = model.Surname,
                LicenseNumber = model.LicenseNumber,
                Specialization=model.Specialization
                
            };

            await _DoctorService.TUpdateAsync(Doctor);
            return Ok("Doctor eklendi");

        }
        [HttpPost]
        public async Task<IActionResult> AddDoctor(CreateDoctorDto model)
        {
            Doctor Doctor = new Doctor()
            {
                
                Address = model.Address,
                Age = model.Age,
                Email = model.Email,
                Name = model.Name,
                Hospital = model.Hospital,
                PasswordHash = model.PasswordHash,
                Role = "doktor",
                Surname = model.Surname,
                LicenseNumber = model.LicenseNumber,
                Specialization = model.Specialization
                
            };

           await _DoctorService.TInsertAsync(Doctor);
            return Ok("Doctor eklendi");

        }
        [HttpDelete]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            await _DoctorService.TDeleteAsync(id);
            return Ok("Doctor silindi");

        }
    }
}
