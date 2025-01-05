using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using MySql.Data.MySqlClient;
using Orqa_Application.Models;
using Orqa_Application.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace Orqa_Application.ViewModels
{
    public partial class AdminViewModel : ReactiveObject
    {
        public IRelayCommand LogoutCommand { get; }
        public IRelayCommand ReloadCommand { get; }

        public UserService _userService;
        public NavigationService _navigationService;
        public WorkPositionService _workPositionService;
        public UserModel User {  get; set; }
        public UserWorkPositionModel? UserWorkPosition { get; set; } = null;
        public bool HasWorkPosition { get; set; } = false;
        public ObservableCollection<UserWorkPositionModel> UserWorkPositionList { get; } = new ObservableCollection<UserWorkPositionModel>();
        public AdminViewModel(
            NavigationService navigationService, 
            UserService userService, 
            WorkPositionService workPositionService)
        {
            _navigationService = navigationService;
            _userService = userService;
            _workPositionService = workPositionService;
            User = _userService.CurrentUser;
            UserWorkPosition = _userService.UserWorkPosition;
            HasWorkPosition = _userService.UserWorkPosition != null;
            LogoutCommand = new RelayCommand(OnLogout);
            ReloadCommand = new RelayCommand(OnReloadWorkPositions);
        }

        private void OnReloadWorkPositions()
        {
            GetWorkPositions();
        }
        private void OnLogout()
        {
            _userService.ClearSession();
            _navigationService.RedirectLoggedInUser(0);
        }

        public void GetWorkPositions()
        {
            var userWorkPositionModels = _workPositionService.GetUserWorkPositions();
            var testData = new ObservableCollection<UserWorkPositionModel>()
            {
                new UserWorkPositionModel()
                {
                    Id = 1,
                    User = new UserModel() {Firstname="Pavo", Lastname="Pavic"},
                    WorkPosition = new WorkPositionModel() {Name="Pavina Firma"},
                    ProductName = "Test",
                    DateCreated = DateTime.Now,
                },
                new UserWorkPositionModel()
                {
                    Id = 2,
                    User = new UserModel() {Firstname="Pavo", Lastname="Pavic"},
                    WorkPosition = new WorkPositionModel() {Name="Pavina Firma"},
                    ProductName = "Test",
                    DateCreated = DateTime.Now,
                },
                new UserWorkPositionModel()
                {
                    Id = 3,
                    User = new UserModel() {Firstname="Pavo", Lastname="Pavic"},
                    WorkPosition = new WorkPositionModel() {Name="Pavina Firma"},
                    ProductName = "Test",
                    DateCreated = DateTime.Now,
                }
            };
            userWorkPositionModels.AddRange(testData);
            UserWorkPositionList.Clear();
            foreach (var item in userWorkPositionModels)
            {
                UserWorkPositionList.Add(item);
            }
        }
    }
}
