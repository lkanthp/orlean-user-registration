using System.Reflection;
using System.Threading.Tasks;
using GrainInterfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace SiloHost
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new SiloHostBuilder()
                .UseLocalhostClustering()
                //.UseDashboard(options => { })
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansService";
                })
                    .UseAdoNetClustering(options =>
                    {
                        options.Invariant = "MySql.Data.MySqlClient";
                        options.ConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
                        Console.WriteLine("Connection string: " + options.ConnectionString);
                    })
                    .AddAdoNetGrainStorage("MySqlStore", options =>
                    {
                        options.Invariant = "MySql.Data.MySqlClient";
                        options.ConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
                        options.UseJsonFormat = true;
                        Console.WriteLine("Connection string: " + options.ConnectionString);
                    })
                //.AddMemoryGrainStorage("PubSubStore")
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IUserGrain).Assembly).WithReferences())
                    .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            await host.StartAsync();

            await Task.Delay(-1);
        }
    }
}