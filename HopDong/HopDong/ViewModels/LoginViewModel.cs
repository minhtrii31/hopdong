using HopDong.Models;
using HopDong.Views;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace HopDong.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private string _username;
        private string _password;

        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public DelegateCommand LoginCommand { get; private set; }

        public LoginViewModel()
        {
            LoginCommand = new DelegateCommand(Login);
        }

        private void CloseWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is Views.Login)
                {
                    window.Close();
                    break;
                }
            }
        }

        private void Login()
        {
            User user;
            using (var context = new HopDongDbContext())
            {
                user = context.Users.FirstOrDefault(u => u.Username == Username && u.UserPassword == Password);
            }
            if (user != null)
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is BaseView)
                    {
                        window.Show();
                        CloseWindow();
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Username hoặc password không đúng. Vui lòng thử lại.", "Lỗi Đăng Nhập", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
