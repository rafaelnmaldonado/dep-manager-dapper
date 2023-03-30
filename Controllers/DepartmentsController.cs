using Dapper;
using dep_manager_maven.Models;
using dep_manager_singleton.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace dep_manager_singleton.Controllers
{
    [Route("api/departments")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly string _connectionString;

        public DepartmentsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ManagerCs");
        }

        /// <summary>
        /// Obter todos os departamentos
        /// </summary>
        /// <returns>Coleção de departamentos</returns>
        /// <response code="200">Sucesso</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                const string sql = "SELECT * FROM Departments WHERE IsDeleted = 0";

                var departments = await sqlConnection.QueryAsync<Department>(sql);

                return Ok(departments);
            }
        }

        /// <summary>
        /// Obter um departamento específico
        /// </summary>
        /// <param name="id">Identificador do departamento</param>
        /// <returns>Dados do departamento</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Departamento não encontrado</response>
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
                const string sql = "SELECT * FROM Departments WHERE Id = @id";

                var department = await sqlConnection.QuerySingleOrDefaultAsync<Department>(sql, parameters);

                if (department is null) return NotFound();

                return Ok(department);
            }
        }

        /// <summary>
        /// Cadastrar um departamento
        /// </summary>
        /// <remarks>
        /// { "name": "nome", "acronym": "sigla" }
        /// </remarks>
        /// <param name="department">Dados do departamento</param>
        /// <returns>Identificador do objeto criado</returns>
        /// <response code="200">Sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(DepartmentInputModel input)
        {
            var department = new Department(input.Name, input.Acronym);

            var parameters = new
            {
                department.Name,
                department.Acronym,
                department.IsDeleted
            };

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                const string sql = "INSERT INTO Departments OUTPUT INSERTED.Id VALUES (@Name, @Acronym, @IsDeleted)";

                var id = await sqlConnection.ExecuteScalarAsync<int>(sql, parameters);

                return Ok(id);
            }
        }

        /// <summary>
        /// Atualizar um departamento
        /// </summary>
        /// <remarks>
        /// { "name": "nome", "acronym": "sigla" }
        /// </remarks>
        /// <param name="id">Identificador do departamento</param>
        /// <param name="input">Dados do departamento</param>
        /// <returns>Void</returns>
        /// <response code="204">Sucesso</response>
        /// <response code="404">Departamento não encontrado</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, DepartmentInputModel input)
        {
            var parameters = new
            {
                id,
                input.Name,
                input.Acronym
            };

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                const string sql = "UPDATE Departments SET Name = @Name, Acronym = @Acronym WHERE Id = @Id";

                await sqlConnection.ExecuteAsync(sql, parameters);

                return NoContent();
            }
        }

        /// <summary>
        /// Excluir um departamento
        /// </summary>
        /// <param name="id">Identificador do departamento</param>
        /// <returns>Void</returns>
        /// <response code="204">Sucesso</response>
        /// <response code="404">Departamento não encontrado</response>
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
                const string sql = "UPDATE Departments SET IsDeleted = 1 WHERE Id = @Id";

                await sqlConnection.ExecuteAsync(sql, parameters);

                return NoContent();
            }
        }
    }
}
