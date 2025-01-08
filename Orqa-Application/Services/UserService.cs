using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using MySqlX.XDevAPI.Common;
using Orqa_Application.Data;
using Orqa_Application.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection.PortableExecutable;
using System.Transactions;
using Tmds.DBus.Protocol;

namespace Orqa_Application.Services
{
    public class UserService
    {
        public UserModel CurrentUser { get; set; }

        public UserWorkPositionModel? UserWorkPosition { get; set; }

        private readonly WorkstationDbContext _dbContext;

        public UserService(WorkstationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public ObservableCollection<UserWorkPositionModel> GetAvailableUsers()
        {
            var userList = _dbContext.Users
                            .Where(u => u.RoleId != 1)
                            .Select(u => u.UserWorkPosition ?? new UserWorkPositionModel())
                            .ToList();
            return new ObservableCollection<UserWorkPositionModel>(userList);
        }

        public void AddUser(UserModel user, string password)
        {
            if (user == null)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(password) && password.Length > 8) {
                var transaction = _dbContext.Database.BeginTransaction();
                user.Password = BCrypt.Net.BCrypt.HashPassword(password);
                try
                {
                    _dbContext.Add(user);
                    _dbContext.SaveChanges();
                    var userId = user.Id;
                    var role = _dbContext.Roles.FirstOrDefault(r => r.Id == 2);
                    if (role != null)
                    {
                        user.Role = role;
                        _dbContext.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            else
            {
                return;
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
            string? role = null;

            try
            {
                var user = _dbContext.Users
                    .Where(u => u.Id == userId)
                    .Include(u => u.Role)
                    .Include(u => u.UserWorkPosition)
                    .FirstOrDefault();

                if (user != null && user.Role != null)
                {
                    role = user.Role.Name;

                    CurrentUser = new UserModel()
                    {
                        Id = userId,
                        Username = user.Username,
                        Firstname = user.Firstname,
                        Lastname = user.Lastname,
                        Role = user.Role
                    };

                    if (user.UserWorkPosition != null)
                    {
                        UserWorkPosition = new UserWorkPositionModel()
                        {
                            User = CurrentUser,
                            WorkPosition = user.UserWorkPosition.WorkPosition
                        };
                    }
                    else
                    {
                        UserWorkPosition = new UserWorkPositionModel();
                    }
                }
            }
            catch (Exception e)
            {
            }
            return role;
        }

        public int LoginUser(string username, string password)
        {
            try
            {
                var user = _dbContext.Users
                    .Where(u => u.Username == username)
                    .FirstOrDefault();
                if (user != null) 
                {
                    string storedHashedPassword = user.Password;
                    if (BCrypt.Net.BCrypt.Verify(password, storedHashedPassword))
                    {
                        return user.Id;
                    }
                }
            }
            catch (Exception)
            {
            }

            return 0;
        }
    }
}
