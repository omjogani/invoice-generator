using Microsoft.EntityFrameworkCore;

namespace InvoiceGenerator.Models
{
    public class ProductDBContext:DbContext
    {
        public ProductDBContext(DbContextOptions<ProductDBContext> options) : base(options)
        {

        }

        public DbSet<Products> products { get; set; }

        
    }
}
