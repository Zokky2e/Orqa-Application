using Microsoft.Extensions.DependencyInjection;
using Orqa_Application.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orqa_Application.ViewModels
{
    public class MainViewModel : ReactiveObject
{
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set => this.RaiseAndSetIfChanged(ref _currentView, value);
        }

        public UserService UserService { get; }
        public WorkPositionService WorkPositionService { get; }
        public NavigationService NavigationService { get; }

        public MainViewModel(IServiceProvider services)
        {
            NavigationService = services.GetRequiredService<NavigationService>();
            NavigationService.ViewChanged += view => CurrentView = view;
            UserService = services.GetRequiredService<UserService>();
            WorkPositionService = services.GetRequiredService<WorkPositionService>();

            // Set the initial view
            string sessionFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "session.txt");
            if (File.Exists(sessionFilePath))
            {
                int userId = int.Parse(File.ReadAllText(sessionFilePath));
                string? role = UserService.CheckSession(userId);
                if (role != null)
                {
                    NavigationService.NavigateTo(role);
                }
            }
            else
            {
                NavigationService.NavigateTo("login");
            }
        }
    }
}
