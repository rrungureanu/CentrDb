using CentricaAPI.Entities;
using CentricaAPI.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentricaAPI.Contracts
{
    public interface ICentricaRepository
    {
        public Task<IEnumerable<District>> GetDistricts();
        public Task<District> GetDistrict(int id);
        public Task<District> CreateDistrict(DistrictCreationDto district);
        public Task UpdateDistrict(int id, DistrictUpdateDto district);
        public Task DeleteDistrict(int id);
        public Task<District> GetSalespersonsAndStoresByDistrictId(int id);
        public Task RemoveSalespersonFromDistrict(int salespersonId, int districtId);
        public Task<IEnumerable<Salesperson>> GetSalespersonsNotInDistrict(int id);
        public Task AppendSalespersonToDistrict(int salespersonId, int districtId, bool isPrimary);
        public Task AddNewSalespersonToDistrict(string familyName, string firstName, string phone, string email, int districtId, bool isPrimary);
    }
}
