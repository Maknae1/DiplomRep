using Avalonia.Controls;
using DiplomApplication.Classes;
using DiplomApplication.DataBase;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia;

namespace DiplomApplication.Windows
{
    public partial class AddMenuUserWindow : Window
    {
        public User user;
        public DataBase.Menu menu;
        private readonly FileDialogFilter dialogFilter = new FileDialogFilter()
        {
            Extensions = new List<string>() { "png", "jpg", "jpeg" },
            Name = "Image Files"
        };

        public AddMenuUserWindow()
        {
            InitializeComponent();
            switch (DataBaseClass.switchValue)
            {
                case 1:
                    labelTextBlock.Text = "Данные пользовaтеля";
                    user = new User();
                    DataContext = user;
                    userStackPanel.IsVisible = true;
                    menuStackPanel.IsVisible = false;
                    LoadUserFields();
                    break;
                case 2:
                    labelTextBlock.Text = "Позиция меню";
                    menu = new DataBase.Menu();
                    DataContext = menu;
                    userStackPanel.IsVisible = false;
                    menuStackPanel.IsVisible = true;
                    LoadMenuFields();
                    break;
                default:
                    labelTextBlock.Text = "Пользователь";
                    user = new User();
                    DataContext = user;
                    DataBaseClass.switchValue = 1;
                    userRoleComboBox.IsVisible = false;
                    userStackPanel.IsVisible = true;
                    menuStackPanel.IsVisible = false;
                    menuEditButton.Content = "Сохранить";
                    LoadUserFields();
                    break;
            }
            var userRole = DataBaseClass.DataBase.Roles.ToList();
            userRoleComboBox.Items = userRole.Select(f => new { f.Title });

        }
        public void LoadUserFields()
        {            
            editButton.Click += AddButtonClick;
            deleteimageButton.Click += DeleteimageButtonClick;
            fileDialogButton.Click += FileDialogButtonClick;
        }
        public void LoadMenuFields()
        {            
            menuEditButton.Click += AddButtonClick;
            menudeleteimageButton.Click += DeleteimageButtonClick;
            menufileDialogButton.Click += FileDialogButtonClick;
        }

        private void DeleteimageButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {

            if (DataBaseClass.switchValue == 1)
            {
                user.Userimage = null;
                imagePathTextBlock.Text = null;
            }
            if (DataBaseClass.switchValue == 2)
            {
                menu.Image = null;
                menuImagePathTextBlock.Text = null;
            }

        }

        private async void FileDialogButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filters?.Add(dialogFilter);

            string[]? result = await dialog.ShowAsync(this);
            if (result == null)
                return;

            string imageName = Path.GetFileName(result[0]);
            File.Copy(result[0], $"./Users/{imageName}", true);
            if (DataBaseClass.switchValue == 1)
            {
                user.Userimage = $".\\Users\\{imageName}";
                imagePathTextBlock.Text = user.Userimage;
            }
            if (DataBaseClass.switchValue == 2)
            {
                menu.Image = $".\\MenuItems\\{imageName}";
                menuImagePathTextBlock.Text = menu.Image;
            }
        }

        private async void AddButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (DataBaseClass.switchValue == 1)
            {
                try
                {
                    user.DateOfBirth = DateOnly.ParseExact(dateOfBirthTextBox.Text, "dd-MM-yyyy", null);
                    user.RoleId = 1;
                    DataBaseClass.DataBase.Users.Add(user);
                    DataBaseClass.DataBase.SaveChanges();
                    editButton.Content = "Успешно";
                    if (DataBaseClass.isLogined == false)
                    {
                        DataBaseClass.WasGuestRegistered = true;
                        EnterOrRegistrationButtonClickClass.EnterButtonClick();
                        this.Close();
                    }                        
                }
                catch (Exception ex)
                {
                    var warning = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Подтверждение",
                        ContentMessage = "Проверьте правильность заполнения всех полей"
                    }).ShowDialog(this);
                }
            }
            if (DataBaseClass.switchValue == 2)
            {
                try
                {
                    DataBaseClass.DataBase.Menus.Add(menu);
                    DataBaseClass.DataBase.SaveChanges();
                    editButton.Content = "Успешно";
                }
                catch (Exception ex)
                {
                    var warning = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Подтверждение",
                        ContentMessage = "Проверьте правильность заполнения всех полей"
                    }).ShowDialog(this);
                }
            }

        }
    }
}
