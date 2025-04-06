using JADA.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace JADA.Data.Context;

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
}
