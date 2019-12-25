using Microsoft.Extensions.DependencyInjection;
using Splat;
using ReactiveUI;
using System.Windows;
using System.Reflection;
using MarlinToolset.Services;
using System;
using MarlinToolset.Views;
using Splat.Microsoft.Extensions.DependencyInjection;
using MarlinToolset.ViewModels;
using System.Diagnostics.CodeAnalysis;

namespace MarlinToolset
{
    [ExcludeFromCodeCoverage]
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureReactive(serviceCollection);
            ConfigureServices(serviceCollection);
            ConfigureViewsAndViewModels(serviceCollection);
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
            services.AddSingleton<IFileIOService, FileIOService>();
            services.AddSingleton<IStoragePathService, WindowsStoragePathService>();
            services.AddSingleton<IPrinterConfigurationManagerService, PrinterConfigurationManagerService>(sp =>
            {
                var service = new PrinterConfigurationManagerService(
                    sp.GetService<IStoragePathService>(),
                    sp.GetService<IFileIOService>());
                service.Load();
                return service;
            });
            services.AddTransient<PrinterConfigurationView>();
            services.AddTransient<PrintersConfigurationView>();
            services.AddScoped<IPrinterControllerService, MarlinPrinterControllerService>();
            services.AddSingleton<ISerialPortAdapter, SerialPortAdapter<WrappedSerialPort>>();
        }

        private void ConfigureViewsAndViewModels(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddTransient<PrinterConfigurationView>();
            services.AddTransient<PrintersConfigurationView>();
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<PrinterConfigurationViewModel>();
            services.AddTransient<PrintersConfigurationViewModel>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = ServiceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }

    }
}
