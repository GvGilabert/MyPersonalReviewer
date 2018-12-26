using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPersonalReviewer.Models
{
    public class Places
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Enums.Categories Category { get; set; }
        public string CreatedByUserId { get; set; }

    }
}
