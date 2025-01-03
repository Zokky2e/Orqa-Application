using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MySql.Data.MySqlClient;
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


        public IRelayCommand LoginCommand { get; }

        public AdminViewModel()
        {
            LoginCommand = new RelayCommand(OnLogin);
        }
        private void OnLogin()
        {
            
        }
    }
}
