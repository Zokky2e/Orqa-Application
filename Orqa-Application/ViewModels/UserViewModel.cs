using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Orqa_Application.Models;
using Orqa_Application.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;

namespace Orqa_Application.ViewModels
{
    public partial class UserViewModel : ReactiveViewModelBase
    {
        public IRelayCommand LogoutCommand { get; }

        public UserService _userService;
        public NavigationService _navigationService;
        public UserCardControlViewModel UserCardViewModel { get; }
        public bool HasWorkPosition { get; set; } = false;

        public UserViewModel(IServiceProvider services)
        {
            _navigationService = services.GetRequiredService<NavigationService>();
            _userService = services.GetRequiredService<UserService>();
            UserCardViewModel = new UserCardControlViewModel(_userService.CurrentUser, _userService.UserWorkPosition);
            LogoutCommand = new RelayCommand(OnLogout);
        }
        private void OnLogout()
        {
            _userService.ClearSession();
            _navigationService.NavigateTo("login");
        }
    }
}
