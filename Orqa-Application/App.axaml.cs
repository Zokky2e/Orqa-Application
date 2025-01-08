using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orqa_Application.Data;
using Orqa_Application.Services;
using Orqa_Application.ViewModels;
using Orqa_Application.Views;

namespace Orqa_Application
{
    public partial class App : Application
    {
        public IServiceProvider Services { get; private set; }
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                try
                {
                    DisableAvaloniaDataAnnotationValidation();
                    var serviceCollection = new ServiceCollection();
                    ConfigureServices(serviceCollection);
                    Services = serviceCollection.BuildServiceProvider();
                    var workPositionService = Services.GetRequiredService<WorkPositionService>();
                    var userService = Services.GetRequiredService<UserService>();
                    var navigationService = Services.GetRequiredService<NavigationService>();
                    desktop.MainWindow = Services.GetRequiredService<MainWindow>();
                }
                catch (Exception e)
                {
                    var error = e;
                }
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Register DbContext
            services.AddDbContext<WorkstationDbContext>(options =>
                options.UseMySql(
                     "datasource=127.0.0.1;port=3306;username=root;password=;database=workstationdb",
                    new MySqlServerVersion(new Version(10, 4, 28))
                ));
            // Register MainWindow
            services.AddTransient<MainWindow>();

            // Register services
            services.AddTransient<UserService>();
            services.AddTransient<WorkPositionService>();
            services.AddTransient<NavigationService>();

            // Register ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddTransient<AdminViewModel>();
            services.AddTransient<EditUserWorkPositionViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddSingleton<UserCardControlViewModel>();
            services.AddTransient<UserViewModel>();
            services.AddTransient<WorkTableViewModel>();
        }

        private void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            // remove each entry found
            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
    }
}