using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class ApplicationContext : DbContext, IApplicationContext
{
    public DbSet<Product> Products { get; set; }
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    { }
    public async Task<int> SaveChanges()
    {
        return await base.SaveChangesAsync();
    }
}