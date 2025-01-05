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

            string addWorkPositionQuery = "INSERT INTO `work_positions` (`id`, `name`, `description`) VALUES(NULL, @Name, @Description)";
            try
            {
                ConnectionService.MySqlConnection.Open();
                MySqlCommand command = ConnectionService.MySqlConnection.CreateCommand();
                command.CommandText = addWorkPositionQuery;
                command.Parameters.AddWithValue("@Name", workPosition.Name);
                command.Parameters.AddWithValue("@Description", workPosition.Description);
                command.Connection = ConnectionService.MySqlConnection;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding work position", ex);
            }
            finally
            {
                ConnectionService.MySqlConnection.Close();
            }
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
