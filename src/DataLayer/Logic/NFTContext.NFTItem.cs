using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer {
  public partial class NFTContext {
    public async Task<int> Upsert(List<NFTItem> list) {
      int result = 0;
      if (list == null || list.Count == 0)
        return result;

      using (var transaction = this.Database.BeginTransaction()) {
        // update Updated tag
        foreach (var item in list) {
          item.UpdatedAt = DateTime.UtcNow;
          await Upsert( item, false );
        }
        transaction.Commit();
        transaction.Dispose();
      }
      return result;
    }

    public async Task Upsert(NFTItem item, bool saveChanges) {
      if (item == null)
        return;
      var tmp = this.Items.Where( x => x.HashId == item.HashId ).FirstOrDefaultAsync();
      if (tmp == null)
        await this.Items.AddAsync( item );
      else
        this.Items.Update( item );

      if (saveChanges)
        await this.SaveChangesAsync();
    }

    public async Task<bool> UpdateImageDownloaded_Item( string hashId) {
      NFTItem item = await this.Items
        .Where( x => x.HashId == hashId )
        .FirstOrDefaultAsync();
      if (item == null)
        return false;

      item.ImagesDownloaded = DateTime.UtcNow;
      this.Items.Update( item );
      int count = await this.SaveChangesAsync();
      return count > 0;
    }

    public async Task<List<NFTItem>> GetNFTItemsToDownload() {
      List<NFTItem> result = await this.Items
        .Where( x => x.ImagesDownloaded == DateTime.MinValue )
        .ToListAsync();
      return result;
    }

  }

}
