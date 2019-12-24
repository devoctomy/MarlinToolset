using Microsoft.Extensions.DependencyInjection;
using Splat;
using ReactiveUI;
using System.Windows;
using System.Reflection;
using MarlinToolset.Services;
using System;
using MarlinToolset.Views;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace MarlinToolset
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureReactive(serviceCollection);
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
            ServiceProvider.UseMicrosoftDependencyResolver();               //This has to be re-registered for Splat
        }

        private void ConfigureReactive(IServiceCollection services)
        {
            services.UseMicrosoftDependencyResolver();
            var resolver = Locator.CurrentMutable;
            resolver.InitializeSplat();
            resolver.InitializeReactiveUI();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IStoragePathService, WindowsStoragePathService>();
            services.AddSingleton<IPrinterConfigurationManagerService, PrinterConfigurationManagerService>();
            services.AddTransient<PrinterConfigurationView>();
            services.AddTransient<PrintersConfigurationView>();
            services.AddScoped<IPrinterControllerService, MarlinPrinterControllerService>();
            services.AddSingleton<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = ServiceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }

    }
}
