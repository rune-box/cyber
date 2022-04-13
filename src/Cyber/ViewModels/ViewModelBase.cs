using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyber.ViewModels {
  public class ViewModelBase : Catel.MVVM.ViewModelBase {
    public DataLayer.AccountContext DBAccount {
      get {
        return ViewModelBridge.DBAccount;
      }
    }
    public DataLayer.NFTContext DBNFT {
      get {
        return ViewModelBridge.DBNFT;
      }
    }

    //public static Catel.Services.MessageService MsgService;

    public ViewModelBase() {
      //if (MsgService == null) {
      //}
    }

  }

}
