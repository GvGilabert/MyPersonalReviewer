using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyPersonalReviewer.Models
{
    public class Review
    {
        public Guid Id { get; set; }
        public string ReviewText { get; set; }
        [Range(1,5)]
        public int Points { get; set; }
    }
}
