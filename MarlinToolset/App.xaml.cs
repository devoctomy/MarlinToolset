using Microsoft.Extensions.DependencyInjection;
using Splat;
using ReactiveUI;
using System.Windows;
using System;
using MarlinToolset.Views;
using Splat.Microsoft.Extensions.DependencyInjection;
using MarlinToolset.ViewModels;
using System.Diagnostics.CodeAnalysis;
using MarlinToolset.Core.Services;
using System.Reflection;
using System.Linq;

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
            ConfigureCommandProcessors(serviceCollection);
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
            services.AddScoped<IPrinterPacketParser, MarlinPrinterPacketParser>(x =>
            {
                var options = new MarlinPrinterPacketParserOptions()
                {
                };

                return new MarlinPrinterPacketParser(options);
            });
            services.AddSingleton<ISerialPortAdapter, SerialPortAdapter<WrappedSerialPort>>();
        }

        private void ConfigureViewsAndViewModels(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddTransient<IPrinterConfigurationView, PrinterConfigurationView>();
            services.AddTransient<IPrintersConfigurationView, PrintersConfigurationView>();
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<PrinterConfigurationViewModel>();
            services.AddTransient<PrintersConfigurationViewModel>();
        }

        private void ConfigureCommandProcessors(IServiceCollection services)
        {
            var assembly = Assembly.GetAssembly(typeof(ICommandProcessorService));

            var type = typeof(ICommandProcessorService);
            var types = assembly.GetTypes()
                .Where(p => type.IsAssignableFrom(p) && ! p.IsInterface)
                .ToArray();

            foreach(var curProcessor in types)
            {
                services.AddTransient<ICommandProcessorService>(sp =>
                {
                    return (ICommandProcessorService)Activator.CreateInstance(curProcessor);
                });
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = ServiceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }

    }
}
