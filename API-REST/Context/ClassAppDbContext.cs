using API_REST.Models;
using Microsoft.EntityFrameworkCore;

namespace API_REST.Context;

public class ClassAppDbContext : DbContext
{
    public ClassAppDbContext(DbContextOptions<ClassAppDbContext> options) : base(options)
    { 
   
    }

    public DbSet<Libros> Libros { get; set; }

}
