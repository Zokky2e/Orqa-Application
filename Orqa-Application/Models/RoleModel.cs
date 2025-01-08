using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orqa_Application.Models
{
    public class RoleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<UserModel> Users { get; set; } = new List<UserModel>();

        public RoleModel() 
        {
            Name = string.Empty;
            Description = string.Empty;
        }

    }
}
