using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalAPI.Helpers
{
    public static class GetExtension
    {
        public static string GetFileExtension(string base64String)
        {
            // determine the file extension according to its 5 characters in the base64 code...
            var data = base64String.Substring(0, 5);

            // accept only: .png, .jpg, .ico
            switch (data.ToUpper())
            {
                case "IVBOR":
                    return "png";
                case "/9J/4":
                    return "jpg";
                case "AAABA":
                    return "ico";
                    // if not one of the three file formats above, return empty...
                default:
                    return string.Empty;
            }
        }
    }
}