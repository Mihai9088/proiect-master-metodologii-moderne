using Microsoft.EntityFrameworkCore;

namespace SpendApp.Models
{
    public class SpendAppDbContext : DbContext
    {
        public DbSet<Expense> Expenses {  get; set; }

        public SpendAppDbContext(DbContextOptions<SpendAppDbContext>options) :base(options)
        {
            
        }
    }
}
