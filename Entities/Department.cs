using dep_manager_maven.Models;

namespace dep_manager_singleton.Entities
{
    public class Department : DepartmentInputModel
    {
        public Department()
        {
            IsDeleted = false;
        }
        public Department(string? name, string? acronym)
        {
            Name = name;
            Acronym = acronym;
        }
    }
}
