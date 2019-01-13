using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPersonalReviewer.Models
{
    public class Menu
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string CreatorsId { get; set; }
        public Guid PlaceId { get; set; }
        public Guid Id { get; set; }
    }

    public class MenuItemIncompleteDataException:Exception{};
}
