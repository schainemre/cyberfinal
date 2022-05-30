using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace siberodev
{
    class Program
    {
        static void Main(string[] args)
        {

            string link;
            int port;

            Console.Write("linki Girin: ");
            link = Console.ReadLine();
            Console.Write("portu girin: ");
            port = Convert.ToInt32(Console.ReadLine());

            // işlem oluşturdum ve nmap.exe uygulamasını çalıştırdım 
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "nmap.exe";
            startInfo.Arguments = "-p " + port + " --script http-sql-injection " + link + " -oX \"C:/Users/pc-34154/Desktop/cikti.xml\"";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();



            // xml dosyasını okutuyoruz 
            XmlDocument doc = new XmlDocument();
            doc.Load("C:/Users/pc-34154/Desktop/cikti.xml");
            XmlNodeList elemList = doc.GetElementsByTagName("script");

            JArray result = new JArray(   //verileri bir json array oluşturup içine kaydediyoruz.
               new JObject(
                        new JProperty("Nmap command", startInfo.Arguments)),
                new JObject(
                        new JProperty("Url", elemList[0].Attributes["output"].Value))); //Script'ten sadece output kısmını çağırıyorum

            // json dosyası oluşturuyorum.

            File.WriteAllText(@"C:\Users\pc-34154\Desktop\cikti.json", result.ToString()); //kendi bilgisayarınıza göre ayarlayın bu kısmı. 
            using (StreamWriter dosya = File.CreateText(@"C:\Users\pc-34154\Desktop\cikti.json"))
            using (JsonTextWriter ciktial = new JsonTextWriter(dosya))
            {
                result.WriteTo(ciktial);
            }



        }






    }
}