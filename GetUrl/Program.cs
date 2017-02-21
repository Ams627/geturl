using System;
using System.IO;
using System.Net.Http;
using System.Linq;

namespace GetUrl
{
    internal class Program
    {
        private static void Main(string[] args)
        {   
            try
            {
                if (args.Count() != 1)
                {
                    throw new Exception("You must supply a URL");
                }
                var url = args[0];
                var outfilename = Path.GetFileName(url);
                using (var httpclient = new HttpClient())
                {
                    var stream = httpclient.GetStreamAsync(url).Result;
                    using (var reader = new StreamReader(stream))
                    using (var outfile = new StreamWriter(outfilename))
                    {
                        var buffer = new char[16 * 1024 * 1024];
                        while (!reader.EndOfStream)
                        {
                            var bytesRead = reader.ReadBlock(buffer, 0, buffer.Count());
                            outfile.Write(buffer, 0, bytesRead);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var codeBase = System.Reflection.Assembly.GetEntryAssembly().CodeBase;
                var progname = Path.GetFileNameWithoutExtension(codeBase);
                Console.Error.WriteLine(progname + ": Error: " + ex.Message);
            }
        }
    }
}
