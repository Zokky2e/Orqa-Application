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
using Microsoft.Extensions.DependencyInjection;

namespace Orqa_Application.ViewModels
{
    public partial class AdminViewModel : ReactiveViewModelBase
    {
        public IRelayCommand LogoutCommand { get; }
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
        private string _userError;
        public string UserError
        {
            get => _userError;
            set => this.RaiseAndSetIfChanged(ref _userError, value);
        }
        private bool _hasWPError;
        public bool HasWPError
        {
            get => _hasWPError;
            set => this.RaiseAndSetIfChanged(ref _hasWPError, value);
        }

        private string _wPError;
        public string WPError
        {
            get => _wPError;
            set => this.RaiseAndSetIfChanged(ref _wPError, value);
        }
        private bool _hasUserError;
        public bool HasUserError
        {
            get => _hasUserError;
            set => this.RaiseAndSetIfChanged(ref _hasUserError, value);
        }

        public UserCardControlViewModel AdminUserCardViewModel { get; }
        public EditUserWorkPositionViewModel EditUserWorkPositionViewModel { get; }
        public WorkTableViewModel WorkTableViewModel { get; }
        public AdminViewModel(IServiceProvider services)
        {
            _navigationService = services.GetRequiredService<NavigationService>();
            _userService = services.GetRequiredService<UserService>();
            _workPositionService = services.GetRequiredService<WorkPositionService>();
            AdminUserCardViewModel = new UserCardControlViewModel(_userService.CurrentUser, _userService.UserWorkPosition);
            WorkTableViewModel = services.GetRequiredService<WorkTableViewModel>();
            EditUserWorkPositionViewModel = new EditUserWorkPositionViewModel(_workPositionService, _userService.GetAvailableUsers(), UpdateWorkPositions);

            LogoutCommand = new RelayCommand(OnLogout);
            AddNewUserCommand = new RelayCommand(OnAddNewUserCommand);
            AddNewWPCommand = new RelayCommand(OnAddNewWPCommand);
        }

        public void UpdateWorkPositions()
        {
            WorkTableViewModel.GetWorkPositions();
        }
        private void OnAddNewUserCommand()
        {
            if (string.IsNullOrWhiteSpace(NewUser.Username) ||
            string.IsNullOrWhiteSpace(NewUser.Firstname) ||
            string.IsNullOrWhiteSpace(NewUser.Lastname) ||
            string.IsNullOrWhiteSpace(NewUserPassword))
            {
                HasUserError = true;
                UserError = "Some fields are empty.";
                return;
            }
            if (NewUserPassword.Length < 8)
            {
                HasUserError = true;
                UserError = "Password too short.";
                return;
            }
            try
            {
                _userService.AddUser(NewUser, NewUserPassword);
                NewUser = new UserModel();
                NewUserPassword = string.Empty;
                EditUserWorkPositionViewModel.AvailableUserList.Clear();
                var updatedUsers = _userService.GetAvailableUsers();
                foreach (var user in updatedUsers)
                {
                    EditUserWorkPositionViewModel.AvailableUserList.Add(user);
                }
                HasUserError = false;
                UserError = string.Empty;
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
                HasWPError = true;
                WPError = "Some fields are empty.";
                return;
            }
            try
            {
                _workPositionService.AddWorkPosition(NewWorkPosition);

                WorkTableViewModel.GetWorkPositions();
                HasWPError = false;
                WPError = string.Empty;
            }
            catch (Exception ex)
            {
            }
        }
        private void OnLogout()
        {
            _userService.ClearSession();
            _navigationService.NavigateTo("login");
        }
    }
}
