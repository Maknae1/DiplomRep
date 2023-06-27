using Avalonia.Controls;
using System.Linq;
using System;
using DiplomApplication.Classes;
using DiplomApplication.Windows;

namespace DiplomApplication
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            loginButton.Click += LoginButtonClick;
            guestButton.Click += GuestButtonClick;
        }

        private void GuestButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            int id = 44;
            UserWindow userWindow = new UserWindow(id);
            userWindow.Show();
            this.Close();
        }

        private void LoginButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            string userlogin = loginTextBox.Text.Trim();
            string userPassword = passwordTextBox.Text;

            try
            {
                var islogin = DataBaseClass.DataBase.Users.Where(f => f.Email == userlogin).First();
                if (islogin.Password == userPassword)
                {
                    DataBaseClass.isLogined = true;
                    DataBaseClass.currentUserId = islogin.UserId;
                    int id = islogin.UserId;
                    switch(islogin.RoleId)
                    {
                        case 1:
                            UserWindow userWindow = new UserWindow(id);
                            userWindow.Show();
                            this.Close();
                        break;

                        case 2:
                            AdminWindow adminWindow = new AdminWindow(id);
                            adminWindow.Show();
                            this.Close();
                        break;

                        case 3:
                            var currentdir = DataBaseClass.DataBase.Directors.Where(f => f.Email == islogin.Email).First();
                            DataBaseClass.currentDir = currentdir.DirectorId;
                            DirectorsWindows directorsWindows = new DirectorsWindows(id);
                            directorsWindows.Show();
                            this.Close();   
                            break;
                    }
                }
                else
                {
                    loginButton.Content = "check password";
                }
            }
            catch (Exception ex)
            {
                loginButton.Content = $"check login";
            }
        }
    }
}
