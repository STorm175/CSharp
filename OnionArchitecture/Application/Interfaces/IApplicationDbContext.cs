using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public interface IApplicationDbContext
{
    DbSet<Product> Products { get; set; }
    Task<int> SaveChanges();
}