namespace Cyber {
  public partial class App : Application {
    public static NavigationPage NavigationWorker;

    public App() {
      InitializeComponent();

      Client.AppHelper.LoadConfiguration();
      MainPage = new Views.ContainerPage();
    }
  }
}