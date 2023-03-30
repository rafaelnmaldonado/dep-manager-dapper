namespace dep_manager_maven.Models
{
    public class EmployeeInputModel
    {
        public int Id { get; private set; }
        public string? Name { get; set; }
        public string? Picture { get; set; }
        public string? Rg { get; set; }
        public int? IdDepartment { get; set; }
        public bool IsDeleted { get; internal set; }
    }
}
