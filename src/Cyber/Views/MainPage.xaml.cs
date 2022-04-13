namespace Cyber.Views {
  public partial class MainPage : TabbedPage {
    int count = 0;
    ViewModels.MainViewModel vm;

    public MainPage() {
      InitializeComponent();

      ViewModels.ViewModelBridge.PrepareDatabases();
      ViewModels.ViewModelBridge.PrepareFolders();

      vm = new ViewModels.MainViewModel();
      vm.NavigeteToDownload = navigateToDownload;
      this.BindingContext = vm;

      pageDownload.BindingContext = new ViewModels.DownloadViewModel();
    }

    async void navigateToDownload() {
      //await App.NavigationWorker.Navigation.PushAsync( new MainPage() );
      tabMain.SelectedItem = pageDownload;
      var vm = pageDownload.BindingContext as ViewModels.DownloadViewModel;
      if(vm != null) {
        vm.LoadData();
      }
    }

    private async void btnPlay_Clicked(object sender, EventArgs e) {
      await App.NavigationWorker.Navigation.PushAsync( new ImageView() );
    }

    //private void OnCounterClicked(object sender, EventArgs e) {
    //  count++;
    //  CounterLabel.Text = $"Current count: {count}";

    //  SemanticScreenReader.Announce( CounterLabel.Text );
    //}

  }

}