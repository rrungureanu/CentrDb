using CentricaAPI.Contracts;
using CentricaAPI.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentricaAPI.Controllers
{
    [Route("api/centrica")]
    [ApiController]
    public class CentricaController : ControllerBase
    {
        private readonly ICentricaRepository _centricaRepo;
        public CentricaController(ICentricaRepository centricaRepo)
        {
            _centricaRepo = centricaRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetDistricts()
        {
            try
            {
                var districts = await _centricaRepo.GetDistricts();
                return Ok(districts);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}", Name = "DistrictById")]
        public async Task<IActionResult> GetDistrict(int id)
        {
            try
            {
                var district = await _centricaRepo.GetDistrict(id);
                if (district == null)
                    return NotFound();

                return Ok(district);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDistrict(DistrictCreationDto district)
        {
            try
            {
                var createdDistrict = await _centricaRepo.CreateDistrict(district);
                return CreatedAtRoute("DistrictById", new { id = createdDistrict.DistrictId }, createdDistrict);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDistrict(int id, DistrictUpdateDto district)
        {
            try
            {
                var dbDistrict = await _centricaRepo.GetDistrict(id);
                if (dbDistrict == null)
                    return NotFound();
                await _centricaRepo.UpdateDistrict(id, district);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDistrict(int id, DistrictUpdateDto district)
        {
            try
            {
                var dbDistrict = await _centricaRepo.GetDistrict(id);
                if (dbDistrict == null)
                    return NotFound();
                await _centricaRepo.DeleteDistrict(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetSalespersonsAndStoresByDistrictId/{id}")]
        public async Task<IActionResult> GetSalespersonsAndStoresByDistrictId(int id)
        {
            try
            {
                var district = await _centricaRepo.GetSalespersonsAndStoresByDistrictId(id);
                if (district == null)
                    return NotFound();
                return Ok(district);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("RemoveSalespersonFromDistrict/{salespersonId}/{districtId}")]
        public async Task<IActionResult> RemoveSalespersonFromDistrict(int salespersonId, int districtId)
        {
            try
            {
                await _centricaRepo.RemoveSalespersonFromDistrict(salespersonId, districtId);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetSalespersonsNotInDistrict/{id}")]
        public async Task<IActionResult> GetSalespersonsNotInDistrict(int id)
        {
            try
            {
                var salespersons = await _centricaRepo.GetSalespersonsNotInDistrict(id);
                return Ok(salespersons);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("AppendSalespersonToDistrict/{salespersonId}/{districtId}/{isPrimary}")]
        public async Task<IActionResult> AppendSalespersonToDistrict(int salespersonId, int districtId,bool isPrimary)
        {
            try
            {
                await _centricaRepo.AppendSalespersonToDistrict(salespersonId, districtId,isPrimary);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("AddNewSalespersonToDistrict/{familyName}/{firstName}/{phone}/{email}/{districtId}/{isPrimary}")]
        public async Task<IActionResult> AddNewSalespersonToDistrict(string familyName, string firstName, string phone, string email, int districtId, bool isPrimary)
        {
            try
            {
                await _centricaRepo.AddNewSalespersonToDistrict(familyName,firstName,phone,email,districtId,isPrimary);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
    }
}
