﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orqa_Application.Models
{
    public class WorkPositionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public WorkPositionModel() 
        {
            Name = string.Empty;
            Description = string.Empty;
        }
    }
}
