using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Orqa_Application.Services;
using Orqa_Application.ViewModels;
using Orqa_Application.Views;

namespace Orqa_Application
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                DisableAvaloniaDataAnnotationValidation();
                string sessionFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "session.txt");
                var connectionService = new ConnectionService();
                var workPositionService = new WorkPositionService(connectionService);
                var userService = new UserService(connectionService);
                var navigationService = new NavigationService(userService, workPositionService);
                if (File.Exists(sessionFilePath))
                {
                    int userId = int.Parse(File.ReadAllText(sessionFilePath));
                    string? role = userService.CheckSession(userId);
                    if (role != null)
                    {
                        navigationService.RedirectLoggedInUser(userId, role);
                    }
                }
                else
                {
                    var loginViewModel = new LoginViewModel(navigationService, userService, connectionService);

                    desktop.MainWindow = new LoginWindow
                    {
                        DataContext = loginViewModel
                    };
                }
            }

            base.OnFrameworkInitializationCompleted();
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