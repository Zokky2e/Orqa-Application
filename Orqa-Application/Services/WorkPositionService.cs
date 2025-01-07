using Avalonia.Win32.Interop.Automation;
using DynamicData;
using MySql.Data.MySqlClient;
using Orqa_Application.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Orqa_Application.Services
{
    public class WorkPositionService
    {
        public ConnectionService ConnectionService { get; set; }
        public WorkPositionService(ConnectionService connectionService)
        {
            ConnectionService = connectionService;
        }
        public void AddWorkPosition(WorkPositionModel workPosition)
        {
            if (workPosition == null)
            {
                return;
            }
            MySqlTransaction transaction = null;
            string addWorkPositionQuery = "INSERT INTO `work_positions` (`id`, `name`, `description`) VALUES(NULL, @Name, @Description)";
            try
            {
                ConnectionService.MySqlConnection.Open();
                transaction = ConnectionService.MySqlConnection.BeginTransaction();
                using (MySqlCommand command = ConnectionService.MySqlConnection.CreateCommand())
                {
                    command.Connection = ConnectionService.MySqlConnection; 
                    command.CommandText = addWorkPositionQuery;
                    command.Parameters.AddWithValue("@Name", workPosition.Name);
                    command.Parameters.AddWithValue("@Description", workPosition.Description);
                    command.CommandTimeout = 60;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                throw new Exception("Error adding work position", ex);
            }
            finally
            {
                ConnectionService.MySqlConnection.Close();
            }
        }

        public void UpadateWorkPosition(UserWorkPositionModel userWorkPosition)
        {
            if (userWorkPosition.WorkPosition == null || string.IsNullOrWhiteSpace(userWorkPosition.WorkPosition.Name))
            {
                DeleteUserWorkPosition(userWorkPosition.User.Id);
                return;
            }
            else
            {
                // Check if an entry for the user already exists
                bool userWorkPositionExists = CheckUserWorkPositionExists(userWorkPosition.User.Id);

                if (userWorkPositionExists)
                {
                    // Update the existing user work position entry
                    UpdateUserWorkPosition(userWorkPosition);
                }
                else
                {
                    // Insert a new user work position entry
                    InsertUserWorkPosition(userWorkPosition);
                }
            }
        }
        private void DeleteUserWorkPosition(int userId)
        {
            MySqlTransaction transaction = null;
            string query = @"DELETE
            FROM
                user_work_positions
            WHERE
                userId = @UserId";
            try
            {
                ConnectionService.MySqlConnection.Open();
                transaction = ConnectionService.MySqlConnection.BeginTransaction();
                using (MySqlCommand command = ConnectionService.MySqlConnection.CreateCommand())
                {
                    command.Connection = ConnectionService.MySqlConnection;
                    command.CommandText = query;
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.CommandTimeout = 60;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                throw new Exception("Error adding work position", ex);
            }
            finally
            {
                ConnectionService.MySqlConnection.Close();
            }
        }

        private bool CheckUserWorkPositionExists(int userId)
        {
            bool hasUser = false;
            string query = "select exists (select 1 from user_work_positions where userId = @UserId limit 1) num";
            MySqlCommand command = new MySqlCommand(query, ConnectionService.MySqlConnection);
            try
            {
                ConnectionService.MySqlConnection.Open();
                command.Parameters.AddWithValue("@UserId", userId);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    hasUser = reader.GetInt32("num") > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding work position", ex);
            }
            finally
            {
                ConnectionService.MySqlConnection.Close();
            }
            return hasUser;
        }

        private void UpdateUserWorkPosition(UserWorkPositionModel userWorkPositionModel)
        {
            string query = @"update user_work_positions
                set
                user_work_positions.work_positionId = @WorkPositionId,
                user_work_positions.productName = @ProductName,
                user_work_positions.dateCreated = @DateCreated
                WHERE
                user_work_positions.userId = @UserId;";
            MySqlTransaction transaction = null;
            try
            {
                ConnectionService.MySqlConnection.Open();
                transaction = ConnectionService.MySqlConnection.BeginTransaction();
                using (MySqlCommand command = ConnectionService.MySqlConnection.CreateCommand())
                {
                    command.Connection = ConnectionService.MySqlConnection;
                    command.CommandText = query;
                    command.Parameters.AddWithValue("@WorkPositionId", userWorkPositionModel.WorkPosition.Id);
                    command.Parameters.AddWithValue("@ProductName", userWorkPositionModel.ProductName);
                    command.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                    command.Parameters.AddWithValue("@UserId", userWorkPositionModel.User.Id);
                    command.CommandTimeout = 60;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                throw new Exception("Error adding work position", ex);
            }
            finally
            {
                ConnectionService.MySqlConnection.Close();
            }
        }
        private void InsertUserWorkPosition(UserWorkPositionModel userWorkPositionModel)
        {
            string query = @"insert into user_work_positions
                (`id`, `userId`, `work_positionId`, `productName`, `dateCreated`) values (NULL, @UserId, @WorkPositionId, @ProductName, @DateCreated);";
            MySqlTransaction transaction = null;
            try
            {
                ConnectionService.MySqlConnection.Open();
                transaction = ConnectionService.MySqlConnection.BeginTransaction();
                using (MySqlCommand command = ConnectionService.MySqlConnection.CreateCommand())
                {
                    command.Connection = ConnectionService.MySqlConnection;
                    command.CommandText = query;
                    command.Parameters.AddWithValue("@WorkPositionId", userWorkPositionModel.WorkPosition.Id);
                    command.Parameters.AddWithValue("@ProductName", userWorkPositionModel.ProductName);
                    command.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                    command.Parameters.AddWithValue("@UserId", userWorkPositionModel.User.Id);
                    command.CommandTimeout = 60;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                throw new Exception("Error adding work position", ex);
            }
            finally
            {
                ConnectionService.MySqlConnection.Close();
            }
        }

        public ObservableCollection<WorkPositionModel> GetWorkPositions()
        {
            var workPositions = new ObservableCollection<WorkPositionModel>();

            string query = "select * from `work_positions`";

            MySqlCommand command = new MySqlCommand(query, ConnectionService.MySqlConnection);
            command.CommandTimeout = 60;
            try
            {
                ConnectionService.MySqlConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var workPosition = new WorkPositionModel()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2)
                        };
                        workPositions.Add(workPosition);
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
            return workPositions;
        }

        public ObservableCollection<UserWorkPositionModel> GetUserWorkPositions()
        {
            var workpositions = new ObservableCollection<UserWorkPositionModel>();

            string query = @"SELECT 
            user_work_positions.id, 
            work_positions.id,
            work_positions.name,
            work_positions.description,
            user_work_positions.productName,
            user_work_positions.dateCreated,
            users.id,
            users.username,
            users.firstname,
            users.lastname
            FROM user_work_positions 
            INNER JOIN users ON user_work_positions.userId = users.id 
            INNER JOIN work_positions ON work_positions.id = user_work_positions.work_positionId";
            MySqlCommand command = new MySqlCommand(query, ConnectionService.MySqlConnection);
            command.CommandTimeout = 60;
            try
            {
                ConnectionService.MySqlConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while(reader.Read()) {
                        var userWorkPosition = new UserWorkPositionModel()
                        {
                            Id = reader.GetInt32(0),
                            WorkPosition = new WorkPositionModel()
                            {
                                Id = reader.GetInt32(1),
                                Name = reader.GetString(2),
                                Description = reader.GetString(3)
                            },
                            ProductName = reader.GetString(4),
                            DateCreated = reader.GetDateTime(5),
                            User = new UserModel()
                            {
                                Id = reader.GetInt32(6),
                                Username = reader.GetString(7),
                                Firstname = reader.GetString(8),
                                Lastname = reader.GetString(9),
                            },
                        };
                        workpositions.Add(userWorkPosition);
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
            return workpositions;
        }

    }
}
