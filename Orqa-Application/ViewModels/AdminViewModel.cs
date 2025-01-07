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
using System.Reactive;

namespace Orqa_Application.ViewModels
{
    public partial class AdminViewModel : ReactiveViewModelBase
    {
        public IRelayCommand LogoutCommand { get; }
        public IRelayCommand ReloadCommand { get; }
        public IRelayCommand AddNewUserCommand { get; }
        public IRelayCommand AddNewWPCommand { get; }

        public UserService _userService;
        public NavigationService _navigationService;
        public WorkPositionService _workPositionService;

        private UserModel _newUser = new UserModel();
        public UserModel NewUser
        {
            get => _newUser;
            set => this.RaiseAndSetIfChanged(ref _newUser, value);
        }
        private WorkPositionModel _newWorkPosition = new WorkPositionModel();
        public WorkPositionModel NewWorkPosition
        {
            get => _newWorkPosition;
            set => this.RaiseAndSetIfChanged(ref _newWorkPosition, value);
        }
        private string _newUserPassword;
        public string NewUserPassword
        {
            get => _newUserPassword;
            set => this.RaiseAndSetIfChanged(ref _newUserPassword, value); 
        }
        public ObservableCollection<UserWorkPositionModel> UserWorkPositionList { get; } = new ObservableCollection<UserWorkPositionModel>();
        public UserCardControlViewModel AdminUserCardViewModel { get; }
        public EditUserWorkPositionViewModel EditUserWorkPositionViewModel { get; }
        public AdminViewModel(
            NavigationService navigationService, 
            UserService userService, 
            WorkPositionService workPositionService)
        {
            _navigationService = navigationService;
            _userService = userService;
            _workPositionService = workPositionService;
            AdminUserCardViewModel = new UserCardControlViewModel(_userService.CurrentUser, _userService.UserWorkPosition);
            EditUserWorkPositionViewModel = new EditUserWorkPositionViewModel(_workPositionService.GetWorkPositions(), _userService.GetAvailableUsers());
            
        }

        private void OnReloadWorkPositions()
        {
            GetWorkPositions();
        }
        private void OnAddNewUserCommand()
        {
            if (string.IsNullOrWhiteSpace(NewUser.Username) ||
            string.IsNullOrWhiteSpace(NewUser.Firstname) ||
            string.IsNullOrWhiteSpace(NewUser.Lastname) ||
            string.IsNullOrWhiteSpace(NewUserPassword) || NewUserPassword.Length < 8)
            {
                return;
            }
            try
            {
                _userService.AddUser(NewUser, NewUserPassword);
                NewUser = new UserModel();
                NewUserPassword = string.Empty;
            }
            catch (Exception ex)
            {
            }
        }

        private void OnAddNewWPCommand()
        {
            if (string.IsNullOrWhiteSpace(NewWorkPosition.Name) ||
            string.IsNullOrWhiteSpace(NewWorkPosition.Description))
            {
                return;
            }
            try
            {
                _workPositionService.AddWorkPosition(NewWorkPosition);

                NewWorkPosition = new WorkPositionModel();
            }
            catch (Exception ex)
            {
            }
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
