using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MySql.Data.MySqlClient;
using Orqa_Application.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Orqa_Application.ViewModels
{
    public partial class UserCardControlViewModel : ReactiveViewModelBase
    {
        private UserModel _user;
        public UserModel User
        {
            get => _user;
            set => this.RaiseAndSetIfChanged(ref _user, value);
        }
        private bool _hasWorkPosition;
        public bool HasWorkPosition
        {
            get => _hasWorkPosition;
            set => this.RaiseAndSetIfChanged(ref _hasWorkPosition, value);
        }
        private UserWorkPositionModel? _userWorkPosition;
        public UserWorkPositionModel? UserWorkPosition
        {
            get => _userWorkPosition;
            set => this.RaiseAndSetIfChanged(ref _userWorkPosition, value);
        }

        public UserCardControlViewModel(UserModel user, UserWorkPositionModel? userWorkPosition = null)
        {
            User = user;
            UserWorkPosition = userWorkPosition; 
            HasWorkPosition = userWorkPosition != null && !string.IsNullOrWhiteSpace(userWorkPosition.WorkPosition?.Name);

        }
    }
}
