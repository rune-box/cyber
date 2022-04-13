using Catel.Data;
using Catel.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyber.ViewModels {

  public class DownloadViewModel : Cyber.ViewModels.ViewModelBase {
    HttpClient client;
    UI.Models.DownloadItem currentTask;
    //string assetsFolder_ContractImages, assetsFolder_CollectionImages,
    //  assetsFolder_NFTImages, assetsFolder_NFTVideos, assetsFolder_NFTModels;

    public DownloadViewModel() {
      CmdStartDownloadTask = new TaskCommand( OnCmdStartDownloadTaskExecuteAsync, OnCmdStartDownloadTaskCanExecute );
      client = new();

      LoadData();
    }

    public override string Title { get { return "Download"; } }

    // TODO: Register properties using 'vmprop'
    // TODO: Register properties that represent models using 'vmpropmodel'
    // TODO: Register properties that map to models using 'vmpropviewmodeltomodel'
    // TODO: Register commands using 'vmcommand', 'vmcommandwithcanexecute', 'vmtaskcommand' or 'vmtaskcommandwithcanexecute'


    public ObservableCollection<UI.Models.DownloadGroup> Items {
      get { return GetValue<ObservableCollection<UI.Models.DownloadGroup>>( ItemsProperty ); }
      set { SetValue( ItemsProperty, value ); }
    }

    public static readonly PropertyData ItemsProperty = RegisterProperty( nameof( Items ), typeof( ObservableCollection<UI.Models.DownloadGroup> ), null );


    public bool IsStarting {
      get { return GetValue<bool>( IsStartingProperty ); }
      set {
        SetValue( IsStartingProperty, value );
        CmdStartDownloadTask.RaiseCanExecuteChanged();
      }
    }

    public static readonly PropertyData IsStartingProperty = RegisterProperty( nameof( IsStarting ), typeof( bool ), false );

    public TaskCommand CmdStartDownloadTask { get; private set; }

    private bool OnCmdStartDownloadTaskCanExecute() {
      if(IsStarting || Items == null || Items.Count == 0)
        return false;
      bool hasTask = false;
      for(int i = 0; i< Items.Count; i++) {
        var group = Items[i];
        int count = group.Where( x => x.Percentage < 100 ).Count();
        if (count > 0) {
          hasTask = true;
          break;
        }
      }
      return hasTask;
    }

    private async Task OnCmdStartDownloadTaskExecuteAsync() {
      await StartDownloadTask();
    }

    public async Task StartDownloadTask() {
      IsStarting = true;
      try {
        UI.Models.DownloadGroup downloadGroup = null;
        int skipCount = 4; // TODO: test
        for (int i = 0; i < Items.Count; i++) {
          downloadGroup = Items[i];
          currentTask = downloadGroup.Where( x => x.Percentage == 0 ).FirstOrDefault();
          if (currentTask != null) {
            break;
          }
        } // for
        if(currentTask == null) {
          IsStarting = false;
          return;
        }

        if (string.IsNullOrEmpty( currentTask.ImageUri )) {
          downloadGroup.Remove( currentTask );
          await StartDownloadTask();
        }
        else if (currentTask.ImageUri.StartsWith( "ipfs" )) { // skip temporarily
          downloadGroup.Remove( currentTask );
          await StartDownloadTask();
        }
        //HttpRequestOptions requestOptions = new HttpRequestOptions();
        var res = await client.GetAsync( currentTask.ImageUri );

        await saveFileAsync( res );
        // TODO: test
        //currentTask.Percentage = 100;
        downloadGroup.Remove( currentTask );
        // DB
        //bool updated = await updateDB();
        //if (updated) {
        //  currentTask.Percentage = 100;
        //  downloadGroup.Remove( currentTask );
        //}
        await Task.Delay( 1000 );
        await StartDownloadTask();
      }
      catch (Exception ex) {
        IsStarting = false;
      }
    }

    public async void LoadData() {
      // test
      var contracts = ViewModelBridge.TestData_Contracts;
      var collections = ViewModelBridge.TestData_Collections;
      var items = ViewModelBridge.TestData_Items;
      if (items == null)
        return;
      // load from database
      //var contracts = await DBNFT.GetContractsToDownload();
      //var collections = await DBNFT.GetCollectionsToDownload();
      //var items = await DBNFT.GetNFTItemsToDownload();

      var dContracts = new ObservableCollection<UI.Models.DownloadItem>();
      contracts.ForEach( x => {
        UI.Models.DownloadItem di = new UI.Models.DownloadItem() {
          Id = x.HashId,
          Name = x.Name,
          FileName = x.HashId,
          ImageUri = x.ImageURL,
          AssetType = Constants.AssetsType.Contract
        };
        dContracts.Add( di );
      } );

      var dCollections = new ObservableCollection<UI.Models.DownloadItem>();
      collections.ForEach( x => {
        UI.Models.DownloadItem di = new UI.Models.DownloadItem() {
          Id = x.HashId,
          Name = x.Name,
          FileName = x.HashId,
          ImageUri = x.ImageURL,
          AssetType = Constants.AssetsType.Collection
        };
        dCollections.Add( di );
      } );

      var dItems = new ObservableCollection<UI.Models.DownloadItem>();
      items.ForEach( x => {
        UI.Models.DownloadItem di = new UI.Models.DownloadItem() {
          Id = x.HashId,
          Name = x.Name,
          FileName = x.HashId,
          ImageUri = String.IsNullOrEmpty( x.ImageOriginalURL ) ? x.ImageURL : x.ImageOriginalURL,
          AssetType = Constants.AssetsType.NFTItem
        };
        dItems.Add( di );
      } );
      
      this.Items = new ObservableCollection<UI.Models.DownloadGroup>();
      this.Items.Add( new UI.Models.DownloadGroup( "Contracts", "layer_1.png", dContracts ) );
      this.Items.Add( new UI.Models.DownloadGroup( "Collections", "layer_2.png", dCollections ) );
      this.Items.Add( new UI.Models.DownloadGroup( "Items", "picture.png", dItems ) );

      CmdStartDownloadTask.RaiseCanExecuteChanged();
    }

    string getFileExtension(HttpContent hc) {
      if (hc == null)
        return null;
      var contentType = hc.Headers.ContentType != null ? ( hc.Headers.ContentType.MediaType ?? "" ) : null;
      return Lib.StringUtility.GetExtensionFromMediaType( contentType );
    }

    string getFilePath(string filename, HttpContent hc) {
      var folder = getAssetsFolder( currentTask.AssetType, currentTask.ContentType );
      var fileExtension = getFileExtension( hc );
      if (fileExtension == null)
        fileExtension = ".unknown";
      return folder + "/" + filename + fileExtension;
    }

    async Task saveFileAsync( HttpResponseMessage res ) {
      if (res == null || res.Content == null)
        return;

      var path = getFilePath( currentTask.FileName, res.Content );
      using (var fs = File.OpenWrite( path )) {
        await res.Content.CopyToAsync( fs );
      } // using
    }

    async Task<bool> updateDB() {
      bool result = false;
      switch (currentTask.AssetType) {
        case Constants.AssetsType.Contract:
          result = await DBNFT.UpdateImageDownloaded_Contract( currentTask.Id );
          break;
        case Constants.AssetsType.Collection:
          result = await DBNFT.UpdateImageDownloaded_Contract( currentTask.Id );
          break;
        case Constants.AssetsType.NFTItem:
          result = await DBNFT.UpdateImageDownloaded_Contract( currentTask.Id );
          break;
      }
      return result;
    }

    string getAssetsFolder(Constants.AssetsType assetType, Constants.NFTContentType contentType) {
      switch (assetType) {
        case Constants.AssetsType.Contract:
          return ViewModelBridge.AssetsFolders.ContractImages;
        case Constants.AssetsType.Collection:
          return ViewModelBridge.AssetsFolders.CollectionImages;
      }

      switch (contentType) {
        case Constants.NFTContentType.Image:
          return ViewModelBridge.AssetsFolders.NFTImages;
        case Constants.NFTContentType.Video:
          return ViewModelBridge.AssetsFolders.NFTVideos;
        case Constants.NFTContentType.Model:
          return ViewModelBridge.AssetsFolders.NFTModels;
      }
      return ViewModelBridge.AssetsFolders.NFTImages;
    }

    protected override async Task InitializeAsync() {
      await base.InitializeAsync();

      // TODO: Add initialization logic like subscribing to events
    }

    protected override async Task CloseAsync() {
      // TODO: Add uninitialization logic like unsubscribing from events

      await base.CloseAsync();
    }

  }

}
