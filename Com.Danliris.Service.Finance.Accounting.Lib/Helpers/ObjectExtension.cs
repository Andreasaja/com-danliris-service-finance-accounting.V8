using Newtonsoft.Json;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Helpers
{
    public static class ObjectExtension
    {
        public static T Clone<T>(this T objSource)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(objSource), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}
