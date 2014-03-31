using System;
using IPAParser;

namespace Run
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Path to the IPA file
            const string filePath = @"C:\Users\subodh\Downloads\IOS Sample IPAs\WirelessAdHocDemo.ipa";

            //Parsing the IPA file
            IOSBundleData bundleData = IOSBundle.GetIpaBundleData(filePath);

            Console.WriteLine(bundleData.BundleAppName);
            Console.WriteLine(bundleData.BundleIdentifier);
            Console.WriteLine(bundleData.BundleVersion);

            Console.ReadLine();
        }
    }
}