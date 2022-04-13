using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyber.Client {
  public static class AppHelper {
    //public static void LoadConfiguration() {
    //  //var folder = Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData );
    //  var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
    //  var path = System.IO.Path.Join( folder, Constants.ConfigData.FileName_Config );
    //  if (!System.IO.File.Exists( path )) {
    //    ViewModels.ViewModelBridge.TheConfigData = null;
    //    return;
    //  }
    //  var text = System.IO.File.ReadAllText( path );
    //  ViewModels.ViewModelBridge.TheConfigData = System.Text.Json.JsonSerializer.Deserialize< Client.Configuration>(text);
    //}

    //public static void SaveConfiguration() {
    //  //var folder = Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData );
    //  var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
    //  var path = System.IO.Path.Join( folder, Constants.ConfigData.FileName_Config );

    //  //var opts = new System.Text.Json.JsonSerializerOptions();
    //  var text = System.Text.Json.JsonSerializer.Serialize( ViewModels.ViewModelBridge.TheConfigData );
    //  System.IO.File.WriteAllText( path, text );
    //}

    public static async Task LoadConfiguration() {
      var dataFolder = await SecureStorage.GetAsync( Constants.ConfigData.Key_DataFolder );
      if (string.IsNullOrEmpty( dataFolder )) {
        ViewModels.ViewModelBridge.TheConfigData = null;
        return;
      }

      var config = new Configuration();
      config.DataPath = dataFolder;
      config.LastWallet = await SecureStorage.GetAsync( Constants.ConfigData.Key_LastWallet );
      ViewModels.ViewModelBridge.TheConfigData = config;
    }

    public static async Task SaveConfiguration() {
      if (ViewModels.ViewModelBridge.TheConfigData == null)
        return;
      await SecureStorage.SetAsync( Constants.ConfigData.Key_DataFolder, ViewModels.ViewModelBridge.TheConfigData.DataPath );
      await SecureStorage.SetAsync( Constants.ConfigData.Key_LastWallet, ViewModels.ViewModelBridge.TheConfigData.LastWallet );
    }

  }

}
