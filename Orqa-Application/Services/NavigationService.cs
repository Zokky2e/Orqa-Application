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

namespace Orqa_Application.Services
{
    public class NavigationService
    {
        public NavigationService() { }
        public void RedirectLoggedInUser(int userId, string role)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow?.Hide();

                Window targetWindow = role == "admin" ? new AdminWindow() : new UserWindow();
                desktop.MainWindow = targetWindow;
                desktop.MainWindow.Show();
            }
        }
    }
}
