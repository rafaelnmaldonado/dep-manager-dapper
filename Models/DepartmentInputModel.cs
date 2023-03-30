namespace dep_manager_maven.Models
{
    public class DepartmentInputModel
    {
        public int Id { get; private set; }
        public string? Name { get; set; }
        public string? Acronym { get; set; }
        public bool IsDeleted { get; internal set; }
    }
}
