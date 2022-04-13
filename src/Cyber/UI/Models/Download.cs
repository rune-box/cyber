using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.Data;

namespace Cyber.UI.Models {
  public class DownloadItem : ModelBase {

    public string Name {
      get { return GetValue<string>( NameProperty ); }
      set { SetValue( NameProperty, value ); }
    }

    public static readonly PropertyData NameProperty = RegisterProperty( nameof( Name ), typeof( string ), null );


    public string ImageUri {
      get { return GetValue<string>( ImageUriProperty ); }
      set { SetValue( ImageUriProperty, value ); }
    }

    public static readonly PropertyData ImageUriProperty = RegisterProperty( nameof( ImageUri ), typeof( string ), null );


    public int Percentage {
      get { return GetValue<int>( PercentageProperty ); }
      set { SetValue( PercentageProperty, value ); }
    }

    public static readonly PropertyData PercentageProperty = RegisterProperty( nameof( Percentage ), typeof( int ), 0 );

    public string Id { get; set; }
    public string FileName { get; set; }
    public Constants.AssetsType AssetType { get; set; }
    public Constants.NFTContentType ContentType { get; set; }

  }

  public class DownloadGroup : ObservableCollection<DownloadItem> {
    public string Name { get; private set; }
    public string IconSource { get; private set; }

    public DownloadGroup(string name, string iconSource, ObservableCollection<DownloadItem> items) : base( items ) {
      Name = name;
      IconSource = iconSource;
    }
  }

}
