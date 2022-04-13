using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer {
  public class AccountContext {

    public string DbPath { get; }

    public AccountContext(string dataPath) {
      if (string.IsNullOrEmpty( dataPath ))
        throw new Exception( "The path of Database is null/empty." );

      DbPath = System.IO.Path.Join( dataPath, Constants.DB.dbFile_Account );
    }

    public Models.Account SelectAccount( string address ) {
      Models.Account result = null;
      using (var db = new LiteDatabase( DbPath )) {
        var q = db.GetCollection<Models.Account>( "accounts" );
        result = q.FindOne( x => x.Address == address );
      }
      return result;
    }

    public List<Models.Account> SelectAccounts() {
      List<Models.Account> result = new List<Models.Account>();
      using (var db = new LiteDatabase( DbPath )) {
        var q = db.GetCollection<Models.Account>( "accounts" );
        var tmp = q.FindAll();
        result = new List<Models.Account>( tmp );
      }
      return result;
    }

    void indexAccount(ILiteCollection<Models.Account> col) {
      if (col == null)
        return;
      col.EnsureIndex( x => x.Address, true );
      col.EnsureIndex( x => x.Name );
      //col.EnsureIndex( x => x.Remark ); // The length of Remark may be too long. Should less than 1023 bytes.
    }

    //public void AddEvmAccount( string address, string name, string remark) {
    //  using (var db = new LiteDatabase( DbPath )) {
    //    var col = db.GetCollection<Models.Account>( "accounts" );
    //    indexAccount( col );

    //    var exists = col.Exists( account => account.Address == address && account.IsEVM );
    //    if (!exists) {
    //      var account = Models.Account.CreateEvmAccount( address, name, remark );
    //      col.Insert( account );
    //    }
    //  }
    //}

    public bool UpsertAccount(Models.Account item) {
      bool result = false;
      using (var db = new LiteDatabase( DbPath )) {
        var col = db.GetCollection<Models.Account>( "accounts" );
        indexAccount( col );

        bool exists = col.Exists( x => x.Address == item.Address );
        if (exists) {
          result = col.Update( item );
        }
        else {
          result = col.Upsert( item );
        }
      }
      return result;
    }

    //public void AddAccount(string address, string evmAddress, string chain, string name, string remark) {
    //  using (var db = new LiteDatabase( DbPath )) {
    //    var col = db.GetCollection<Models.Account>( "accounts" );
    //    indexAccount( col );

    //    var exists = col.Exists( account => account.Address == address );
    //    if (!exists) {
    //      var account = Models.Account.CreateAccount( address, evmAddress, chain, name, remark );
    //      col.Insert( account );
    //    }
    //  }
    //}

    public int RemoveAccount(string address) {
      int deleted = 0;
      using (var db = new LiteDatabase( DbPath )) {
        var col = db.GetCollection<Models.Account>( "accounts" );
        deleted = col.DeleteMany( account => account.Address == address );
      }
      return deleted;
    }

  }

}
