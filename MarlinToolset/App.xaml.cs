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
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
            ServiceProvider.UseMicrosoftDependencyResolver();
            //Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.UseMicrosoftDependencyResolver();
            var resolver = Locator.CurrentMutable;
            resolver.InitializeSplat();
            resolver.InitializeReactiveUI();

            services.AddSingleton<IStoragePathService, WindowsStoragePathService>();
            services.AddSingleton<IPrinterConfigurationManagerService, PrinterConfigurationManagerService>();
            services.AddScoped<PrinterConfigurationView>();
            services.AddScoped<PrintersConfigurationView>();
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
