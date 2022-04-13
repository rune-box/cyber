using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models {
  [Table( "Items" )]
  public class NFTItem {
    /// <summary>
    /// {contract:TokenID}
    /// </summary>
    public string ItemID { get; set; }
    /// <summary>
    /// "opensea:id"
    /// </summary>
    public long MarketID { get; set; }
    public string TokenID { get; set; }
    
    public string Name { get; set; }
    public string CollectionId { get; set; }
    public string Contract { get; set; }

    public string Permalink { get; set; }
    public string ImageURL { get; set; }
    public string ImagePreviewURL { get; set; }
    public string ImageThumbnailURL { get; set; }
    public string ImageOriginalURL { get; set; }
    public string AnimationURL { get; set; }
    public string AnimationOriginalURL { get; set; }

    public string TokenMetadata { get; set; }
    /// <summary>
    /// json array string
    /// </summary>
    public string Traits { get; set; }

    /// <summary>
    /// creator address
    /// </summary>
    public string Creator { get; set; }
    /// <summary>
    /// owner address
    /// </summary>
    public string Owner { get; set; }

    public string MediaType { get; set; }
    [Required]
    [Key]
    public string HashId { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime ImagesDownloaded { get; set; }

    ///// <summary>
    ///// Different market means different collections.
    ///// </summary>
    //public string Collections{ get; set; }

    public NFTItem() {
      UpdatedAt = DateTime.MinValue;
      ImagesDownloaded = DateTime.MinValue;
    }

    public void SetItemID(string contract, string tokenId) {
      ItemID = $"{contract}:{tokenId}";
    }

  }
}
