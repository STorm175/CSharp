using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public interface IApplicationContext
{
    DbSet<Product> Products { get; set; }

    Task<int> SaveChanges();
}