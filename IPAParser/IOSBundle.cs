using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.Zip;

namespace IPAParser
{
    public class IOSBundle
    {
        public static IOSBundleData GetIpaBundleData(string filePath)
        {
            Dictionary<string, object> plist = GetIpaPList(filePath);
            return new IOSBundleData
            {
                BundleAppName = plist["CFBundleName"].ToString(),
                BundleIdentifier = plist["CFBundleIdentifier"].ToString(),
                BundleVersion = plist["CFBundleVersion"].ToString()
            };
        }

        private static Dictionary<string, object> GetIpaPList(string filePath)
        {
            var plist = new Dictionary<string, object>();
            var zip = new ZipInputStream(File.OpenRead(filePath));
            using (var filestream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var zipfile = new ZipFile(filestream);
                ZipEntry item;


                while ((item = zip.GetNextEntry()) != null)
                {
                    Match match = Regex.Match(item.Name.ToLower(), @"Payload/([A-Za-z0-9\-. ]+)\/info.plist$",
                        RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        var bytes = new byte[50*1024];

                        using (Stream strm = zipfile.GetInputStream(item))
                        {
                            int size = strm.Read(bytes, 0, bytes.Length);

                            using (var s = new BinaryReader(strm))
                            {
                                var bytes2 = new byte[size];
                                Array.Copy(bytes, bytes2, size);
                                plist = (Dictionary<string, object>) PlistCS.readPlist(bytes2);
                            }
                        }


                        break;
                    }
                }
            }
            return plist;
        }
    }
}