using System.ComponentModel.DataAnnotations;

namespace InvoiceGenerator.Models
{
    public class Products
    {
        public int userId { get; set; }
        [Key]
        public int productId { get; set; }
        
        
        public string productName { get; set; }

        public int Qty { get; set; }

        public double price { get; set; }

        public double total { get; set; }
    }

}
