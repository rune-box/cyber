using Microsoft.EntityFrameworkCore;

namespace DataLayer {
  public partial class NFTContext : DbContext{
    public string DbPath { get; }
    string _connectionString;

    public DbSet<Models.NFTContract> Contracts { get; set; }
    public DbSet<Models.NFTCollection> Collections { get; set; }
    public DbSet<Models.NFTItem> Items { get; set; }

    public NFTContext(string dataPath) {
      if (string.IsNullOrEmpty( dataPath ))
        throw new Exception( "The path of Database is null/empty." );

      DbPath = System.IO.Path.Join( dataPath, Constants.DB.dbFile_NFT );
      _connectionString = "Data Source=" + DbPath;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
      optionsBuilder.UseSqlite( _connectionString );
    }

    //protected override void OnModelCreating(ModelBuilder modelBuilder) {
    //  modelBuilder.Entity<Models.NFTContract>()
    //      .has( c => new { c.State, c.LicensePlate } );
    //}

    public void CheckDatabase() {
      this.Database.EnsureCreated();
    }

  }

}
