using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskMonitorWebAPI.Entities
{
    //public class User : IdentityUser
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public Guid Id { get; set; }
    //    public string Name { get; set; }
    //    public string Surname { get; set; }
    //    public int age { get; set; }
    //    public string EmailAddress { get; set; }
    //    public DateOnly DateOfBirth { get; set; }
    //    public string? Address { get; set; }
    //    public List<Tasks> Tasks { get; set; }
    //}

    public class IdentityRoles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        [Column("Password")]
        public string PasswordHash { get; set; } = string.Empty;
        [EmailAddress]
        [Column("Email Address")]
        public string EmailAddress { get; set; } = string.Empty;
        public int age { get; set; }
        public string Roles { get; set; } = string.Empty;
        [Column("DOB")]
        public DateOnly DateOfBirth { get; set; }
        [Column("Residential Address")]
        public string? Address { get; set; }
        public List<Tasks> Tasks { get; set; }


    }

    public class User : IdentityRoles
    {
        public User()
        {
            Roles = "User";
        }

    }

    public class Supervisior : User
    {
        public Supervisior()
        {
            Roles = "Supervisor";
        }
    }
}
