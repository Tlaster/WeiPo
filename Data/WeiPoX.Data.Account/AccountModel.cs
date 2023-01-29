using Realms;
using WeiPoX.Data.Common;

namespace WeiPoX.Data.Account;

public partial class AccountModel : IRealmObject
{
    [PrimaryKey] public string AccountKeyRaw { get; set; } = string.Empty;

    public long LastActiveTime { get; set; }


    public MicroBlogKey AccountKey
    {
        get => MicroBlogKey.Parse(AccountKeyRaw);
        set => AccountKeyRaw = value.ToString();
    }
}

public enum CredentialsType
{
    OAuth,
    OAuth2
}