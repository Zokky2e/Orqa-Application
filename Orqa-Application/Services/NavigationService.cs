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
using Microsoft.Extensions.DependencyInjection;

namespace Orqa_Application.Services
{
    public class NavigationService
    {
        private readonly IServiceProvider _services;

        public event Action<object>? ViewChanged;
        public NavigationService(IServiceProvider services)
        {
            _services = services;
        }
        public void NavigateTo(string viewName, object? parameter = null)
        {
            object viewModel = viewName.ToLower() switch
            {
                "admin" => _services.GetRequiredService<AdminViewModel>(),
                "user" => _services.GetRequiredService<UserViewModel>(),
                "login" => _services.GetRequiredService<LoginViewModel>(),
                _ => _services.GetRequiredService<LoginViewModel>(),
            };

            ViewChanged?.Invoke(viewModel);
        }
    }
}
