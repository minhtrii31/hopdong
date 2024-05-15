using HopDong.Models;
using HopDong.Views;
using Microsoft.VisualBasic.ApplicationServices;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using User = HopDong.Models.User;

namespace HopDong.ViewModels
{
    public class UserPageViewModel : BindableBase, INavigationAware
    {
        private ObservableCollection<User> _userCollection;
        public ObservableCollection<User> UserCollection
        {
            get { return _userCollection; }
            set { SetProperty(ref _userCollection, value); }
        }

        public ICommand DetailCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand AddCommand { get; }

        private readonly IRegionManager _regionManager;

        public UserPageViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            DetailCommand = new DelegateCommand<User>(ExecuteDetailCommand);
            EditCommand = new DelegateCommand<User>(ExecuteEditCommand);
            DeleteCommand = new DelegateCommand<User>(ExecuteDeleteCommand);
            AddCommand = new DelegateCommand(ExecuteAddCommand);
        }

        private void ExecuteAddCommand()
        {
            _regionManager.RequestNavigate("ContentRegion", "AddUser");
        }

        private void RefreshUserCollection()
        {
            using (var context = new HopDongDbContext())
            {
                UserCollection = new ObservableCollection<User>(context.Users.ToList());
            }
        }

        private void ExecuteDetailCommand(User user)
        {
            var parameters = new NavigationParameters();
            parameters.Add("SelectedUser", user);
            _regionManager.RequestNavigate("ContentRegion", "UserDetail", parameters);
        }

        private void ExecuteEditCommand(User user)
        {
            var parameters = new NavigationParameters();
            parameters.Add("SelectedUser", user);
            _regionManager.RequestNavigate("ContentRegion", "UserUpdate", parameters);
        }

        private void ExecuteDeleteCommand(User user)
        {
            var parameters = new NavigationParameters();
            parameters.Add("SelectedUser", user);
            _regionManager.RequestNavigate("ContentRegion", "UserDelete", parameters);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            using (var context = new HopDongDbContext())
            {
                UserCollection = new ObservableCollection<User>(context.Users.ToList());
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
