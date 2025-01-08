using Orqa_Application.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orqa_Application.Models
{
    [Table("user_work_positions")]
    public class UserWorkPositionModel
    {
        public int Id { get; set; }
        [Column("work_positionId")]
        public int WorkPositionId { get; set; }
        public int UserId { get; set; }
        public WorkPositionModel WorkPosition { get; set; }
        public UserModel User { get; set; }
        [MaxLength(40)]
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
