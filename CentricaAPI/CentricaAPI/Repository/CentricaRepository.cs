using Dapper;
using CentricaAPI.Context;
using CentricaAPI.Contracts;
using CentricaAPI.Dto;
using CentricaAPI.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace CentricaAPI.Repository
{
    public class CentricaRepository : ICentricaRepository
    {
        private readonly DapperContext _context;

        public CentricaRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<District>> GetDistricts()
        {
            var query = "SELECT * FROM commerce.districts";

            using (var connection = _context.CreateConnection())
            {
                var districts = await connection.QueryAsync<District>(query);
                return districts.ToList();
            }
        }

        public async Task<District> GetDistrict(int id)
        {
            var query = "SELECT * FROM commerce.districts WHERE districtId = @id";

            using (var connection = _context.CreateConnection())
            {
                var district = await connection.QuerySingleOrDefaultAsync<District>(query, new { id });
                return district;
            }
        }

        public async Task<District> CreateDistrict(DistrictCreationDto district)
        {
            var query = "INSERT INTO commerce.districts (primarySalespersonId, name) VALUES (@primarySalespersonId, @name)" +
                "SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new DynamicParameters();
            parameters.Add("primarySalespersonId", district.PrimarySalespersonId, DbType.Int32);
            parameters.Add("name", district.Name, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);

                var createdDistrict = new District
                {
                    DistrictId = id,
                    PrimarySalespersonId = district.PrimarySalespersonId,
                    Name = district.Name
                };

                return createdDistrict;
            }
        }

        public async Task UpdateDistrict(int id, DistrictUpdateDto district)
        {
            var query = "UPDATE commerce.districts SET name = @Name, primarySalespersonId = @primarySalespersonId WHERE districtId = @id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("primarySalespersonId", district.PrimarySalespersonId, DbType.Int32);
            parameters.Add("name", district.Name, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task DeleteDistrict(int id)
        {
            var query = "DELETE FROM commerce.districts WHERE districtId = @id";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id });
            }
        }

        public async Task<District> GetSalespersonsAndStoresByDistrictId(int id)
        {
            var procedureName = "commerce.getSalespersonsAndStoresByDistrictId";
            var parameters = new DynamicParameters();
            parameters.Add("districtId", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = _context.CreateConnection())
            {
                using (var multi = await connection.QueryMultipleAsync(procedureName, parameters, commandType: CommandType.StoredProcedure))
                {
                    District district = new District();
                    district.Salespersons = (await multi.ReadAsync<Salesperson>()).ToList();
                    district.Stores = (await multi.ReadAsync<Store>()).ToList();
                    return district;
                }
            }
        }

        public async Task RemoveSalespersonFromDistrict(int salespersonId, int districtId)
        {
            var procedureName = "commerce.removeSalespersonFromDistrict";
            var parameters = new DynamicParameters();
            parameters.Add("salespersonId", salespersonId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("districtId", districtId, DbType.Int32, ParameterDirection.Input);

            using (var connection = _context.CreateConnection())
            {
                await connection.QueryFirstOrDefaultAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Salesperson>> GetSalespersonsNotInDistrict(int id)
        {
            var procedureName = "commerce.getSalespersonsNotInDistrict";
            var parameters = new DynamicParameters();
            parameters.Add("districtId", id, DbType.Int32, ParameterDirection.Input);

            using (var connection = _context.CreateConnection())
            {
               var salespersons = await connection.QueryAsync<Salesperson>(procedureName, parameters, commandType: CommandType.StoredProcedure);
               return salespersons.ToList();
            }
        }

        public async Task AppendSalespersonToDistrict(int salespersonId, int districtId, bool isPrimary)
        {
            var procedureName = "commerce.appendSalespersonToDistrict";
            var parameters = new DynamicParameters();
            parameters.Add("salespersonId", salespersonId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("districtId", districtId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("isPrimary", isPrimary, DbType.Boolean, ParameterDirection.Input);

            using (var connection = _context.CreateConnection())
            {
                await connection.QueryFirstOrDefaultAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task AddNewSalespersonToDistrict(string familyName, string firstName, string phone, string email, int districtId, bool isPrimary)
        {
            var procedureName = "commerce.addNewSalespersonToDistrict";
            var parameters = new DynamicParameters();
            parameters.Add("familyName", familyName, DbType.String, ParameterDirection.Input);
            parameters.Add("firstName", firstName, DbType.String, ParameterDirection.Input);
            parameters.Add("phone", phone, DbType.String, ParameterDirection.Input);
            parameters.Add("email", email, DbType.String, ParameterDirection.Input);
            parameters.Add("districtId", districtId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("isPrimary", isPrimary, DbType.Boolean, ParameterDirection.Input);

            using (var connection = _context.CreateConnection())
            {
                await connection.QueryFirstOrDefaultAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
