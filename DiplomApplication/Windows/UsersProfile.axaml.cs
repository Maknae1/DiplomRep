using Avalonia.Controls;
using DiplomApplication.Classes;
using DiplomApplication.DataBase;
using System.Collections.Generic;
using System.Linq;

namespace DiplomApplication.Windows
{
    public partial class UsersProfile : Window
    {
        int currentUser;

        public UsersProfile()
        {
            InitializeComponent();
            currentUser = DataBaseClass.currentUserId;

            exitButton.Click += ExitButtonClick;
            editButton.Click += EditButtonClick;
            LoadData();
        }

        private async void EditButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            DataBaseClass.switchValue = 1;
            EditUsersDataWindow editUsersDataWindow = new EditUsersDataWindow(currentUser);
            await editUsersDataWindow.ShowDialog(this);
            LoadData();
        }

        private void ExitButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            DataBaseClass.isLogined = false;
            DataBaseClass.currentUserId = 0;
            DataBaseClass.basketClassList.Clear();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        public void LoadData()
        {
            List<User> userData = new List<User>();

            userData = DataBaseClass.DataBase.Users.Where(f => f.UserId == currentUser).ToList();
            userListBox.Items = userData.Select(f => new
            {
                FIO = $"{f.FirstName} {f.LastName} {f.Patronymic}",
                Email = $"Email: {f.Email}",
                f.UserPhoto,
                Phone = $"Телефон: {f.Phone}",
                Password = $"Пароль: {f.Password}",
                DateOfBirth = $"Дата рождения: {f.DateOfBirth}"
            });
        }
    }
}
