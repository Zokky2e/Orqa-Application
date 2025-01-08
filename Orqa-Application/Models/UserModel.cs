using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orqa_Application.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required, MaxLength(40)]
        public string Username { get; set; }
        [Required, MaxLength(40)]
        public string Firstname { get; set; }
        [Required, MaxLength(40)]
        public string Lastname { get; set; }
        [Required, MaxLength(100)]
        public string Password { get; set; }
        [NotMapped]
        public string Fullname => $"{Firstname} {Lastname}";
        public int RoleId { get; set; }
        public RoleModel Role { get; set; }
        public UserWorkPositionModel? UserWorkPosition { get; set; }

        public UserModel() 
        {
            Username = string.Empty;
            Firstname = string.Empty;
            Lastname = string.Empty;
            Password = string.Empty;
            Role = new RoleModel();
        }
    }
}
