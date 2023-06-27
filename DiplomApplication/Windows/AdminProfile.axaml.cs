using Avalonia.Controls;
using Avalonia.Media;
using DiplomApplication.Classes;
using DiplomApplication.DataBase;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls.Primitives;

namespace DiplomApplication.Windows
{
    public partial class AdminProfile : Window
    {
        int switchValue = 6;
        public AdminProfile()
        {
            InitializeComponent();
            searchTextBox.KeyUp += SearchTextBoxKeyUp;
            sortComboBox.SelectionChanged += SortComboBoxSelectionChanged;
            usersButton.Click += UsersButtonClick;
            menuItemsButton.Click += MenuItemsButtonClick;
            AddButton.Click += AddButtonClick;
            editAdminDataButton.Click += EditAdminDataButtonClick;
            LoadData();
        }

        private async void EditAdminDataButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            DataBaseClass.switchValue = 1;
            UsersProfile usersProfile = new UsersProfile();
            await usersProfile.ShowDialog(this);
            if (DataBaseClass.isLogined == false)
                this.Close();
            LoadData();
        }

        private async void AddButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            AddMenuUserWindow addMenuUserWindow = new AddMenuUserWindow();
            await addMenuUserWindow.ShowDialog(this);
            LoadData(); 
        }

        private void MenuItemsButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            switchValue = 2;
            toBottomComboBoxItem.IsVisible = false;
            toTopComboBoxItem.IsVisible = false;
            LoadData();
        }

        private void UsersButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            switchValue = 1;
            toBottomComboBoxItem.IsVisible = false;
            toTopComboBoxItem.IsVisible = false;
            LoadData();
        }
        private async void DeleteButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            int deletedUser = (int)(sender as Button).Tag;
            if(switchValue == 1)
            {

                if (deletedUser != DataBaseClass.currentUserId && deletedUser != 44) 
                {
                    var warning = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.OkCancel,
                        ContentTitle = "Предупреждение",
                        ContentMessage = $"Вы уверены, что хотите удалить этого пользователя"
                    }).ShowDialog(this);

                    if (warning == MessageBox.Avalonia.Enums.ButtonResult.Ok)
                    {

                        DataBaseClass.DataBase.Users.Remove(DataBaseClass.DataBase.Users.Find(deletedUser));
                        DataBaseClass.DataBase.SaveChanges();

                        var doneValue = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
                        {
                            ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.OkCancel,
                            ContentTitle = "Предупреждение",
                            ContentMessage = $"Успешно!"
                        }).ShowDialog(this);
                    }
                }
                else
                {
                    var warning = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Предупреждение",
                        ContentMessage = "Этого пользователя нельзя удалить",
                        Width = 300
                    }).ShowDialog(this);
                }
                
            }
            if (switchValue == 2)
            {

                var warning = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.OkCancel,
                    ContentTitle = "Предупреждение",
                    ContentMessage = $"Вы уверены, что хотите удалить этот продукт"
                }).ShowDialog(this);

                if (warning == MessageBox.Avalonia.Enums.ButtonResult.Ok)
                {

                    DataBaseClass.DataBase.Menus.Remove(DataBaseClass.DataBase.Menus.Find(deletedUser));
                    DataBaseClass.DataBase.SaveChanges();

                    var doneValue = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.OkCancel,
                        ContentTitle = "Предупреждение",
                        ContentMessage = $"Успешно!"
                    }).ShowDialog(this);
                }
            }
            LoadData();
        }
        private async void EditButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            int editedUser = (int)(sender as Button).Tag;
            EditUsersDataWindow editUsersDataWindow = new EditUsersDataWindow(editedUser);
            await editUsersDataWindow.ShowDialog(this);
            LoadData();
        }

        private void SortComboBoxSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            LoadData();
        }

        private void SearchTextBoxKeyUp(object? sender, Avalonia.Input.KeyEventArgs e)
        {
            LoadData();
        }

        public void LoadData()
        {
            int selectedSort = sortComboBox.SelectedIndex;
            string searchedText = searchTextBox.Text;
            
            if (switchValue == 1)
            {
                DataBaseClass.switchValue = 1;
                List<User> users = new List<User>();
                users = DataBaseClass.DataBase.Users.ToList();

                //сщртировка
                switch (selectedSort)
                {
                    default:
                        users = users.ToList();
                        break;

                    case 1:
                        users = users.OrderBy(f => f.LastName)
                            .ToList();
                        break;
                    case 2:
                        users = users.OrderByDescending(f => f.LastName)
                            .ToList();
                        break;
                }
                //поиск
                if (searchedText != null)
                    users = users.Where(f => f.FirstName
                    .ToLower()
                    .Contains(searchedText
                    .ToLower()) || f.LastName
                    .ToLower()
                    .Contains(searchedText
                    .ToLower()) || f.Patronymic
                    .ToLower()
                    .Contains(searchedText
                    .ToLower()) || f.Email
                    .ToLower()
                    .Contains(searchedText
                    .ToLower()))
                    .ToList();

                commonListBox.Items = users.Select(f => new
                {
                    FIOTitle = $"{f.LastName} {f.FirstName} {f.Patronymic}",
                    PhoneComposition = $"Телефон: {f.Phone}",
                    EmailCalories = $"Email: {f.Email}",
                    DateOfBirthDescription = $"Дата рождения: {f.DateOfBirth}",
                    BonusCountEnergyValue = $"Количество бонусов: {f.BonusCount}",
                    PasswordPFC = $"Пароль: {f.Password}",
                    RoleCost = $"Роль: {f.UserRole}",
                    CommonPhoto = f.UserPhoto,
                    ID = f.UserId
                });
            }


                if (switchValue == 2)
                {
                DataBaseClass.switchValue = 2;
                toBottomComboBoxItem.IsVisible = true;
                toTopComboBoxItem.IsVisible = true;
                List<DataBase.Menu> menu = new List<DataBase.Menu>();
                menu = DataBaseClass.DataBase.Menus.ToList();

                //сщртировка
                switch (selectedSort)
                {
                    default:
                        menu = menu.ToList();
                        break;

                    case 1:
                        menu = menu.OrderBy(f => f.Title)
                            .ToList();
                        break;
                    case 2:
                        menu = menu.OrderByDescending(f => f.Title)
                            .ToList();
                        break;
                    case 3:
                        menu = menu.OrderBy(f => f.Cost)
                            .ToList();
                        break;
                    case 4:
                        menu = menu.OrderByDescending(f => f.Cost)
                            .ToList();
                        break;
                }
                //поиск


                if (searchedText != null)
                    menu = menu.Where(f => f.Title
                    .ToLower()
                    .Contains(searchedText
                    .ToLower()) || f.Description
                    .ToLower()
                    .Contains(searchedText
                    .ToLower()) || f.Composition
                    .ToLower()
                    .Contains(searchedText
                    .ToLower()))
                    .ToList();

                commonListBox.Items = menu.Select(f => new
                {
                    FIOTitle = $"Наименование {f.Title}",
                    PhoneComposition = $"Состав: {f.Composition}",
                    EmailCalories = $"Калорийность: {f.Calories}",
                    DateOfBirthDescription = $"Описание: {f.Description}",
                    BonusCountEnergyValue = $"Енергетическая ценность: {f.Energyvalue}",
                    PasswordPFC = $"Белки: {f.Proteins}, Жиры: {f.Fats}, Углеводы: {f.Carbohydrates}",
                    RoleCost = $"Стоимость: {f.Cost}",
                    CommonPhoto = f.MenuItemImage,
                    ID = f.MenuItemId
                });
            }

        }
    }
}
