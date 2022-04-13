using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer {
  public partial class NFTContext {
    public async Task<int> Upsert(List<NFTContract> list) {
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

    public async Task Upsert(NFTContract item, bool saveChanges) {
      if (item == null)
        return;
      var tmp = this.Contracts.Where( x => x.HashId == item.HashId ).FirstOrDefaultAsync();
      if (tmp == null)
        await this.Contracts.AddAsync( item );
      else
        this.Contracts.Update( item );

      if (saveChanges)
        await this.SaveChangesAsync();
    }

    public async Task<bool> UpdateImageDownloaded_Contract(string hashId) {
      NFTContract item = await this.Contracts
        .Where( x => x.HashId == hashId )
        .FirstOrDefaultAsync();
      if(item == null) 
        return false;

      item.ImagesDownloaded = DateTime.UtcNow;
      this.Contracts.Update( item );
      int count = await this.SaveChangesAsync();
      return count > 0;
    }

    public async Task<List<NFTContract>> GetContractsToDownload() {
      List<NFTContract> result = await this.Contracts
        .Where( x => x.ImagesDownloaded == DateTime.MinValue )
        .ToListAsync();
      return result;
    }

  }

}
