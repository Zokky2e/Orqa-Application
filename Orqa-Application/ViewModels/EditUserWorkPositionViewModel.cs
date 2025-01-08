using CommunityToolkit.Mvvm.Input;
using Orqa_Application.Models;
using Orqa_Application.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace Orqa_Application.ViewModels
{
    public partial class EditUserWorkPositionViewModel : ReactiveViewModelBase
    {
        private readonly Action _onWorkPositionUpdated;
        public IRelayCommand UpdateWorkPositionCommand { get; }
        public WorkPositionService _workPositionService; 

        public ObservableCollection<WorkPositionModel> WorkPositionList { get; } = new ObservableCollection<WorkPositionModel>();

        private WorkPositionModel? _selectedWorkPosition;
        public WorkPositionModel? SelectedWorkPosition
        {
            get => _selectedWorkPosition;
            set => this.RaiseAndSetIfChanged(ref _selectedWorkPosition, value);
        }

        public ObservableCollection<UserModel> AvailableUserList { get; } = new ObservableCollection<UserModel>();

        private UserModel? _selectedUser;
        public UserModel? SelectedUser
        {
            get => _selectedUser;
            set => this.RaiseAndSetIfChanged(ref _selectedUser, value);
        }
        public EditUserWorkPositionViewModel(
            WorkPositionService workPositionService,
            ObservableCollection<UserModel> availabelUsers,
            Action onWorkPositionUpdated)
        {
            _workPositionService = workPositionService;
            WorkPositionList = _workPositionService.GetWorkPositions();
            WorkPositionList = new ObservableCollection<WorkPositionModel>(
                new[] { new WorkPositionModel { Name = "No work" } }.Concat(WorkPositionList)
            );
            AvailableUserList = availabelUsers;
            this.WhenAnyValue(vm => vm.SelectedUser)
            .Subscribe(user =>
            {
                if (user?.UserWorkPosition != null && user.UserWorkPosition.WorkPosition.Name != string.Empty)
                {
                    SelectedWorkPosition = WorkPositionList.FirstOrDefault(wp => wp.Name == user.UserWorkPosition.WorkPosition.Name);
                }
                else
                {
                    SelectedWorkPosition = WorkPositionList.FirstOrDefault(wp => wp.Name == "No work");
                }
            });
            UpdateWorkPositionCommand = new RelayCommand(OnUpdateWorkPosition);
            _onWorkPositionUpdated = onWorkPositionUpdated;
        }

        private void OnUpdateWorkPosition() 
        {
            if (SelectedUser == null)
            {
                return;
            }

            if (SelectedWorkPosition?.Name == "No work")
            {
                SelectedUser.UserWorkPosition = new UserWorkPositionModel();
                SelectedUser.UserWorkPosition.WorkPosition = new WorkPositionModel();
            }
            else
            {
                SelectedUser.UserWorkPosition.WorkPosition = SelectedWorkPosition;
            }
            _workPositionService.UpadateWorkPosition(SelectedUser.UserWorkPosition);
            _onWorkPositionUpdated?.Invoke();
        }

    }
}
