using System;
using System.Web.Script.Serialization;

namespace Merchandising.Portal.Utils
{
    public static class DemoUtils
    {
        public static string FruitsUrl = "~/Content/Pictures/Fruits/";

        public static void TryError()
        {
            var random = new Random();
            if (random.Next(10) > 5)
            {
                throw new Exception("a demo exception has occurred");
            }
        }

        public static void Error()
        {
            throw new Exception("a demo exception has occurred");
        }

        public static string Encode(object input)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = Int32.MaxValue;
            return jss.Serialize(input);
        }
    }
}