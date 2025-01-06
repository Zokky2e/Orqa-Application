﻿using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using MySqlX.XDevAPI.Common;
using Orqa_Application.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Transactions;

namespace Orqa_Application.Services
{
    public class UserService
    {
        public UserModel CurrentUser {  get; set; } = new UserModel();
        public UserWorkPositionModel? UserWorkPosition { get; set; }
        public ConnectionService ConnectionService { get; set; }
        public UserService(ConnectionService connectionService) 
        {
            ConnectionService = connectionService;
        }

        public void AddUser(UserModel user, string password)
        {
            if (user == null)
            {
                return;
            }
            MySqlTransaction transaction = null;
            string addUserQuery = "INSERT INTO `users` (`username`, `firstname`, `lastname`, `password`) VALUES(@Username, @Firstname, @Lastname, @Password)";
            string addUserRoleQuery = "INSERT INTO `user_roles` (`userId`, `roleId`) VALUES(@UserId, @RoleId)";
            try
            {
                ConnectionService.MySqlConnection.Open();
                transaction = ConnectionService.MySqlConnection.BeginTransaction();
                using (MySqlCommand command = ConnectionService.MySqlConnection.CreateCommand())
                {
                    command.Connection = ConnectionService.MySqlConnection;
                    command.CommandText = addUserQuery;
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Firstname", user.Firstname);
                    command.Parameters.AddWithValue("@Lastname", user.Lastname);
                    command.Parameters.AddWithValue("@Password", BCrypt.Net.BCrypt.HashPassword(password));
                    command.CommandTimeout = 60;
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                    command.CommandText = "SELECT LAST_INSERT_ID()";
                    int newUserIdAfterCommit = Convert.ToInt32(command.ExecuteScalar());
                    command.CommandText = addUserRoleQuery;
                    command.Parameters.AddWithValue("@UserId", newUserIdAfterCommit);
                    command.Parameters.AddWithValue("@RoleId", 2);
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                throw new Exception("Error adding user", ex);
            }
            finally
            {
                ConnectionService.MySqlConnection.Close();
            }
        }

        public void SaveSession(int userId)
        {
            string sessionFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "session.txt");
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
            string query = @"SELECT 
            roles.name, 
            roles.id,
            users.username,
            users.firstname,
            users.lastname,
            work_positions.name
            FROM roles 
            INNER JOIN user_roles ON user_roles.roleId = roles.id 
            INNER JOIN users ON users.id = user_roles.userId 
            LEFT JOIN user_work_positions ON users.id = user_work_positions.userId 
            LEFT JOIN work_positions ON work_positions.id = user_work_positions.work_positionId 
            WHERE users.id = @UserId;";
            MySqlCommand command = new MySqlCommand(query, ConnectionService.MySqlConnection);
            command.CommandTimeout = 60;
            command.Parameters.AddWithValue("@UserId", userId);
            string? role = null;
            try
            {
                ConnectionService.MySqlConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    role = reader.GetString(0);
                    CurrentUser = new UserModel()
                    {
                        Id = userId,
                        Role = new RoleModel()
                        {
                            Id = reader.GetInt32(1),
                            Name = reader.GetString(0)
                        },
                        Username = reader.GetString(2),
                        Firstname = reader.GetString(3),
                        Lastname = reader.GetString(4),
                    };
                    if (!reader.IsDBNull(5)) {
                        UserWorkPosition = new UserWorkPositionModel()
                        {
                            User = CurrentUser,
                            WorkPosition = new WorkPositionModel()
                            {
                                Name = reader.GetString(5),
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                ConnectionService.MySqlConnection.Close();
            }
            return role;
        }
    }
}
