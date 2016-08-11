using System;
using Microsoft.Owin.Hosting;
using tba.Core.Configuration;
using tba.Core.Utilities;
using tba.EFPersistence;
using tba.Restaurant.App.Models;
using tba.Restaurant.App.Services;
using tba.Restaurant.Configuration;
using tba.Restaurant.Entities;
using tba.Restaurant.WebApi.Context;

namespace tba.Restaurant.WebApi
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting web Server...");
            var cp = new ConfigurationProvider<RestaurantConfigurationSection>("AppConfiguration/Restaurants");
            var settings = cp.Read();
            IDisposable server = WebApp.Start<Startup>(settings.ServiceUrl);
            Console.WriteLine("Server running at {0} - press Enter to quit. ", settings.ServiceUrl);
            Console.ReadLine();
        }

    }
}
