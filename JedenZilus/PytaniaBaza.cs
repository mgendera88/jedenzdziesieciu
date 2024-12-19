using JedenZilus;
using Microsoft.EntityFrameworkCore;

class PytaniaBaza : DbContext
{
    public PytaniaBaza(DbContextOptions<PytaniaBaza> options) : base(options)
    {

    }
    public DbSet<pytanie> Pytania =>Set<pytanie>();
}