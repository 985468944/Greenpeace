
namespace System
{
    public static class ExtendFunction
    {
        public static T fromJson<T>(this string jsonStr)
        {
            try
            {
                return new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<T>(jsonStr);
            }
            catch (Exception ex)
            {

            }
            return default(T);
        }
    }
}
