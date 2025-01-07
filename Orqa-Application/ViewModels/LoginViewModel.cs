using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MySql.Data.MySqlClient;
using Orqa_Application.Services;
using System;
using System.Data;

namespace Orqa_Application.ViewModels
{
    public partial class LoginViewModel : ObservableViewModelBase
    {
        private readonly NavigationService _navigationService;
        private readonly UserService _userService;
        private readonly ConnectionService _connectionService;

        [ObservableProperty]
        public string username = "";

        [ObservableProperty]
        public string password = "";

        [ObservableProperty]
        public string result = "";

        [ObservableProperty]
        public bool hasResult = true;

        public IRelayCommand LoginCommand { get; }

        public LoginViewModel(NavigationService navigationService, UserService userService, ConnectionService connectionService)
        {
            _navigationService = navigationService;
            _userService = userService;
            _connectionService = connectionService;
            LoginCommand = new RelayCommand(OnLogin);
        }
        private void OnLogin()
        {
            string query = "SELECT id, password FROM users WHERE username = @Username LIMIT 1";
            MySqlCommand command = new MySqlCommand(query, _connectionService.MySqlConnection);
            command.CommandTimeout = 60;
            command.Parameters.AddWithValue("@Username", Username);
            bool success = false;
            int userId = 0;
            try
            {
                _connectionService.MySqlConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    string storedHashedPassword = reader.GetString(1);
                    if (BCrypt.Net.BCrypt.Verify(Password, storedHashedPassword))
                    {
                        userId = reader.GetInt32(0);
                        reader.Close();
                        success = true;
                    }
                    else
                    {
                        Result = "Incorrect credentials!";
                        success = false;
                        HasResult = false;
                    }
                }
                else {
                    Result = "Incorrect credentials!";
                    success = false;
                    HasResult = false;
                }
            }catch(Exception e)
            {

            }
            finally
            {
                _connectionService.MySqlConnection.Close();
                if (success && userId != 0)
                {
                    this.RedirectLoggedInUser(userId);
                }
            }
        }

        private void RedirectLoggedInUser(int userId)
        {
            string? role = _userService.CheckSession(userId);
            _navigationService.RedirectLoggedInUser(userId, role);
            _userService.SaveSession(userId);
        }

    }
}
