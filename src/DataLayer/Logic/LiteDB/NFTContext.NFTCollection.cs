using DataLayer.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer {
  public partial class NFTContext {
    public int Upsert(List<NFTCollection> list) {
      int result = 0;
      if (list == null || list.Count == 0)
        return result;
      using (var db = new LiteDatabase( DbPath )) {
        var col = db.GetCollection<NFTCollection>( "collections" );
        indexNFTCollection( col );

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
      } // using
      return result;
    }

    public bool UpdateImageDownloaded_Collection(string id) {
      bool result = false;
      using (var db = new LiteDatabase( DbPath )) {
        var col = db.GetCollection<NFTCollection>( "collections" );
        indexNFTCollection( col );

        var item = col.Find( x => x.CollectionID == id ).FirstOrDefault();
        if (item != null) {
          item.ImagesDownloaded = DateTime.UtcNow;
          result = col.Update( item );
        }
      }
      return result;
    }

    public List<NFTCollection> GetCollectionsToDownload() {
      List<NFTCollection> result;
      using (var db = new LiteDatabase( DbPath )) {
        var col = db.GetCollection<NFTCollection>( "collections" );
        indexNFTCollection( col );

        var dtMin = DateTime.MinValue;
        result = col.Find( x => x.ImagesDownloaded == dtMin )
          .ToList();
      }
      return result;
    }

  }

}
