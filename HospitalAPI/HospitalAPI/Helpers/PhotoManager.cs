using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace HospitalAPI.Helpers
{
    public static class PhotoManager
    {
        public static string Upload(string FileRaw, string ext, string folderName)
        {
            var imageDataByteArray = Convert.FromBase64String(FileRaw);

            var imageDataStream = new MemoryStream(imageDataByteArray)
            {
                Position = 0
            };
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + ext;

            var path = Path.Combine(
                       AppDomain.CurrentDomain.BaseDirectory, "Uploads", folderName,
                       filename);

            FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);

            imageDataStream.WriteTo(file);
            file.Close();
            return filename;
        }

        public static void Delete(string folderName, string filename)
        {
            string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Uploads"), folderName, filename);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}