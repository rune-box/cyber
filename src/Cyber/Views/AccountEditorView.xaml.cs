namespace Cyber.Views {
	public partial class AccountEditorView : ContentPage {
		ViewModels.AccountEditorViewModel vm;

		public AccountEditorView(DataLayer.Models.Account account) {
			InitializeComponent();

			vm = new ViewModels.AccountEditorViewModel( account );
			vm.AfterAdded = afterAdded;
			this.BindingContext = vm;
		}

		async Task<bool> afterAdded(bool success, string message) {
			if (success) {
			 await App.NavigationWorker.PushAsync( new AccountsManagerView() );
			}
			else {
			}
			//await App.NavigationWorker.Navigation.PushAsync( new Views.MainPage() );
			return success;
		}

	}

}