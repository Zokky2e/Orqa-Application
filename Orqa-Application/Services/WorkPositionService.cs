using Avalonia.Win32.Interop.Automation;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Orqa_Application.Data;
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
        private readonly WorkstationDbContext _dbContext;

        public WorkPositionService(WorkstationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void AddWorkPosition(WorkPositionModel workPosition)
        {
            if (workPosition == null)
            {
                return;
            }
            var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.Add(workPosition);
                _dbContext.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
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
            var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var userWorkPosition = _dbContext.UserWorkPositions.Where(uwp => uwp.UserId == userId).FirstOrDefault();
                if (userWorkPosition != null)
                {
                    _dbContext.Remove(userWorkPosition);
                    _dbContext.SaveChanges();
                    transaction.Commit();
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
        }

        private bool CheckUserWorkPositionExists(int userId)
        {
            try
            {
                bool hasUser = _dbContext.UserWorkPositions
                    .Any(uwp => uwp.UserId == userId);

                return hasUser;
            }
            catch (Exception ex)
            {
                // Handle exception, log if necessary
                throw new Exception("Error checking work position existence", ex);
            }
        }

        private void UpdateUserWorkPosition(UserWorkPositionModel userWorkPositionModel)
        {
            try
            {
                var existingUserWorkPosition = _dbContext.UserWorkPositions
                    .FirstOrDefault(uwp => uwp.UserId == userWorkPositionModel.User.Id);

                if (existingUserWorkPosition != null)
                {
                    existingUserWorkPosition.WorkPositionId = userWorkPositionModel.WorkPosition.Id;
                    if (existingUserWorkPosition.ProductName != userWorkPositionModel.ProductName)
                    {
                        existingUserWorkPosition.ProductName = userWorkPositionModel.ProductName;
                        existingUserWorkPosition.DateCreated = DateTime.Now;
                    }
                    _dbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("User work position not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating work position", ex);
            }
        }
        private void InsertUserWorkPosition(UserWorkPositionModel userWorkPositionModel)
        {
            try
            {
                // Create a new UserWorkPosition entity
                var newUserWorkPosition = new UserWorkPositionModel
                {
                    UserId = userWorkPositionModel.User.Id,
                    WorkPositionId = userWorkPositionModel.WorkPosition.Id,
                    ProductName = userWorkPositionModel.ProductName,
                    DateCreated = DateTime.Now
                };
                _dbContext.UserWorkPositions.Add(newUserWorkPosition);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting work position", ex);
            }
        }

        public ObservableCollection<WorkPositionModel> GetWorkPositions()
        {
            var workPositionsList = _dbContext.WorkPositions
                            .ToList();
            return new ObservableCollection<WorkPositionModel>(workPositionsList);
        }

        public ObservableCollection<UserWorkPositionModel> GetUserWorkPositions()
        {
            try
            {
                var workPositions = _dbContext.UserWorkPositions
                    .Include(uwp => uwp.WorkPosition)
                    .Include(uwp => uwp.User)
                    .Select(uwp => new UserWorkPositionModel
                    {
                        Id = uwp.Id,
                        ProductName = uwp.ProductName,
                        DateCreated = uwp.DateCreated,
                        WorkPosition = new WorkPositionModel
                        {
                            Id = uwp.WorkPosition.Id,
                            Name = uwp.WorkPosition.Name,
                            Description = uwp.WorkPosition.Description
                        },
                        User = new UserModel
                        {
                            Id = uwp.User.Id,
                            Username = uwp.User.Username,
                            Firstname = uwp.User.Firstname,
                            Lastname = uwp.User.Lastname
                        }
                    })
                    .ToList();
                return new ObservableCollection<UserWorkPositionModel>(workPositions);
            }
            catch (Exception)
            {
            }
            return new ObservableCollection<UserWorkPositionModel>();
        }

    }
}
