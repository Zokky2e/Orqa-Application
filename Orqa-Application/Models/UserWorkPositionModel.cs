using Orqa_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orqa_Application.Models
{
    public class UserWorkPositionModel
    {
        public int Id { get; set; }
        public WorkPositionModel WorkPosition { get; set; }
        public UserModel User { get; set; }
        public string ProductName { get; set; }
        public DateTime DateCreated { get; set; }

        public UserWorkPositionModel() {
            WorkPosition = new WorkPositionModel();
            User = new UserModel();
            ProductName = string.Empty;
            DateCreated = DateTime.Now;
        }
    }
}
