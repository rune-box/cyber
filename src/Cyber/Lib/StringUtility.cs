using System.Collections.Specialized;
using System.Text;

namespace Cyber.Lib {
  public class StringUtility {
    public static string BytesToString(byte[] bytes, string byteFormat = "x2") {
      StringBuilder sb = new StringBuilder();
      foreach (byte b in bytes)
        sb.Append( b.ToString( "x2" ) );
      return sb.ToString();
    }

    public static string Base64Encode(string plainText) {
      var plainTextBytes = System.Text.Encoding.UTF8.GetBytes( plainText );
      return System.Convert.ToBase64String( plainTextBytes );
    }

    public static string Base64Decode(string base64EncodedData) {
      var base64EncodedBytes = System.Convert.FromBase64String( base64EncodedData );
      return System.Text.Encoding.UTF8.GetString( base64EncodedBytes );
    }

    public static bool TryGetValueByName(NameValueCollection collection, string key, bool defaultValue) {
      string vStr = collection[key];
      bool v = defaultValue;
      if (!string.IsNullOrEmpty( vStr ))
        bool.TryParse( vStr, out v );
      return v;
    }

    public static bool TryGetValueByName(IEnumerable<KeyValuePair<string, string>> collection, string key, bool defaultValue) {
      bool v = defaultValue;
      var keys = collection.Select( i => i.Key ).ToList();
      int index = keys.IndexOf( key );
      if(index >= 0) {
        var kvp = collection.ElementAt( index );
        string vStr = kvp.Value;
        if (!string.IsNullOrEmpty( vStr ))
          bool.TryParse( vStr, out v );
      }
      return v;
    }

    public static string TryGetValueByName(NameValueCollection collection, string key, string defaultValue) {
      string vStr = collection[key];
      string v = !string.IsNullOrEmpty( vStr ) ? vStr : defaultValue;
      return v;
    }

    public static string TryGetValueByName(IEnumerable<KeyValuePair<string, string>> collection, string key, string defaultValue) {
      string v = defaultValue;
      var keys = collection.Select( i => i.Key ).ToList();
      int index = keys.IndexOf( key );
      if (index >= 0) {
        var kvp = collection.ElementAt( index );
        v = kvp.Value;
      }
      return v;
    }

    /// <summary>
    /// jpg -> image/jpeg
    /// </summary>
    /// <param name="ext"></param>
    /// <returns></returns>
    public static string ExtensionNameToContentType(string ext) {
      string ctFormat = "image/{0}";
      switch (ext) {
        case "bmp":
        case "gif":
        case "jpeg":
        case "png":
        case "tiff":
          return string.Format( ctFormat, ext );
        case "jpg":
          return string.Format( ctFormat, "jpeg" );
        case "svg":
          return string.Format( ctFormat, "svg+xml" );
      }
      return "image";
    }

    public static string GetExtensionFromMediaType(string mediaType) {
      if (string.IsNullOrEmpty( mediaType ))
        return null;
      string imagePrefix = "image/";
      if (mediaType.EndsWith( "svg+xml" ))
        return ".svg";
      else if (mediaType.StartsWith( imagePrefix ))
        return "." + mediaType.Substring( imagePrefix.Length );
      return null;
    }

  }

}
