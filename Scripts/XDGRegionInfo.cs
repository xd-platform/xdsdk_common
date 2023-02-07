namespace XD.SDK.Common
{
    public interface XDGRegionInfo
    {
        string city { get; }
        string countryCode { get; }
        string timeZone{ get; }
        string locationInfoType { get; }
    }
}