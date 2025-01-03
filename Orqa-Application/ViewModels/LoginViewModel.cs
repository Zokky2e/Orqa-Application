using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;
using Orqa_Application.Views;
using System;
using System.Collections.Generic;
using Tmds.DBus.Protocol;

namespace Orqa_Application.ViewModels
{
    public partial class LoginViewModel : ViewModelBase
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

        private MySqlConnection mySqlConnection { get; set; }

        public LoginViewModel()
        {
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
            string query = "SELECT roles.name FROM roles inner join user_roles on user_roles.roleId = roles.id inner join users on users.id = user_roles.userId where users.id = @UserId;";
            MySqlCommand command = new MySqlCommand(query, mySqlConnection);
            command.CommandTimeout = 60;
            command.Parameters.AddWithValue("@UserId", userId);
            try
            {
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    string role = reader.GetString(0);
                    if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                    {
                        desktop.MainWindow?.Hide();
                        if (role == "admin")
                        {
                            var adminWindow = new AdminWindow();
                            desktop.MainWindow = adminWindow;
                            desktop.MainWindow.Show();
                        }
                        else if (role == "user")
                        {
                            var userWindow = new UserWindow();
                            desktop.MainWindow = userWindow;
                            desktop.MainWindow.Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Result = "An error occurred!";
                HasResult = false;
            }
        }
    }
}
