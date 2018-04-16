using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Stocks.Util
{
    public class Images
    {
        public static  List<Uri> bitmapImages = LoadImages();
        private static string currentPaht =  Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

        private Images()
        {
        }

        private static List<Uri> LoadImages()
        {
            

            return new List<Uri>
            {
               new Uri(Path.Combine(Environment.CurrentDirectory, "1.png")),
               new Uri(Path.Combine(Environment.CurrentDirectory, "2.png")),
               new Uri(Path.Combine(Environment.CurrentDirectory, "3.png")),
            };
        }
        
    }
}
