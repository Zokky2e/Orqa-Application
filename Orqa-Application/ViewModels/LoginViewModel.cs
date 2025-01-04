using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MySql.Data.MySqlClient;
using Orqa_Application.Services;
using System;
using System.Data;

namespace Orqa_Application.ViewModels
{
    public partial class LoginViewModel : ViewModelBase
    {
        private readonly NavigationService _navigationService;
        private readonly UserService _userService;
        private MySqlConnection mySqlConnection { get; set; }

        [ObservableProperty]
        public string username = "";

        [ObservableProperty]
        public string password = "";

        [ObservableProperty]
        public string result = "";

        [ObservableProperty]
        public bool hasResult = true;

        public IRelayCommand LoginCommand { get; }

        public LoginViewModel(NavigationService navigationService, UserService userService)
        {
            _navigationService = navigationService;
            _userService = userService;
            LoginCommand = new RelayCommand(OnLogin);

            string sqlConnectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=workstationdb";
            mySqlConnection = new MySqlConnection(sqlConnectionString);
        }
        private void OnLogin()
        {
            string query = "SELECT id, password FROM users WHERE username = @Username LIMIT 1";
            MySqlCommand command = new MySqlCommand(query, mySqlConnection);
            command.CommandTimeout = 60;
            command.Parameters.AddWithValue("@Username", Username);

            try
            {
                mySqlConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    string storedHashedPassword = reader.GetString(1);
                    if (BCrypt.Net.BCrypt.Verify(Password, storedHashedPassword))
                    {
                        int userId = reader.GetInt32(0);
                        reader.Close();
                        this.RedirectLoggedInUser(userId);
                    }
                    else
                    {
                        Result = "Incorrect credentials!";
                        HasResult = false;
                    }
                }
                else {
                    Result = "Incorrect credentials!";
                    HasResult = false;
                }
            }catch(Exception e)
            {

            }
            finally
            {
                mySqlConnection.Close();
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
