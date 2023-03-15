using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.IO.Compression;

namespace RockSnifferBootstrapper
{
    internal class Updater
    {
        static string latestAssetUrl;
        static Version latestVersionNo;
        static Version currentVersionNo;

        public static void Update()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("https://api.github.com/repos/kokolihapihvi/RockSniffer/releases/latest"));
            
            request.Method = "GET";

            request.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string jsonString;
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                jsonString = reader.ReadToEnd();
            }

            try
            {
                dynamic requestItems = JsonConvert.DeserializeObject(jsonString);
                latestAssetUrl = Convert.ToString(requestItems.assets[0].browser_download_url);
                latestVersionNo = new Version(Convert.ToString(requestItems.tag_name).Replace("v", ""));

                if (Directory.Exists("RockSniffer"))
                {
                    string[] directories = Directory.GetDirectories("RockSniffer");
                    string firstDir = directories[0];

                    if (firstDir != null)
                    {
                        currentVersionNo = new Version(Path.GetFileName(firstDir).Replace("RockSniffer ", ""));
                    }

                    var result = latestVersionNo.CompareTo(currentVersionNo);
                    bool updateAvailable = result > 0;
                    if (updateAvailable)
                    {
                        DialogResult dialogResult = MessageBox.Show(string.Format("An update is available for RockSniffer ({0}). Do you want to update?", latestVersionNo), "RockSniffer Update Available", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            Directory.Delete("RockSniffer", true);
                            DownloadLatest();
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            //do something else
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Failed to update RockSniffer:\n {0}", ex.Message));
            }
        }

        public static void DownloadLatest()
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add("Accept: text/html, application/xhtml+xml, */*");
            webClient.Headers.Add("User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36");
            webClient.DownloadFile(new Uri(latestAssetUrl), "RSUpdate.zip");

            if (File.Exists("RSUpdate.zip"))
            {
                ZipFile.ExtractToDirectory("RSUpdate.zip", "RockSniffer");
            }
        }
    }
}
