using System;

namespace Meridian.UserManagement.Core.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }        
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsActive { get; set; } 
        public bool IsAddedToCatalogService { get; set; }

    }

    public class UserAndRoleInfo
    {
        public User User { get; set; }
        public int RoleId { get; set; }
    }
}