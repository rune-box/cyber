using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models {
  [Table( "Contracts" )]
  public class NFTContract {
    public string Address { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public string ExternalURL { get; set; }
    public string ImageURL { get; set; }

    public string SchemaName { get; set; }
    public string Symbol { get; set; }
    public string NFTVersion { get; set; }

    public string PayoutAddress { get; set; }

    public string MediaType { get; set; }
    [Required]
    [Key]
    public string HashId { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime ImagesDownloaded { get; set; }

    public NFTContract() {
      UpdatedAt = DateTime.MinValue;
      ImagesDownloaded = DateTime.MinValue;
    }
  }

}
