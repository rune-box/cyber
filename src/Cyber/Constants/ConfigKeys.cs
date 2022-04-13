using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyber.Constants {
  public class ConfigData {
    //public const string FileName_Config = "config.json";

    public const string Key_DataFolder = "data_folder";
    public const string Key_LastWallet = "last_wallet";

    public const string Folder_Database = "db";

    public const string Folder_Contracts = "contracts";
    public static string Folder_ContractsImages = Path.Combine( "contracts", "images" );

    public const string Folder_Collections = "collections";
    public static string Folder_CollectionsImages = Path.Combine( "collections", "images" );
    
    public const string Folder_NFTItems = "nft-items";
    public static string Folder_NFTItemsImages = Path.Combine( "nft-items", "images" );
    public static string Folder_NFTItemsVideos = Path.Combine( "nft-items", "videos" );
    public static string Folder_NFTItemsModels = Path.Combine( "nft-items", "models" );

    public const string Seperator_FileNameParts = "____";
  }
}
