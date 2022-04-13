using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer {
  public class AccountContext: DbContext {

    public string DbPath { get; }
    string _connectionString;

    public DbSet<Models.Account> Accounts { get; set; }

    public AccountContext(string dataPath) {
      if (string.IsNullOrEmpty( dataPath ))
        throw new Exception( "The path of Database is null/empty." );

      DbPath = System.IO.Path.Join( dataPath, Constants.DB.dbFile_Account );
      _connectionString = "Data Source=" + DbPath;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
      optionsBuilder.UseSqlite( _connectionString );
    }

    public void CheckDatabase() {
      this.Database.EnsureCreated();
    }

    public async Task<Models.Account> SelectAccount( string address ) {
      Models.Account result = await this.Accounts
        .Where( x => x.Address == address )
        .FirstOrDefaultAsync();
      return result;
    }

    public async Task<List<Models.Account>> SelectAccounts() {
      List<Models.Account> result = await this.Accounts
        .ToListAsync();
      return result;
    }


    public async Task<bool> UpsertAccount(Models.Account item) {
      Models.Account exists = await this.Accounts
        .Where( x => x.Address == item.Address )
        .FirstOrDefaultAsync();
      if (exists != null) {
        this.Accounts.Update( item );
      }
      else {
        this.Accounts.Add( item );
      }
      int count = await this.SaveChangesAsync();
      return count > 0;
    }

    public async Task<bool> RemoveAccount(string address) {
      Models.Account exists = await this.Accounts
        .Where( x => x.Address == address )
        .FirstOrDefaultAsync();
      if (exists != null) {
        this.Accounts.Remove( exists );
        await this.SaveChangesAsync();
        return true;
      }
      return false;
    }

  }

}
