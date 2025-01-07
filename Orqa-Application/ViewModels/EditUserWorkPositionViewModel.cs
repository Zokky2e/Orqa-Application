using Orqa_Application.Models;
using Orqa_Application.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orqa_Application.ViewModels
{
    public partial class EditUserWorkPositionViewModel : ReactiveViewModelBase
    {
        public ObservableCollection<WorkPositionModel> WorkPositionList { get; } = new ObservableCollection<WorkPositionModel>();

        private WorkPositionModel? _selectedWorkPosition;
        public WorkPositionModel? SelectedWorkPosition
        {
            get => _selectedWorkPosition;
            set => this.RaiseAndSetIfChanged(ref _selectedWorkPosition, value);
        }

        public ObservableCollection<UserWorkPositionModel> AvailableUserList { get; } = new ObservableCollection<UserWorkPositionModel>();

        private UserWorkPositionModel? _selectedUser;
        public UserWorkPositionModel? SelectedUser
        {
            get => _selectedUser;
            set => this.RaiseAndSetIfChanged(ref _selectedUser, value);
        }
        public EditUserWorkPositionViewModel(
            ObservableCollection<WorkPositionModel> workPositions,
            ObservableCollection<UserWorkPositionModel> availabelUsers)
        {
            WorkPositionList = workPositions;
            AvailableUserList = availabelUsers;
        }
    }
}
