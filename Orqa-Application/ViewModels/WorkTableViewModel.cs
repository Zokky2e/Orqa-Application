using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Orqa_Application.Models;
using Orqa_Application.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Orqa_Application.ViewModels
{
    public partial class WorkTableViewModel : ReactiveObject
    {
        public IRelayCommand ReloadCommand { get; }

        public WorkPositionService _workPositionService;

        private ObservableCollection<UserWorkPositionModel> _userWorkPositionList;
        public ObservableCollection<UserWorkPositionModel> UserWorkPositionList
        {
            get => _userWorkPositionList;
            set => this.RaiseAndSetIfChanged(ref _userWorkPositionList, value);
        }
        public WorkTableViewModel(IServiceProvider services)
        {
            _workPositionService = services.GetRequiredService<WorkPositionService>();
            UserWorkPositionList = new ObservableCollection<UserWorkPositionModel>();
            ReloadCommand = new RelayCommand(OnReloadWorkPositions);
            this.WhenAnyValue(e => e.UserWorkPositionList)
                .Subscribe(e =>
                {
                    OnReloadWorkPositions();
                });
        }

        private void OnReloadWorkPositions()
        {
            GetWorkPositions();
        }

        public void GetWorkPositions()
        {
            var userWorkPositionModels = _workPositionService.GetUserWorkPositions();
            UserWorkPositionList.Clear();
            foreach (var item in userWorkPositionModels)
            {
                UserWorkPositionList.Add(item);
            }
            this.RaiseAndSetIfChanged(ref _userWorkPositionList, UserWorkPositionList);
        }
    }
}
