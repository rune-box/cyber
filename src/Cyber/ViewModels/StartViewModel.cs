using Catel.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyber.ViewModels {

  public class StartViewModel : Cyber.ViewModels.ViewModelBase {
    public StartViewModel() {
      CmdChooseDataFolder = new TaskCommand( OnCmdChooseDataFolderExecuteAsync );
    }

    public override string Title { get { return "Start"; } }

    // TODO: Register properties using 'vmprop'
    // TODO: Register properties that represent models using 'vmpropmodel'
    // TODO: Register properties that map to models using 'vmpropviewmodeltomodel'
    // TODO: Register commands using 'vmcommand', 'vmcommandwithcanexecute', 'vmtaskcommand' or 'vmtaskcommandwithcanexecute'

    public Action AfterChooseDataFolder { get; set; }

    public TaskCommand CmdChooseDataFolder { get; private set; }

    private async Task OnCmdChooseDataFolderExecuteAsync() {
      Catel.Services.DetermineDirectoryContext ddc = new Catel.Services.DetermineDirectoryContext();
      ddc.InitialDirectory = Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData );
      ddc.ShowNewFolderButton = true;

      Catel.Services.SelectDirectoryService sds = new Catel.Services.SelectDirectoryService();
      var r = await sds.DetermineDirectoryAsync( ddc );
      if (r.Result) {
        var configData = ViewModelBridge.TheConfigData;
        if (configData == null) {
          configData = new Client.Configuration();
        }
        configData.DataPath = r.DirectoryName;
        
        ViewModelBridge.TheConfigData = configData;
        Client.AppHelper.SaveConfiguration();
        // navigate
        //var dependencyResolver = this.DependencyResolver;
        //var navigationService = dependencyResolver.Resolve( typeof( Catel.Services.INavigationRootService ) ); //dependencyResolver.Resolve<Catel.Services.INavigationService>();
        //navigationService.Navigate<MainViewModel>();
        if (AfterChooseDataFolder != null) {
          AfterChooseDataFolder();
        }
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
