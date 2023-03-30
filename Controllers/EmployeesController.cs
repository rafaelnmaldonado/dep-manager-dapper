using Dapper;
using dep_manager_maven.Models;
using dep_manager_singleton.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace dep_manager_singleton.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : Controller
    {
        private readonly string _connectionString;

        public EmployeesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ManagerCs");
        }

        /// <summary>
        /// Obter todos os colaboradores
        /// </summary>
        /// <returns>Coleção de colaboradores</returns>
        /// <response code="200">Sucesso</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                const string sql = "SELECT * FROM Employees WHERE IsDeleted = 0";

                var employees = await sqlConnection.QueryAsync<Employee>(sql);

                return Ok(employees);
            }
        }

        /// <summary>
        /// Obter todos os colaboradores associados a um departamento
        /// </summary>
        /// <param name="idDep">Identificador do departamento</param>
        /// <returns>Coleção de colaboradores</returns>
        /// <response code="200">Sucesso</response>
        [HttpGet("dep/{idDep}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByDep(int idDep)
        {
            var parameters = new
            {
                idDep
            };

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                const string sql = "SELECT * FROM Employees WHERE IsDeleted = 0 AND IdDepartment = @idDep";

                var employees = await sqlConnection.QueryAsync<Employee>(sql, parameters);

                return Ok(employees);
            }
        }

        /// <summary>
        /// Obter um colaborador específico
        /// </summary>
        /// <param name="id">Identificador do colaborador</param>
        /// <returns>Dados do colaborador</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Colaborador não encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var parameters = new
            {
                id
            };

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                const string sql = "SELECT * FROM Employees WHERE Id = @id";

                var employee = await sqlConnection.QuerySingleOrDefaultAsync<Employee>(sql, parameters);

                if (employee is null) return NotFound();

                return Ok(employee);
            }
        }

        /// <summary>
        /// Cadastrar um colaborador
        /// </summary>
        /// <remarks>
        /// { "name": "nome", "picture": "foto", "rg": "XX.XXX.XXX-X", idDepartment: "Guid" }
        /// </remarks>
        /// <param name="employee">Dados do colaborador</param>
        /// <returns>Identificador do objeto criado</returns>
        /// <response code="200">Sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(EmployeeInputModel input)
        {
            var employee = new Employee(input.Name, input.Picture, input.Rg, input.IdDepartment);

            var parameters = new
            {
                employee.Name,
                employee.Picture,
                employee.Rg,
                employee.IdDepartment,
                employee.IsDeleted
            };

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                const string sql = "INSERT INTO Employees OUTPUT INSERTED.Id VALUES (@Name, @Picture, @Rg, @IdDepartment, @IsDeleted)";

                var id = await sqlConnection.ExecuteScalarAsync<int>(sql, parameters);

                return Ok(id);
            }
        }

        /// <summary>
        /// Atualiza um colaborador
        /// </summary>
        /// <remarks>
        /// { "name": "nome", "picture": "foto", "rg": "XX.XXX.XXX-X", idDepartment: "Guid" }
        /// </remarks>
        /// <param name="id">Identificador do colaborador</param>
        /// <param name="input">Dados do colaborador</param>
        /// <returns>Void</returns>
        /// <response code="204">Sucesso</response>
        /// <response code="404">Colaborador não encontrado</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, EmployeeInputModel input)
        {
            var parameters = new
            {
                id,
                input.Name,
                input.Picture,
                input.Rg,
                input.IdDepartment
            };

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                const string sql = "UPDATE Employees SET Name = @Name, Picture = @Picture, Rg = @Rg, IdDepartment = @IdDepartment WHERE Id = @Id";

                await sqlConnection.ExecuteAsync(sql, parameters);

                return NoContent();
            }
        }

        /// <summary>
        /// Deletar um colaborador
        /// </summary>
        /// <param name="id">Identificador do colaborador</param>
        /// <returns>Void</returns>
        /// <response code="204">Sucesso</response>
        /// <response code="404">Colaborador não encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var parameters = new
            {
                id
            };

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                const string sql = "UPDATE Employees SET IsDeleted = 1 WHERE Id = @Id";

                await sqlConnection.ExecuteAsync(sql, parameters);

                return NoContent();
            }
        }
    }
}
