using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Configuration;
using Nancy;

namespace FTDBFlexgetServer
{
    public class APIModule : NancyModule
    {
        public APIModule()
        {
            Get["/"] = _ => String.Join
                            ("",
                                new SHA256Managed().ComputeHash
                                (
                                    Encoding.UTF8.GetBytes
                                    (
                                        File.ReadAllLines(ConfigurationManager.AppSettings["flexgetConfig"])
                                                .Single(s => s.StartsWith("      Cookie: \"WebsiteID="))
                                                .Replace("      Cookie: \"WebsiteID=", String.Empty)
                                                .Replace("\"", "")
                                    )
                                )
                                .Select(
                                            b => b.ToString("x2")
                                       )
                            );
            
            Get["/{key:length(26,26)}"] = parameters =>
            {
                String fileContent = File.ReadAllText(ConfigurationManager.AppSettings["flexgetConfig"]);
                string oldCookie = fileContent.Split('\n')
                                                .Single(s => s.StartsWith("      Cookie: \"WebsiteID="))
                                                .Replace("      Cookie: \"WebsiteID=", String.Empty)
                                                .Replace("\"", "");

                File.WriteAllText(ConfigurationManager.AppSettings["flexgetConfig"], fileContent.Replace(oldCookie, parameters.key));
                Console.WriteLine("Updated key: {0}", parameters.key);
                Process.Start(ConfigurationManager.AppSettings["flexgetBin"], "daemon reload");
                return "OK";
            };
        }
    }
}
