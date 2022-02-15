namespace BlazorCollaborativeApp.Shared.Models
{
#nullable disable
    public class Project
    {
        public Project()
        {
            Sheets = new HashSet<Sheet>();
            Collaborations = new HashSet<Collaboration>();
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Description { get; set; }
        public string OwnerId { get; set; }

        public virtual ICollection<Sheet> Sheets { get; set; }
        public virtual ICollection<Collaboration> Collaborations { get; set; }

    }
}