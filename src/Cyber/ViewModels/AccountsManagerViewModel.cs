using Catel.Data;
using Catel.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyber.ViewModels {

  public class AccountsManagerViewModel : ViewModels.ViewModelBase {
    public AccountsManagerViewModel() {
      initialize();
    }

    public override string Title { get { return "Account Editor"; } }

    // TODO: Register properties using 'vmprop'
    // TODO: Register properties that represent models using 'vmpropmodel'
    // TODO: Register properties that map to models using 'vmpropviewmodeltomodel'
    // TODO: Register commands using 'vmcommand', 'vmcommandwithcanexecute', 'vmtaskcommand' or 'vmtaskcommandwithcanexecute'


    public ObservableCollection<DataLayer.Models.Account> Accounts {
      get { return GetValue<ObservableCollection<DataLayer.Models.Account>>( AccountsProperty ); }
      set { SetValue( AccountsProperty, value ); }
    }

    public static readonly PropertyData AccountsProperty = RegisterProperty( nameof( Accounts ), typeof( ObservableCollection<DataLayer.Models.Account> ), null );

    //public DataLayer.Models.Account SelectedAccount {
    //  get { return GetValue<DataLayer.Models.Account>( SelectedAccountProperty ); }
    //  set { SetValue( SelectedAccountProperty, value ); }
    //}

    //public static readonly PropertyData SelectedAccountProperty = RegisterProperty( nameof( SelectedAccount ), typeof( DataLayer.Models.Account ), null );

    async void initialize() {
      var data = await DBAccount.SelectAccounts();
      Accounts = new ObservableCollection<DataLayer.Models.Account>( data );
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
