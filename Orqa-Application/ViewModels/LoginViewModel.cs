using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Orqa_Application.Data;
using Orqa_Application.Services;
using System;
using System.Data;

namespace Orqa_Application.ViewModels
{
    public partial class LoginViewModel : ObservableViewModelBase
    {
        private readonly NavigationService _navigationService;
        private readonly UserService _userService;

        [ObservableProperty]
        public string username = "";

        [ObservableProperty]
        public string password = "";

        [ObservableProperty]
        public string result = "";

        [ObservableProperty]
        public bool hasResult = true;

        public IRelayCommand LoginCommand { get; }

        public LoginViewModel(IServiceProvider services)
        {
            _navigationService = services.GetRequiredService<NavigationService>();
            _userService = services.GetRequiredService<UserService>();
            LoginCommand = new RelayCommand(OnLogin);
        }
        private void OnLogin()
        {
            int userId = _userService.LoginUser(Username, Password);
            if (userId != 0)
            {
                this.RedirectLoggedInUser(userId);
            }
            else
            {
                Result = "Incorrect credentials!";
                HasResult = true;
            }
        }

        private void RedirectLoggedInUser(int userId)
        {
            string? role = _userService.CheckSession(userId);
            _navigationService.NavigateTo(role!);
            _userService.SaveSession(userId);
        }

    }
}
