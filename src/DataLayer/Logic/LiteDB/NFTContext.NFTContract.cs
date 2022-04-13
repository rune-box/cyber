using DataLayer.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer {
  public partial class NFTContext {
    public int Upsert(List<NFTContract> list) {
      int result = 0;
      if (list == null || list.Count == 0)
        return result;
      using (var db = new LiteDatabase( DbPath )) {
        var col = db.GetCollection<NFTContract>( "contracts" );
        indexNFTContract( col );

        try {
          db.BeginTrans();
          result = col.Upsert( list );

          // update Updated tag
          list.ForEach( x => x.UpdatedAt = DateTime.UtcNow );
          col.Update( list );
          db.Commit();
        }
        catch (Exception ex) {
          db.Rollback();
        }
      }
      return result;
    }

    public bool UpdateImageDownloaded_Contract(string id) {
      bool result = false;
      using (var db = new LiteDatabase( DbPath )) {
        var col = db.GetCollection<NFTContract>( "contracts" );
        indexNFTContract( col );

        var item = col.Find( x => x.Address == id ).FirstOrDefault();
        if (item != null) {
          
          item.ImagesDownloaded = DateTime.UtcNow;
          result = col.Update( item );
        }
      }
      return result;
    }

    public List<NFTContract> GetContractsToDownload() {
      List<NFTContract> result;
      using (var db = new LiteDatabase( DbPath )) {
        var col = db.GetCollection<NFTContract>( "contracts" );
        indexNFTContract( col );

        var dtMin = DateTime.MinValue;
        result = col.Find( x => x.ImagesDownloaded == dtMin )
          .ToList();
      }
      return result;
    }

  }

}
