namespace Cyber.Views {

	public partial class StartPage : ContentPage {
		ViewModels.StartViewModel vm;
		public StartPage() {
			InitializeComponent();

			vm = new ViewModels.StartViewModel();
			vm.AfterChooseDataFolder = afterChooseDataFolder;
			this.BindingContext = vm;
		}

		async void afterChooseDataFolder() {
			//Dictionary<string, string> param = new Dictionary<string, string>();
			//param.Add( Navigation.Key_NavigateFromView, "Lore.Views.AccountView" );
			//this.Frame.Navigate( typeof( MainView ), param );
			//// refresh hamburger menu
			////Tools.UIUtility.RefreshHamburgerMenu();
			//App.TheContainerVM.LoggedIn = true;

		 await App.NavigationWorker.Navigation.PushAsync( new Views.MainPage() );
		}

	}

}