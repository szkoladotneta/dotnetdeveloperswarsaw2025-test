using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Persistance;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
}