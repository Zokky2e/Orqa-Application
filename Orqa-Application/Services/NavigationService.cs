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

namespace Orqa_Application.Services
{
    public class NavigationService
    {
        public NavigationService() { }
        public void RedirectLoggedInUser(int userId, string? role = null)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                Window targetWindow = new Window();
                desktop.MainWindow?.Hide();
                if (role == null)
                {
                    targetWindow = new LoginWindow();
                } 
                else
                {
                    if (role == "admin")
                    {
                        var adminViewModel = new AdminViewModel(this, new UserService());

                        targetWindow = new AdminWindow()
                        {
                            DataContext = adminViewModel
                        };
                    }
                    else
                    {
                        var userViewModel = new UserViewModel();

                        targetWindow = new UserWindow()
                        {
                            DataContext = userViewModel
                        };
                    }
                }
                desktop.MainWindow = targetWindow;
                desktop.MainWindow.Show();
            }
        }
    }
}
