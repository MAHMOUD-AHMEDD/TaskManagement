//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace TaskManagement.Domain.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Project> OwnedProjects { get; set; } = new List<Project>();


        // Projects this user is a member of (via junction table)
        public ICollection<ProjectMember> ProjectMemberships { get; set; } = new List<ProjectMember>();



        // Tasks this user is assigned to (via junction table)
        public ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();



    }
}
