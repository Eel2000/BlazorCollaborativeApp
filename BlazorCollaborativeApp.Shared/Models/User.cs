using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCollaborativeApp.Shared.Models
{
#nullable disable
    public class User
    {
        public User()
        {
            Projects = new HashSet<Project>();
            Contents = new HashSet<Content>();
            Collaborations = new HashSet<Collaboration>();
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();//set default Id generation
        public string Username { get; set; }

        public virtual ICollection<Content> Contents { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Collaboration> Collaborations { get; set; }
    }
}
