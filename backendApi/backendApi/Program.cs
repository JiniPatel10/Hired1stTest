using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace backendApi
{
    public class Program
    {
        [Obsolete]
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel(opt =>
                {
                    opt.Limits.MinRequestBodyDataRate = null;
                    opt.Limits.MaxRequestBodySize = 209715200;
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }  
   

}
