using Catel.Data;
using Catel.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyber.ViewModels {

  public class AccountEditorViewModel : ViewModels.ViewModelBase {
    public AccountEditorViewModel(DataLayer.Models.Account account) {
      if (account == null) {
        var newAccount = new DataLayer.Models.Account() {
          Name = "my wallet",
          Remark = DateTime.Now.ToLongDateString()
        };
        DataContext = newAccount;
      }
      else
        DataContext = account;
      IsEvm = DataContext.IsEVM; // hack

      CmdSaveAccount = new TaskCommand( OnCmdSaveAccountExecuteAsync, OnCmdSaveAccountCanExecute );
    }

    public override string Title { get { return "Account Editor"; } }

    // TODO: Register properties using 'vmprop'
    // TODO: Register properties that represent models using 'vmpropmodel'
    // TODO: Register properties that map to models using 'vmpropviewmodeltomodel'
    // TODO: Register commands using 'vmcommand', 'vmcommandwithcanexecute', 'vmtaskcommand' or 'vmtaskcommandwithcanexecute'


    [Model]
    public DataLayer.Models.Account DataContext {
      get { return GetValue<DataLayer.Models.Account>( DataContextProperty ); }
      private set { SetValue( DataContextProperty, value ); }
    }

    public static readonly PropertyData DataContextProperty = RegisterProperty( nameof( DataContext ), typeof( DataLayer.Models.Account ) );


    [ViewModelToModel]
    public string Address {
      get { return GetValue<string>( AddressProperty ); }
      set {
        SetValue( AddressProperty, value );
        CmdSaveAccount.RaiseCanExecuteChanged();
        if(IsEvm.HasValue && IsEvm.Value && value != EvmAddress)
          EvmAddress = value;
      }
    }

    public static readonly PropertyData AddressProperty = RegisterProperty( nameof( Address ), typeof( string ) );

    [ViewModelToModel]
    public string EvmAddress {
      get { return GetValue<string>( EvmAddressProperty ); }
      set { SetValue( EvmAddressProperty, value ); }
    }

    public static readonly PropertyData EvmAddressProperty = RegisterProperty( nameof( EvmAddress ), typeof( string ) );

    [ViewModelToModel]
    public bool? IsEvm {
      get { return GetValue<bool?>( IsEvmProperty ); }
      set {
        SetValue( IsEvmProperty, value );
        if (value.HasValue && value.Value)
          EvmAddress = Address;
        else
          EvmAddress = null;
      }
    }

    public static readonly PropertyData IsEvmProperty = RegisterProperty( nameof( IsEvm ), typeof( bool? ), false );


    [ViewModelToModel]
    public string Name {
      get { return GetValue<string>( NameProperty ); }
      set { SetValue( NameProperty, value ); }
    }

    public static readonly PropertyData NameProperty = RegisterProperty( nameof( Name ), typeof( string ) );


    [ViewModelToModel]
    public string Remark {
      get { return GetValue<string>( RemarkProperty ); }
      set { SetValue( RemarkProperty, value ); }
    }

    public static readonly PropertyData RemarkProperty = RegisterProperty( nameof( Remark ), typeof( string ) );


    [ViewModelToModel]
    public string Chain {
      get { return GetValue<string>( ChainProperty ); }
      set { SetValue( ChainProperty, value ); }
    }

    public static readonly PropertyData ChainProperty = RegisterProperty( nameof( Chain ), typeof( string ) );


    public TaskCommand CmdSaveAccount { get; private set; }

    private bool OnCmdSaveAccountCanExecute() {
      return DataContext != null && !string.IsNullOrEmpty( DataContext.Address);
    }

    public Func<bool, string, Task<bool>> AfterAdded;

    private async Task OnCmdSaveAccountExecuteAsync() {
      var dependencyResolver = this.DependencyResolver;
      Catel.Services.MessageService msgService = dependencyResolver.Resolve( typeof( Catel.Services.MessageService ) ) as Catel.Services.MessageService;
      try {
        DataContext.IsEVM = IsEvm; // hack
        bool success = await DBAccount.UpsertAccount( DataContext );
        if (success) {
          ViewModelBridge.TheConfigData.LastWallet = DataContext.Address;
          await Client.AppHelper.SaveConfiguration();

          await msgService.ShowInformationAsync( "Saved!" );
          if (AfterAdded != null)
            await AfterAdded( true, "Saved" );
        }
        else {
          await msgService.ShowErrorAsync( "Unknown error..." );
          if (AfterAdded != null)
            await AfterAdded( false, "Unknown error..." );
        }
      }
      catch (Exception ex) {
        await msgService.ShowErrorAsync( ex.Message );
        if (AfterAdded != null)
          await AfterAdded( false, ex.Message );
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
