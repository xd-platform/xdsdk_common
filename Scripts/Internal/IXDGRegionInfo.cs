namespace XD.SDK.Common.Internal
{
    public interface IXDGRegionInfo
    {
        string city { get; }
        string countryCode { get; }
        string timeZone{ get; }
        string locationInfoType { get; }
    }
}