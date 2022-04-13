using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models {
  [Table( "Accounts" )]
  public class Account {
    public string Name { get; set; }
    public string Remark { get; set; }

    [Required]
    [Key]
    public string Address { get; set; }
    public string EvmAddress { get; set; }

    public bool? IsEVM { get; set; }
    public string Chain { get; set; }

    public static Account CreateEvmAccount(string address, string name, string remark) {
      return new Account {
        Name = name,
        Address = address,
        EvmAddress = address,
        IsEVM = true,
        Remark = remark
      };
    }

    public static Account CreateAccount(string address, string evmAddress, string chain, string name, string remark) {
      return new Account {
        Name = name,
        Address = address,
        EvmAddress = evmAddress,
        Chain = chain,
        Remark = remark
      };
    }

    public static void Update(Account account, Account newData) {
      account.Name = newData.Name;
      account.Remark = newData.Remark;
      account.IsEVM = newData.IsEVM;
      account.EvmAddress = newData.EvmAddress;
      account.Chain = newData.Chain;
    }

  }
}
