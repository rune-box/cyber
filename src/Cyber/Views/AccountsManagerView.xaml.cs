namespace Cyber.Views {
	public partial class AccountsManagerView : ContentPage {
		ViewModels.AccountsManagerViewModel vm;
		public AccountsManagerView() {
			InitializeComponent();
      this.NavigatedTo += AccountsManagerView_NavigatedTo;
			vm = new ViewModels.AccountsManagerViewModel();
			this.BindingContext = vm;
		}

    private void AccountsManagerView_NavigatedTo(object sender, NavigatedToEventArgs e) {
			this.cvAccounts.SelectedItem = null;
    }

    private async void btnAddAccount_Clicked(object sender, EventArgs e) {
			await App.NavigationWorker.PushAsync( new AccountEditorView( null ) );
		}

		private async void btnEdit_Clicked(object sender, EventArgs e) {
			var btn = sender as ImageButton;
			if (btn != null) {
				var item = btn.BindingContext as DataLayer.Models.Account;
				if (item != null) {
					await App.NavigationWorker.PushAsync( new AccountEditorView( item ) );
				}
			}
		}

		private async void cvAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (e.CurrentSelection.Count > 0) {
				DataLayer.Models.Account item = e.CurrentSelection[0] as DataLayer.Models.Account;
				if (item != null) {
					await ViewModels.ViewModelBridge.SwitchAccount( item );
					await App.NavigationWorker.PushAsync( new MainPage() );
				}
			}
		}
	}
}