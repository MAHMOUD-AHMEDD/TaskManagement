//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace TaskManagement.Domain.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Project> OwnedProjects { get; set; }


        // Projects this user is a member of (via junction table)
        public ICollection<ProjectMember> PrjoectMemberships { get; set; }



        // Tasks this user is assigned to (via junction table)
        public ICollection<TaskAssignment> TaskAssignments { get; set; }
        public ICollection<Comment> Comments { get; set; }



    }
}
