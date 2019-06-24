using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSentry("https://1c371401f8c845a7aba995ca52a33465@sentry.io/1488632");
    }
}
