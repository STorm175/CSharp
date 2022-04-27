using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Product> Products { get; set; }
    public async Task<int> SaveChanges()
    {
        return await base.SaveChangesAsync();
    }
}