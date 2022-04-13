using Cyber.UI.Models;

namespace Cyber.Views {
  public partial class ContainerPage : FlyoutPage {
    //ViewModels.MainViewModel vm;

    public ContainerPage() {
      InitializeComponent();

      //this.NavigatedTo += ContainerPage_NavigatedTo; // not work
      flyoutMenu.ItemSelected += OnFlyoutMenuItemSelected;

      //vm = new ViewModels.MainViewModel();
      //this.BindingContext = vm;
      App.NavigationWorker = this.navigator;

      ContainerPage_NavigatedTo( null, null );
    }

    private async void ContainerPage_NavigatedTo(object sender, NavigatedToEventArgs e) {
      if (ViewModels.ViewModelBridge.TheConfigData == null) {
        await App.NavigationWorker.PushAsync( new StartPage() );
      }
      else {
        await App.NavigationWorker.PushAsync( new MainPage() );
      }
    }

    void OnFlyoutMenuItemSelected(object sender, SelectedItemChangedEventArgs e) {
      FlyoutPageItem item = e.SelectedItem as FlyoutPageItem;
      if (item != null) {
        if (App.NavigationWorker.CurrentPage != null) {
          var typeOfCurrentPage = App.NavigationWorker.CurrentPage.GetType();
          if (typeOfCurrentPage == item.TargetType) {
            return;
          }
        }

        var targetPage = (Page)Activator.CreateInstance( item.TargetType );
        //Detail = new NavigationPage( targetPage );
        App.NavigationWorker.PushAsync( targetPage );
        flyoutMenu.SelectedItem = null;
        //IsPresented = false;
      }
    }

  }

}