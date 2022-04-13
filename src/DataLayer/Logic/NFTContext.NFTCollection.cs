using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer {
  public partial class NFTContext {
    public async Task<int> Upsert(List<NFTCollection> list) {
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

    public async Task Upsert(NFTCollection item, bool saveChanges) {
      if (item == null)
        return;
      var tmp = this.Collections.Where( x => x.HashId == item.HashId ).FirstOrDefaultAsync();
      if (tmp == null)
        await this.Collections.AddAsync( item );
      else
        this.Collections.Update( item );

      if (saveChanges)
        await this.SaveChangesAsync();
    }

    public async Task<bool> UpdateImageDownloaded_Collection(string hashId) {
      NFTCollection item = await this.Collections
        .Where( x => x.HashId == hashId )
        .FirstOrDefaultAsync();
      if (item == null)
        return false;

      item.ImagesDownloaded = DateTime.UtcNow;
      this.Collections.Update( item );
      int count = await this.SaveChangesAsync();
      return count > 0;
    }

    public async Task<List<NFTCollection>> GetCollectionsToDownload() {
      List<NFTCollection> result = await this.Collections
        .Where( x => x.ImagesDownloaded == DateTime.MinValue )
        .ToListAsync();
      return result;
    }

  }

}
