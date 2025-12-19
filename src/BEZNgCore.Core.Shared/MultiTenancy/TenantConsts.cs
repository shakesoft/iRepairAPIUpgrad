namespace BEZNgCore.MultiTenancy;

public class TenantConsts
{
    public const string TenancyNameRegex = "^[a-zA-Z][a-zA-Z0-9_-]{1,}$";

    public const string DefaultTenantName = "TEST";//"TCOC";// "HGCB";//  "TCOH";//"HGCM";// "HGCAW";//"JCHGC";// "HGCH";//"HGCL";// "HCO"; //"Chancellor";//"Default";

    public const int MaxNameLength = 128;

    public const int DefaultTenantId = 1;
}

