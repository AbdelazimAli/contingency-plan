using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Db.Persistence
{
    public class ImageProcesses
    {
        public static byte[] ReadFully(Stream input)
        {
            try
            {
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }

                    return ms.ToArray();
                }
            }
            catch
            {
                return null;
            }
        }

        public static void GetFull_ThumpImages(string Source,Stream _stream,string ContentType, out WebImage fullsize, out WebImage thumbs,out byte[] stream)
        {
            fullsize = null;
            thumbs = null;

             stream = ReadFully(_stream);
            if (Source == "EmployeePic")
            {
                fullsize = new WebImage(stream).Resize(180, 180);
                thumbs = new WebImage(stream).Resize(32, 32);
            }
            else if (Source == "CompanyLogo")
            {
                fullsize = new WebImage(stream).Resize(396, 130);
                thumbs = new WebImage(stream).Resize(80, 80);
            }
            else if (ContentType != "application/pdf")
            {
                fullsize = new WebImage(stream).Resize(1240, 1754); //   1240, 1754    2480, 3508
                thumbs = new WebImage(stream).Resize(124, 175);
            }
            else if (ContentType == "application/pdf")
            {
                // do nothing          
            }
            else
            {
                fullsize = new WebImage(stream).Resize(1240, 1754);
                thumbs = new WebImage(stream).Resize(80, 80);
            }
        }

        public static bool IsValidExtension(HttpPostedFileBase[] Images, string ValidExtensions)
        {
            try
            {
                if (Images == null)
                    return true;

                for (int i = 0; i < Images.Count(); i++)
                {
                    var Image = Images[i];
                    string ImageExt = Path.GetExtension(Image.FileName);

                    if (!ValidExtensions.Contains(ImageExt))
                        return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

       
    }
}
