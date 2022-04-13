using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyber.Lib {
  public static class CryptoHelper {
    static System.Security.Cryptography.MD5 MD5 = System.Security.Cryptography.MD5.Create();

    public static string MD5Encrypt(string input) {
      if (input == null || input.Length == 0)
        return string.Empty;

      byte[] inputBytes = System.Text.UTF8Encoding.UTF8.GetBytes( input );
      return MD5Encrypt( inputBytes );
    }

    public static string MD5Encrypt(byte[] inputBytes) {
      if (inputBytes == null || inputBytes.Length == 0)
        return string.Empty;
      byte[] bytes = MD5.ComputeHash( inputBytes );
      var result = StringUtility.BytesToString( bytes );
      return result;
    }
  }

}
