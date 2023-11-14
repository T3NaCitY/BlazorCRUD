using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCRUD.Shared
{
    public class Product
    {
        public int Id { get; set; }
        public String Title { get; set; } = string.Empty;
        public String Description { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
