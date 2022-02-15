namespace BlazorCollaborativeApp.Shared.Models
{
#nullable disable
    public class Sheet
    {
        public Sheet()
        {
            Contents = new HashSet<Content>();
        }

        public string Id { get; set; }= Guid.NewGuid().ToString();
        public string Title { get; set; }
        public string ProjectId { get; set; }
        public string SessionId { get; set; }
        public DateTime EditDate { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<Content> Contents { get; set; }
    }
}