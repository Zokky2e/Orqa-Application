using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orqa_Application.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Fullname => $"{Firstname} {Lastname}";
        public RoleModel Role { get; set; }

        public UserModel() 
        {
            Username = string.Empty;
            Firstname = string.Empty;
            Lastname = string.Empty;
            Role = new RoleModel();
        }
    }
}
