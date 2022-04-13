namespace Cyber.Views {
	public partial class ImageView : ContentPage {
		Timer timer;
		List<DataLayer.Models.NFTItem> data;
		public ImageView() {
			InitializeComponent();
			initialize();
		}

		void initialize() {
			if (ViewModels.ViewModelBridge.TestData_Items == null)
				return;
			data = ViewModels.ViewModelBridge.TestData_Items.Where( x => x.ImageURL != null ).ToList();
			if (data.Count == 0)
				return;

			timer = new Timer((e) => {
				setImage(e);
			}, image, 1000, 7000 );
		}

	 	async void setImage(object sender) {
			var img = sender as Image;
			int index = Lib.RandomUtility.CreateRandom( 0, data.Count );
			var item = data[index];
			//string ext = Lib.StringUtility.GetExtensionFromMediaType( item.MediaType );
			string folder = Lib.FileHelper.GetAssetsFolder( item.MediaType );
			if (folder == null)
				folder = ViewModels.ViewModelBridge.AssetsFolders.NFTImages;
			string path = Path.Combine( folder, item.HashId + ".png" );
			if(!File.Exists(path))
				path = Path.Combine( folder, item.HashId + ".jpg" );
			if (!File.Exists( path ))
				path = Path.Combine( folder, item.HashId + ".jpeg" );
			if (!File.Exists( path ))
				path = Path.Combine( folder, item.HashId + ".svg" );
			
			img.Dispatcher.Dispatch( new Action( () => {
				//img.Source = path;
			 	var imgSource = ImageSource.FromFile( path );
				img.Source = imgSource;
			} ) );
		}

	}
}