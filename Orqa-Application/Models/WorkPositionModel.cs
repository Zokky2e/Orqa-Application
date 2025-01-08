using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orqa_Application.Models
{
    [Table("work_positions")]
    public class WorkPositionModel
    {
        public int Id { get; set; }
        [Required, MaxLength(40)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public ICollection<UserWorkPositionModel> UserWorkPositions { get; set; } = new List<UserWorkPositionModel>();


        public WorkPositionModel() 
        {
            Name = string.Empty;
            Description = string.Empty;
        }
    }
}
