using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyber.ViewModels {

  public static class ViewModelBridge {
    public static Client.Configuration TheConfigData { get; set; }
    public static string Wallet { get; set; }

    public static class AssetsFolders {
      public static string ContractImages { get; set; }
      public static string CollectionImages { get; set; }

      public static string NFTImages { get; set; }
      public static string NFTVideos { get; set; }
      public static string NFTModels { get; set; }
    }

    public static DataLayer.AccountContext DBAccount;
    public static DataLayer.NFTContext DBNFT;

    public static DataLayer.Models.Account CurrentAccount;

    public static List<DataLayer.Models.NFTContract> TestData_Contracts;
    public static List<DataLayer.Models.NFTCollection> TestData_Collections;
    public static List<DataLayer.Models.NFTItem> TestData_Items;


    public static void Initialize() {
      //
    }

    public static void PrepareDatabases() {
      var dbPath = System.IO.Path.Combine( TheConfigData.DataPath, Constants.ConfigData.Folder_Database );
      if (DBAccount == null) {
        DBAccount = new DataLayer.AccountContext( dbPath );
        DBAccount.CheckDatabase();
      }
      if (DBNFT == null) {
        DBNFT = new DataLayer.NFTContext( dbPath );
        DBNFT.CheckDatabase();
      }
    }

    public static void PrepareFolders() {
      AssetsFolders.ContractImages = Path.Combine( ViewModelBridge.TheConfigData.DataPath, Constants.ConfigData.Folder_ContractsImages );
      AssetsFolders.CollectionImages = Path.Combine( ViewModelBridge.TheConfigData.DataPath, Constants.ConfigData.Folder_CollectionsImages );

      AssetsFolders.NFTImages = Path.Combine( ViewModelBridge.TheConfigData.DataPath, Constants.ConfigData.Folder_NFTItemsImages );
      AssetsFolders.NFTVideos = Path.Combine( ViewModelBridge.TheConfigData.DataPath, Constants.ConfigData.Folder_NFTItemsVideos );
      AssetsFolders.NFTModels = Path.Combine( ViewModelBridge.TheConfigData.DataPath, Constants.ConfigData.Folder_NFTItemsModels );

      Directory.CreateDirectory( AssetsFolders.ContractImages );
      Directory.CreateDirectory( AssetsFolders.CollectionImages );
      Directory.CreateDirectory( AssetsFolders.NFTImages );
      Directory.CreateDirectory( AssetsFolders.NFTVideos );
      Directory.CreateDirectory( AssetsFolders.NFTModels );
    }

    public static async Task SwitchAccount(DataLayer.Models.Account account) {
      CurrentAccount = account;
      TheConfigData.LastWallet = CurrentAccount.Address;
      await Client.AppHelper.SaveConfiguration();
    }

  }

}
