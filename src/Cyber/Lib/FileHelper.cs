using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyber.Lib {
  public class FileHelper {

    public static string IdToFileName(string id) {
      if (string.IsNullOrEmpty( id ))
        return "unknown";
      return id.Replace( ":", Constants.ConfigData.Seperator_FileNameParts );
    }

    public static string GetAssetsFolder(string mediaType) {
      if (string.IsNullOrEmpty( mediaType ))
        return null;
      string imagePrefix = "image/";
      if (mediaType.EndsWith( "svg+xml" ) || mediaType.StartsWith( imagePrefix ))
        return ViewModels.ViewModelBridge.AssetsFolders.NFTImages;
      return ViewModels.ViewModelBridge.AssetsFolders.NFTImages;
    }

    public static string GetAssetsFolder(Constants.NFTContentType contentType) {
      switch (contentType) {
        case Constants.NFTContentType.Image:
          return ViewModels.ViewModelBridge.AssetsFolders.NFTImages;
        case Constants.NFTContentType.Video:
          return ViewModels.ViewModelBridge.AssetsFolders.NFTVideos;
        case Constants.NFTContentType.Model:
          return ViewModels.ViewModelBridge.AssetsFolders.NFTModels;
      }
      return ViewModels.ViewModelBridge.AssetsFolders.NFTImages;
    }

    public static string GetAssetsFolder(Constants.AssetsType assetType, Constants.NFTContentType contentType) {
      switch (assetType) {
        case Constants.AssetsType.Contract:
          return ViewModels.ViewModelBridge.AssetsFolders.ContractImages;
        case Constants.AssetsType.Collection:
          return ViewModels.ViewModelBridge.AssetsFolders.CollectionImages;
      }

      switch (contentType) {
        case Constants.NFTContentType.Image:
          return ViewModels.ViewModelBridge.AssetsFolders.NFTImages;
        case Constants.NFTContentType.Video:
          return ViewModels.ViewModelBridge.AssetsFolders.NFTVideos;
        case Constants.NFTContentType.Model:
          return ViewModels.ViewModelBridge.AssetsFolders.NFTModels;
      }
      return ViewModels.ViewModelBridge.AssetsFolders.NFTImages;
    }

  }

}
