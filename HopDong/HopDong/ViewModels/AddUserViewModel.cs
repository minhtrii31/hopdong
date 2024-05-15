using DocumentFormat.OpenXml.Wordprocessing;
using HopDong.Models;
using HopDong.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace HopDong.ViewModels
{
    public class AddUserViewModel : BindableBase, INavigationAware
    {
        private string _userID;
        public string UserID
        {
            get { return _userID; }
            set { SetProperty(ref _userID, value); }
        }
        private DateTime _createAt;
        public DateTime CreateAt
        {
            get { return _createAt; }
            set { SetProperty(ref _createAt, value); }
        }
        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { SetProperty(ref _fullName, value); }
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }
        private string _selectedUserRole;
        public string SelectedUserRole
        {
            get { return _selectedUserRole; }
            set { SetProperty(ref _selectedUserRole, value); }
        }

        private List<string> _userRoles;
        public List<string> UserRoles
        {
            get { return _userRoles; }
            set { SetProperty(ref _userRoles, value); }
        }

        public DelegateCommand AddUserCommand { get; }
        public DelegateCommand CancelCommand { get; }
        private readonly IRegionManager _regionManager;
        public AddUserViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            AddUserCommand = new DelegateCommand(ExecuteAddUserCommand);
            CancelCommand = new DelegateCommand(ExecuteCancelCommand);

            InitializeUserRoles();

            SelectedUserRole = UserRoles.FirstOrDefault();
            UserID = Guid.NewGuid().ToString();
            CreateAt = DateTime.Now;
        }
        private void InitializeUserRoles()
        {
            UserRoles = new List<string>
            {
                "User",
                "Admin",
            };
        }

        private void ExecuteAddUserCommand()
        {
            User newUser = new()
            {
                UserId = UserID,
                UserFullName = FullName,
                Username = Username,
                UserPassword = Password,
                UserRole = SelectedUserRole,
                CreateAt = CreateAt,
            };

            using (var context = new HopDongDbContext())
            {
                context.Users.Add(newUser);
                context.SaveChanges();
            }
            _regionManager.RequestNavigate("ContentRegion", "UserPage");
        }

        private void ExecuteCancelCommand()
        {
            _regionManager.RequestNavigate("ContentRegion", "UserPage");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
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
