using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizzaData.Models
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public int Status { get; set; }
        public string ProductCode { get; set; }
        public decimal Price { get; set; }
        public byte IsExternal { get; set; }
        public string Description { get; set; }
    }
}
