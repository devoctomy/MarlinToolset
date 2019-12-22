using Microsoft.Extensions.DependencyInjection;
using Splat;
using ReactiveUI;
using System.Windows;
using System.Reflection;
using MarlinToolset.Services;

namespace MarlinToolset
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();

            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IStoragePathService, WindowsStoragePathService>();
            services.AddSingleton<IPrinterConfigurationManagerService, PrinterConfigurationManagerService>();
            services.AddSingleton<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }

    }
}
