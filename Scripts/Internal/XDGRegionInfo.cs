namespace XD.SDK.Common.Internal
{
    public interface XDGRegionInfo
    {
        string city { get; }
        string countryCode { get; }
        string timeZone{ get; }
        string locationInfoType { get; }
    }
}