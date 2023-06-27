using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using DiplomApplication.Classes;
using DiplomApplication.DataBase;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;

namespace DiplomApplication
{
    public partial class EditUsersDataWindow : Window
    {
        public User user;
        public DataBase.Menu menu;
        private readonly FileDialogFilter dialogFilter = new FileDialogFilter()
        {
            Extensions = new List<string>() { "png", "jpg", "jpeg" },
            Name = "Image Files"
        };
        
        public EditUsersDataWindow()
        {
            InitializeComponent();
            
            
        }
        public EditUsersDataWindow(int currentUser)
        {
            InitializeComponent();
            switch (DataBaseClass.switchValue)
            {
                case 1:
                    user = new User();
                    DataContext = user;
                    break;
                case 2:
                    menu = new DataBase.Menu();
                    DataContext = menu;
                    break;
                default:
                    user = new User();
                    DataContext = user;
                    break;
            }

            if (DataBaseClass.switchValue == 1)
            {
                user = DataBaseClass.DataBase.Users.Find(currentUser);
                userStackPanel.IsVisible = true;
                menuStackPanel.IsVisible = false;
                LoadUserFields();
            }
            if (DataBaseClass.switchValue == 2)
            {
                userStackPanel.IsVisible = false;
                menuStackPanel.IsVisible = true;
                menu = DataBaseClass.DataBase.Menus.Find(currentUser);
                LoadMenuFields();
            }

            
        }
        public void LoadUserFields()
        {
            DataContext = user;
            dateOfBirthTextBox.Text = user.DateOfBirth.ToString();
            imagePathTextBlock.Text = user.Userimage;
            editButton.Click += EditButtonClick;
            deleteimageButton.Click += DeleteimageButtonClick;
            fileDialogButton.Click += FileDialogButtonClick;
        }
        public void LoadMenuFields()
        {
            DataContext = menu;
            menuImagePathTextBlock.Text = menu.Image;
            menuEditButton.Click += EditButtonClick;
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

            string imageName = System.IO.Path.GetFileName(result[0]);
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

        private async void EditButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            //string oldPassword = user.Password;
            if (DataBaseClass.switchValue == 1)
            {
                try
                {
                    user.DateOfBirth = DateOnly.Parse(dateOfBirthTextBox.Text);
                    DataBaseClass.DataBase.Users.Update(user);
                    DataBaseClass.DataBase.SaveChanges();
                    editButton.Content = "Успешно";
                }
                catch (Exception ex)
                {
                    var warning = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams 
                    { 
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok, 
                        ContentTitle = "Подтверждение", 
                        ContentMessage = "Проверьте правильность заполнения всех полей" })
                        .ShowDialog(this);
                }
            }
            if (DataBaseClass.switchValue == 2)
            {
                try
                {
                    
                    DataBaseClass.DataBase.Menus.Update(menu);
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
