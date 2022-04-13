//using CommunityToolkit.Maui;
//using Telerik.Maui.Controls.Compatibility;

namespace Cyber {
  public static class MauiProgram {
    public static MauiApp CreateMauiApp() {
      var builder = MauiApp.CreateBuilder();
      builder
        //.UseTelerik()
        .UseMauiApp<App>()
        .ConfigureFonts( fonts => {
          fonts.AddFont( "OpenSans-Regular.ttf", "OpenSansRegular" );
        } );
      //.UseMauiCommunityToolkit();
      //builder.UseMauiApp<App>().UseMauiCommunityToolkit();

      return builder.Build();
    }
  }
}