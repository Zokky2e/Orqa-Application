using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using Orqa_Application.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using System.IO;
using Orqa_Application.ViewModels;
using ReactiveUI;

namespace Orqa_Application.Services
{
    public class NavigationService
    {
        UserService UserService;
        WorkPositionService WorkPositionService;
        public NavigationService(UserService userService, WorkPositionService workPositionService) 
        {
            UserService = userService;
            WorkPositionService = workPositionService;
        }
        public void RedirectLoggedInUser(int userId, string? role = null)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow?.Hide();
                Window targetWindow;
                if (role == null)
                {
                    var loginViewModel = new LoginViewModel(this, UserService);
                    targetWindow = new LoginWindow
                    {
                        DataContext = loginViewModel
                    };
                }
                else if (role == "admin")
                {
                    var adminViewModel = new AdminViewModel(this, UserService, WorkPositionService);

                    targetWindow = new AdminWindow()
                    {
                        DataContext = adminViewModel
                    };
                }
                else
                {
                    var userViewModel = new UserViewModel(this, UserService);

                    targetWindow = new UserWindow()
                    {
                        DataContext = userViewModel
                    };
                }
                desktop.MainWindow = targetWindow;
                desktop.MainWindow.Show();
            }
        }
    }
}
