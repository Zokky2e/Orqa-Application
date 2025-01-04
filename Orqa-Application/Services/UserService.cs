using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.IO;

namespace Orqa_Application.Services
{
    public class UserService
    {
        private MySqlConnection mySqlConnection { get; set; }
        public UserService()
        {
            string sqlConnectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=workstationdb";
            mySqlConnection = new MySqlConnection(sqlConnectionString);
        }

        public void SaveSession(int userId)
        {
            string sessionFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "session.txt");
            File.WriteAllText(sessionFilePath, userId.ToString());
        }

        public void ClearSession()
        {
            string sessionFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "session.txt");
            if (File.Exists(sessionFilePath))
            {
                File.Delete(sessionFilePath);
            }
        }

        public string? CheckSession(int userId)
        {
            string query = "SELECT roles.name FROM roles INNER JOIN user_roles ON user_roles.roleId = roles.id INNER JOIN users ON users.id = user_roles.userId WHERE users.id = @UserId;";
            MySqlCommand command = new MySqlCommand(query, mySqlConnection);
            command.CommandTimeout = 60;
            command.Parameters.AddWithValue("@UserId", userId);
            string? role = null;
            try
            {
                mySqlConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    role = reader.GetString(0);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                mySqlConnection.Close();
            }
            return role;
        }
    }
}
