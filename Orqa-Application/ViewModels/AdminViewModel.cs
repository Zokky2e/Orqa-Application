using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MySql.Data.MySqlClient;
using Orqa_Application.Services;
using System;
using System.Collections.Generic;

namespace Orqa_Application.ViewModels
{
    public partial class AdminViewModel : ViewModelBase
    {
        [ObservableProperty]
        public string username = "";

        [ObservableProperty]
        public string password = "";

        [ObservableProperty]
        public string result = "";

        [ObservableProperty]
        public bool hasResult = true;


        public IRelayCommand LogoutCommand { get; }

        public UserService _userService;
        public NavigationService _navigationService;

        public AdminViewModel(NavigationService navigationService, UserService userService)
        {
            _navigationService = navigationService;
            _userService = userService;
            LogoutCommand = new RelayCommand(OnLogout);
        }
        private void OnLogout()
        {
            _userService.ClearSession();
            _navigationService.RedirectLoggedInUser(0);
        }
    }
}
