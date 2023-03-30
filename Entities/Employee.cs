using dep_manager_maven.Models;

namespace dep_manager_singleton.Entities
{
    public class Employee : EmployeeInputModel
    {
        public Employee()
        {
            IsDeleted = false;
        }

        public Employee(string? name, string? picture, string? rg, int? idDep)
        {
            Name = name;
            Picture = picture;
            Rg = rg;
            IdDepartment = idDep;
        }

    }
}