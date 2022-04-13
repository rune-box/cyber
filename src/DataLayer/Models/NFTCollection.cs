using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models {
  /// <summary>
  /// Specified to some NFT market.
  /// </summary>
  [Table( "Collections" )]
  public class NFTCollection {
    public string Name { get; set; }
    public string Description { get; set; }
    /// <summary>
    /// {Market:Slug}
    /// </summary>
    public string CollectionID { get; set; }

    public string ExternalURL { get; set; }
    public string WikiURL { get; set; }
    public string DiscordURL { get; set; }
    public string TelegramURL { get; set; }
    public string MediumUserName { get; set; }
    public string TwitterUserName { get; set; }
    public string InstagramUserName { get; set; }

    public string ImageURL { get; set; }
    public string LargeImageURL { get; set; }
    public string BannerImageURL { get; set; }
    public string FeaturedImageURL { get; set; }

    public string PayoutAddress { get; set; }
    public bool IsNSFW { get; set; }

    /// <summary>
    /// NFT Market
    /// </summary>
    public string Market { get; set; }
    public string Slug { get; set; }

    public string MediaType { get; set; }
    [Required]
    [Key]
    public string HashId { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime ImagesDownloaded { get; set; }

    public NFTCollection() {
      UpdatedAt = DateTime.MinValue;
      ImagesDownloaded = DateTime.MinValue;
    }
  }
}
