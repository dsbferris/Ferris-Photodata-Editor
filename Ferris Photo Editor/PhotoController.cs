using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ferris_Photo_Editor
{
    static class PhotoController
    {


        public static void SetDateTaken(string input, DateTime dateTime, string output)
        {
            if (File.Exists(input))
            {
                try
                {
                    using (FileStream fs = new FileStream(input, FileMode.Open))
                    using (Image myImage = Image.FromStream(fs, false, false))
                    {
                        PropertyItem propItem36867 = GetPropItem36867(dateTime);
                        myImage.SetPropertyItem(propItem36867);
                        myImage.Save(output);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                
            }
        }

        private static PropertyItem GetPropItem36867(DateTime dateTime)
        {
            var newItem = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
            newItem.Id = 36867;
            newItem.Len = 20;
            newItem.Type = 2;
            newItem.Value = Encoding.UTF8.GetBytes(dateTime.ToString("yyyy:MM:dd HH:mm:ss") + '\0');
            return newItem;
        }


        //we init this once so that if the function is repeatedly called
        //it isn't stressing the garbage man
        private static Regex r = new Regex(":");

        public static DateTime GetDateTaken(string path)
        {
            if (File.Exists(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (Image myImage = Image.FromStream(fs, false, false))
                {
                    try
                    {
                        PropertyItem propItem = myImage.GetPropertyItem(36867);
                        string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                        return DateTime.Parse(dateTaken);
                    }
                    catch (Exception)
                    {
                        return DateTime.MinValue;
                        throw;
                    }
                }
            }
            else return DateTime.MinValue;
        }

        /*
        private static string GetDateNamePath(string filepath, DateTime dateTime)
        {
            string s = dateTime.Year.ToString();
            s += (dateTime.Month < 10) ? ("0" + dateTime.Month) : (dateTime.Month.ToString());
            s += (dateTime.Day < 10) ? ("0" + dateTime.Day) : (dateTime.Day.ToString());
            s += "_";
            s += (dateTime.Hour < 10) ? ("0" + dateTime.Hour) : (dateTime.Hour.ToString());
            s += (dateTime.Minute < 10) ? ("0" + dateTime.Minute) : (dateTime.Minute.ToString());
            s += (dateTime.Second < 10) ? ("0" + dateTime.Second) : (dateTime.Second.ToString());

            FileInfo fi = new FileInfo(filepath);
            return fi.FullName.Replace(fi.Name, s) + fi.Extension;
        }
        */

        /*
        private static List<DateFile> GetDateFiles(IEnumerable<string> files)
        {
            List<DateFile> datefiles = new List<DateFile>();
            foreach (var f in files)
            {
                foreach (var ex in exts)
                {
                    if (f.EndsWith(ex, true, CultureInfo.CurrentCulture))
                    {
                        datefiles.Add(new DateFile(f, GetDateTakenFromImage(f)));
                        break;
                    }
                }
            }
            return datefiles;
        }
        */



        private static string numberPattern = "_{0}";

        public static string NextAvailableFilename(string path)
        {
            // Short-cut if already available
            if (!File.Exists(path))
                return path;

            // If path has extension then insert the number pattern just before the extension and return next filename
            if (Path.HasExtension(path))
                return GetNextFilename(path.Insert(path.LastIndexOf(Path.GetExtension(path)), numberPattern));

            // Otherwise just append the pattern to the path and return next filename
            return GetNextFilename(path + numberPattern);
        }

        private static string GetNextFilename(string pattern)
        {
            string tmp = string.Format(pattern, 1);
            if (tmp == pattern)
                throw new ArgumentException("The pattern must include an index place-holder", "pattern");

            if (!File.Exists(tmp))
                return tmp; // short-circuit if no matches

            int min = 1, max = 2; // min is inclusive, max is exclusive/untested

            while (File.Exists(string.Format(pattern, max)))
            {
                min = max;
                max *= 2;
            }

            while (max != min + 1)
            {
                int pivot = (max + min) / 2;
                if (File.Exists(string.Format(pattern, pivot)))
                    min = pivot;
                else
                    max = pivot;
            }

            return string.Format(pattern, max);
        }
    }
}
