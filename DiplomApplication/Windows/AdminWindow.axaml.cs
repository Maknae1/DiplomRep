using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using DiplomApplication.Classes;
using DiplomApplication.Windows;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiplomApplication
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }
        int currentuser;
        public AdminWindow(int userid)
        {
            InitializeComponent();
            currentuser = userid;
            DataBaseClass.currentUserId = currentuser;
            LoadMenuItemTypeData();
            LoadClicks();
            LoadImages();
            LoadUserData();
        }

        public void LoadClicks()
        {
            userOffice.Click += UserOfficeClick;
            menuButton.Click += MenuButtonClick;
            basketButton.Click += BasketButtonClick;
            spendButton.Click += SpendButtonClick;
            feedBackButton.Click += FeedBackButtonClick;
            workButton.Click += WorkButtonClick;
            promotionButton.Click += PromotionButtonClick;
        }

        private async void PromotionButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            PromotionWindow promotionWindow = new PromotionWindow();
            await promotionWindow.ShowDialog(this);
        }

        private async void WorkButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            QuestionairsWindow questionaire = new QuestionairsWindow();
            await questionaire.ShowDialog(this);
        }

        private async void FeedBackButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            FeedBackWindow feedBackWindow = new FeedBackWindow();
            await feedBackWindow.ShowDialog(this);

        }

        private async void SpendButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            SpendingBonusesWindow spendingBonusesWindow = new SpendingBonusesWindow(currentuser);
            await spendingBonusesWindow.ShowDialog(this);
            LoadUserData();
        }

        public void LoadImages()
        {
            string logo = "./MenuItems/logo.png";
            logoImage.Source = new Bitmap(logo);
            ad1Image.Source = new Bitmap("./Ad/ad1.png");
            ad2Image.Source = new Bitmap("./Ad/ad2.png");
            ad3Image.Source = new Bitmap("./Ad/ad3.png");
        }

        private async void BasketButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            BasketWindow basketWindow = new BasketWindow();
            await basketWindow.ShowDialog(this);
            LoadUserData();
        }

        private void AddToBasketButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            int menuItemId = (int)(sender as Button).Tag;
            var selectedItem = DataBaseClass.DataBase.Menus.Where(f => f.MenuItemId == menuItemId)
                .First();

            int orderCount = DataBaseClass.DataBase.Orders.Where(f => f.UserId == currentuser).Count();
            float UsersSale = SaleForUser(orderCount);

            DataBaseClass.basketClassList.Add(new BasketClass
            {
                ID = menuItemId,
                Title = selectedItem.Title,
                Cost = Math.Truncate((int)selectedItem.Cost * UsersSale),
                MenuItemImage = selectedItem.Image != null ? new Bitmap(selectedItem.Image
                .Trim()) : new Bitmap(".\\MenuItems\\logo.png"),
                Tag = DataBaseClass.basketClassList.Count + 1
            });
        }

        public void LoadUserData()
        {
            var username = DataBaseClass.DataBase.Users.Where(f => f.UserId == currentuser).First();
            usernameTextBlock.Text = $"{username.FirstName} {username.Patronymic}";
            userphotoImage.Source = username.UserPhoto;
            bonusCount.Text = $"Количество бонусов: {username.BonusCount}";
        }

        private async void MenuButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            adScrollViewer.IsVisible = false;
            menuStackPanel.IsVisible = true;


            var menu = DataBaseClass.DataBase.Menus.ToList();
            int orderCount = DataBaseClass.DataBase.Orders.Where(f => f.UserId == currentuser).Count();
            float UsersSale = SaleForUser(orderCount);

            menuListBox.Items = menu.Select(f => new
            {
                Id = f.MenuItemId,
                f.Title,
                f.Description,
                f.MenuItemImage,
                Cost = $"{Math.Truncate((int)f.Cost * UsersSale)} руб.",
                Color = UsersSale > 0 ? Brushes.Red : Brushes.Black
            }).ToList();

            var result = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.OkCancel,
                ContentTitle = "Подтверждение",
                ContentMessage = $"{UsersSale}"
            }).ShowDialog(this);
        }

        private async void UserOfficeClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            AdminProfile adminProfile = new AdminProfile();
            await adminProfile.ShowDialog(this);
            if (DataBaseClass.isLogined == false)
                this.Close();
            LoadUserData();
        }

        public void LoadMenuItemTypeData()
        {
            int itemCount = DataBaseClass.DataBase.MenuItemTypes.Count();
            for (int i = 1; i <= itemCount; i++)
            {
                var currentType = DataBaseClass.DataBase.MenuItemTypes.Where(f => f.MenuItemTypeId == i)
                    .First();
                Button button = new Button();
                button.Tag = i;
                button.Content = currentType.Title;
                button.Margin = new Thickness(10);
                button.Foreground = Brushes.Gray;
                button.FontSize = 12;
                button.Background = Brushes.Transparent;
                button.FontFamily = "Comic Sans MS";
                button.FontWeight = FontWeight.DemiBold;
                button.Click += ButtonClick;
                menuItemTypeStackPanel.Children.Add(button);
            }
        }

        private void ButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            int currentItemType = (int)(sender as Button).Tag;

            List<DataBase.Menu> menu = new List<DataBase.Menu>();
            menu = DataBaseClass.DataBase.Menus.ToList();

            switch (currentItemType)
            {
                case 1:
                    menu = menu.Where(f => f.MenuItemTypeId == 1)
                        .ToList();
                    break;
                case 2:
                    menu = menu.Where(f => f.MenuItemTypeId == 2)
                        .ToList();
                    break;
                case 3:
                    menu = menu.Where(f => f.MenuItemTypeId == 3)
                        .ToList();
                    break;
                case 4:
                    menu = menu.Where(f => f.MenuItemTypeId == 4)
                        .ToList();
                    break;
                case 5:
                    menu = menu.Where(f => f.MenuItemTypeId == 5)
                        .ToList();
                    break;
                case 6:
                    menu = menu.Where(f => f.MenuItemTypeId == 6)
                        .ToList();
                    break;
                case 7:
                    menu = menu.Where(f => f.MenuItemTypeId == 7)
                        .ToList();
                    break;
                default:
                    menu.ToList();
                    break;
            }

            int orderCount = DataBaseClass.DataBase.Orders.Where(f => f.UserId == currentuser).Count();
            float UsersSale = SaleForUser(orderCount);

            menuListBox.Items = menu.Select(f => new
            {
                Id = f.MenuItemId,
                f.Title,
                f.Description,
                f.MenuItemImage,
                Cost = $"{Math.Truncate((int)f.Cost * UsersSale)} руб.",
                Color = UsersSale > 1 ? Brushes.Red : Brushes.Black
            }).ToList();
        }

        public float SaleForUser(int orderCount)
        {
            if (orderCount < 3) return 1;
            if (orderCount >= 3 && orderCount < 5) return 0.8f;
            if (orderCount >= 5 && orderCount < 7) return 0.7f;
            if (orderCount >= 7 && orderCount < 10) return 0.65f;
            if (orderCount >= 10) return 0.5f;
            return 0;
        }
    }
}
