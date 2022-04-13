using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer {
  public partial class NFTContext {

    public string DbPath { get; }

    public NFTContext(string dataPath) {
      var folder = Environment.SpecialFolder.LocalApplicationData;
      var path = dataPath ?? Environment.GetFolderPath( folder );
      DbPath = System.IO.Path.Join( path, Constants.DB.dbFile_NFT );
    }

    void indexNFTContract(ILiteCollection<Models.NFTContract> col) {
      if (col == null)
        return;
      col.EnsureIndex( x => x.Address, true );
      col.EnsureIndex( x => x.Name );
      //col.EnsureIndex( x => x.Description ); // The length of Description may be too long. Should less than 1023 bytes.
    }

    void indexNFTCollection(ILiteCollection<Models.NFTCollection> col) {
      if (col == null)
        return;
      col.EnsureIndex( x => x.CollectionID, true );
      col.EnsureIndex( x => x.Name );
      //col.EnsureIndex( x => x.Description ); // The length of Description may be too long. Should less than 1023 bytes.
      col.EnsureIndex( x => x.Market );
      col.EnsureIndex( x => x.Slug );
    }

    void indexNFTItem(ILiteCollection<Models.NFTItem> col) {
      if (col == null)
        return;
      col.EnsureIndex( x => x.ItemID, true );
      col.EnsureIndex( x => x.Name );
    }

  }

}
