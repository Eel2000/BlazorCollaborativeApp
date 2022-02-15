using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCollaborativeApp.Shared.Models
{
#nullable disable
    public class Collaboration
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string UserId { get; set; }

        public virtual User User { get; set; }
        public virtual Project Project { get; set; }
    }
}
