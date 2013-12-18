using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;

namespace PicasaDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Picasa Downloader");
            
            String xmlUrl;
            do
            {
                Console.Write("Please enter the picasa rss feed url: ");
                xmlUrl = Console.ReadLine();
            } while (String.IsNullOrWhiteSpace(xmlUrl));
            
            String filePath;
            do
            {
                Console.Write("Please enter the download location: ");
                filePath = Console.ReadLine();
            } while (String.IsNullOrWhiteSpace(filePath) || !Directory.Exists(filePath));

            var xDoc = new XmlDocument();
            xDoc.Load(xmlUrl);
            var nodes = xDoc.SelectNodes("//rss/channel/item/enclosure");
            
            if( nodes != null && nodes.Count > 0 )
            {
                var webClient = new WebClient();
                for (int i = 0; i < nodes.Count; i++)
                {
                    var url = nodes.Item(i).Attributes["url"].InnerText;
                    var uri = new Uri(url);
                    Console.WriteLine("Downloading [{0}/{1}]...", i+1, nodes.Count);
                    var data = webClient.DownloadData (uri);
                    var targetPath = Path.Combine(filePath, uri.Segments[uri.Segments.Count() -1 ]);
                    File.WriteAllBytes(targetPath, data);
                }
            }

            Console.WriteLine("Finished downloading..");
            Console.ReadLine();
        }
    }
}
