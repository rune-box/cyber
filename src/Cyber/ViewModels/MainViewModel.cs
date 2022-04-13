using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyber.ViewModels {

  public class MainViewModel : Cyber.ViewModels.ViewModelBase {
    string title = "Hello";
    public MainViewModel() {
      CmdTestOpenSea = new TaskCommand( OnCmdTestOpenSeaExecuteAsync ); // test
      CmdLoadFromOpenSea = new TaskCommand( OnCmdLoadFromOpenSeaExecuteAsync );

      if (ViewModelBridge.TheConfigData != null && !string.IsNullOrEmpty( ViewModelBridge.TheConfigData.LastWallet )) {
        tryGetCurrentAccount();
        if(ViewModelBridge.CurrentAccount == null) {
          ViewModelBridge.TheConfigData.LastWallet = "";
          Client.AppHelper.SaveConfiguration();
        }
        else {
          Address = ViewModelBridge.CurrentAccount.Address;
          title = Address;//title = "Hello, " + ViewModelBridge.CurrentAccount.Name;
        }
      }
    }

    async void tryGetCurrentAccount() {
      if (ViewModelBridge.CurrentAccount == null || ViewModelBridge.CurrentAccount.Address != ViewModelBridge.TheConfigData.LastWallet) {
        ViewModelBridge.CurrentAccount = await DBAccount.SelectAccount( ViewModelBridge.TheConfigData.LastWallet );
      }
    }

    public override string Title { get { return title; } }

    // TODO: Register properties using 'vmprop'
    // TODO: Register properties that represent models using 'vmpropmodel'
    // TODO: Register properties that map to models using 'vmpropviewmodeltomodel'
    // TODO: Register commands using 'vmcommand', 'vmcommandwithcanexecute', 'vmtaskcommand' or 'vmtaskcommandwithcanexecute'

    public DataLayer.Models.Account CurrentAccount {
      get {
        return ViewModelBridge.CurrentAccount;
      }
    }

    public string Address {
      get { return GetValue<string>( AddressProperty ); }
      set { SetValue( AddressProperty, value ); }
    }

    public static readonly PropertyData AddressProperty = RegisterProperty( nameof( Address ), typeof( string ), "n/a" );

    public Action NavigeteToDownload { get; set; }

    public TaskCommand CmdLoadFromOpenSea { get; private set; }

    private async Task OnCmdLoadFromOpenSeaExecuteAsync() {
      var dependencyResolver = this.DependencyResolver;
      var pleaseWaitService = dependencyResolver.Resolve<Catel.Services.IPleaseWaitService>();
      pleaseWaitService.Push();
      try {
        NFT.OpenSea worker = new NFT.OpenSea( ViewModelBridge.TheConfigData.APIKey_OpenSea );
        // test data

        pleaseWaitService.Pop();

        // navigate
      }
      catch (Exception ex) {
        pleaseWaitService.Pop();
      }
    }

    private bool OnCmdLoadFromOpenSeaCanExecute() {
      return false;
    }

    public TaskCommand CmdTestOpenSea { get; private set; }

    private async Task OnCmdTestOpenSeaExecuteAsync() {
      var dependencyResolver = this.DependencyResolver;
      var pleaseWaitService = dependencyResolver.Resolve<Catel.Services.IPleaseWaitService>();
      pleaseWaitService.Push();
      var openFileService = dependencyResolver.Resolve<Catel.Services.IOpenFileService>();
      try {
        var ctx = new Catel.Services.DetermineOpenFileContext();
        var result = await openFileService.DetermineFileAsync(ctx);
        if (result.Result) {
          string json = System.IO.File.ReadAllText( result.FileName );
          NFT.OpenSea worker = new NFT.OpenSea( ViewModelBridge.TheConfigData.APIKey_OpenSea );
          worker.Process( json );

          ViewModelBridge.TestData_Contracts = worker.NFTContracts;
          ViewModelBridge.TestData_Collections = worker.NFTCollections;
          ViewModelBridge.TestData_Items = worker.NFTItems;

          int countContracts = await DBNFT.Upsert( worker.NFTContracts );
          int countCollections = await DBNFT.Upsert( worker.NFTCollections );
          int countNFTs = await DBNFT.Upsert( worker.NFTItems );

          pleaseWaitService.Pop();

          // navigate
          if (NavigeteToDownload != null)
            NavigeteToDownload();
          //if (countContracts > 0 || countCollections > 0 || countNFTs > 0) {
          //  if (NavigeteToDownload != null)
          //    NavigeteToDownload();
          //}
        }
      }
      catch (Exception ex) {
        pleaseWaitService.Pop();
      }
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
