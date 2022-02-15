using System.ComponentModel.DataAnnotations;

namespace BlazorCollaborativeApp.Shared.Models
{
#nullable disable
    public class Content
    {
        [Key]
        public int Id { get; set; }
        public string Data { get; set; }
        public int Line { get; set; }
        public string SheetId { get; set; }
        public bool IsDeleted { get; set; }
        public string UserId { get; set; }
        public DateTime EditionDate { get; set; } = DateTime.Now;

        public virtual Sheet Sheet { get; set; }
        public virtual User User { get; set;}
    }
}