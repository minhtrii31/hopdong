using HopDong.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HopDong.ViewModels
{
    public class UserUpdateViewModel : BindableBase, INavigationAware
    {
        private User _selectedUser;
        public User SelectedUser
        {
            get { return _selectedUser; }
            set { SetProperty(ref _selectedUser, value); }
        }


        public DelegateCommand CancelCommand { get; }

        public DelegateCommand SaveCommand { get; }
        private readonly IRegionManager _regionManager;
        public UserUpdateViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            CancelCommand = new DelegateCommand(ExecuteCancelCommand);
            SaveCommand = new DelegateCommand(ExecuteSaveCommand);
        }
   
        private void ExecuteCancelCommand()
        {
            _regionManager.RequestNavigate("ContentRegion", "UserPage");
        }
        private void ExecuteSaveCommand()
        {
            using (var context = new HopDongDbContext())
            {
                if (context.Users.Any(c => c.UserId == SelectedUser.UserId))
                {
                    var userToUpdate = context.Users
                        .FirstOrDefault(c => c.UserId == SelectedUser.UserId);

                    if (userToUpdate != null)
                    {
                        userToUpdate.UserFullName = SelectedUser.UserFullName;
                        userToUpdate.Username = SelectedUser.Username;
                        userToUpdate.UserPassword = SelectedUser.UserPassword;
                        userToUpdate.UserRole = SelectedUser.UserRole;
                    }

                    context.SaveChanges();
                }
            }
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
