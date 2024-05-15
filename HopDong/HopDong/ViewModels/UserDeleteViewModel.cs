using DocumentFormat.OpenXml.Math;
using HopDong.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HopDong.ViewModels
{
    public class UserDeleteViewModel : BindableBase, INavigationAware
    {
        private User _selectedUser;
        public User SelectedUser
        {
            get { return _selectedUser; }
            set { SetProperty(ref _selectedUser, value); }
        }

        public DelegateCommand<User> CancelCommand { get; private set; }
        public DelegateCommand<User> AcceptCommand { get; private set; }
        private readonly IRegionManager _regionManager;
        public UserDeleteViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            CancelCommand = new DelegateCommand<User>(Cancel);
            AcceptCommand = new DelegateCommand<User>(Accept);
        }
        private void Accept(User user)
        {
            using (var context = new HopDongDbContext())
            {
                var userInDB = context.Users.FirstOrDefault(cd => cd.UserId == SelectedUser.UserId);
                if (userInDB != null)
                {
                    context.Users.Remove(userInDB);
                    context.SaveChanges();
                }
            }
            _regionManager.RequestNavigate("ContentRegion", "UserPage");
        }

        private void Cancel(User user)
        {
            _regionManager.RequestNavigate("ContentRegion", "UserPage");
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.TryGetValue("SelectedUser", out User selectedUser))
            {
                SelectedUser = selectedUser;
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
