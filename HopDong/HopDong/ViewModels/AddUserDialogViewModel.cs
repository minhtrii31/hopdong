using HopDong.Models;
using HopDong.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace HopDong.ViewModels
{
    public class AddUserDialogViewModel : BindableBase
    {
        private User _newUser;
        public User NewUser
        {
            get { return _newUser; }
            set { SetProperty(ref _newUser, value); }
        }

        private string _userID;
        public string UserID
        {
            get { return _userID; }
            set { SetProperty(ref _userID, value); }
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

        private DateTime _createAt;
        public DateTime CreateAt
        {
            get { return _createAt; }
            set { SetProperty(ref _createAt, value); }
        }

        private List<string> _userRoles;
        public List<string> UserRole
        {
            get { return _userRoles; }
            set { SetProperty(ref _userRoles, value); }
        }

        public DelegateCommand AddUserCommand { get; }

        public DelegateCommand CancelCommand { get; }

        public AddUserDialogViewModel()
        {
            AddUserCommand = new DelegateCommand(ExecuteAddUserCommand);

            CancelCommand = new DelegateCommand(ExecuteCancelCommand);

            UserRole = new List<string>
            {
                "User",
                "Admin",
            };

            SelectedUserRole = UserRole.FirstOrDefault();
            UserID = Guid.NewGuid().ToString();
            CreateAt = DateTime.Now;

            NewUser = new User();
        }

        private void ExecuteAddUserCommand()
        {
            try
            {
                User newUser = new User
                {
                    UserId = UserID,
                    UserFullName = FullName,
                    Username = Username,
                    UserPassword = Password,
                    UserRole = SelectedUserRole 
                };

                using (var context = new HopDongDbContext())
                {
                    context.Users.Add(newUser);
                    context.SaveChanges();
                }

                CloseDialog(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi thêm người dùng: {ex.Message}");
            }
        }

        private void ExecuteCancelCommand()
        {
            CloseDialog(false);
        }

        private void CloseDialog(bool? dialogResult)
        {
            var window = App.Current.Windows.OfType<Window>().FirstOrDefault(w => w is AddUserDialog);
            if (window != null)
            {
                window.Close();
            }
        }
    }
}
