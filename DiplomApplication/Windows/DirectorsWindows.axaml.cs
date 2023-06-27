using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using DiplomApplication.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiplomApplication.Windows
{
    public partial class DirectorsWindows : Window
    {
        public DirectorsWindows()
        {
            InitializeComponent();
        }
        int currentuser;
        public DirectorsWindows(int userid)
        {
            InitializeComponent();
            LoadMenuItemTypeData();
            userOffice.Click += UserOfficeClick;
            menuButton.Click += MenuButtonClick;
            basketButton.Click += BasketButtonClick;
            feedBackButton.Click += FeedBackButtonClick;
            promotionButton.Click += PromotionButtonClick;
            spendButton.Click += SpendButtonClick;
            currentuser = userid;
            string logo = "./MenuItems/logo.png";
            logoImage.Source = new Bitmap(logo);
            ad1Image.Source = new Bitmap("./Ad/ad1.png");
            ad2Image.Source = new Bitmap("./Ad/ad2.png");
            ad3Image.Source = new Bitmap("./Ad/ad3.png");
            LoadUserData();
        }

        private async void SpendButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            SpendingBonusesWindow spendingBonusesWindow = new SpendingBonusesWindow(currentuser);
            await spendingBonusesWindow.ShowDialog(this);
            LoadUserData();
        }

        private async void PromotionButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            PromotionWindow promotionWindow = new PromotionWindow();
            await promotionWindow.ShowDialog(this);
            if (DataBaseClass.WasGuestRegistered == true)
                this.Close();
        }

        private async void FeedBackButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            PromotionWindow promotionWindow = new PromotionWindow();
            await promotionWindow.ShowDialog(this);
            if (DataBaseClass.WasGuestRegistered == true)
                this.Close();
        }

        private async void BasketButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            BasketWindow basketWindow = new BasketWindow();
            await basketWindow.ShowDialog(this);
        }

        private void AddToBasketButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            int menuItemId = (int)(sender as Button).Tag;
            var selectedItem = DataBaseClass.DataBase.Menus.Where(f => f.MenuItemId == menuItemId)
                .First();

            int orderCount = DataBaseClass.DataBase.Orders.Where(f => f.UserId == currentuser).Count();
            float UsersSale = SaleForUser(orderCount);
            if (selectedItem.Count == 0)
                selectedItem.Count = 1;

            var isInBasket = DataBaseClass.basketClassList.Where(f => f.ID == menuItemId).Count();
            if (isInBasket == 0)
            {
                DataBaseClass.basketClassList.Add(new BasketClass
                {
                    ID = menuItemId,
                    Title = selectedItem.Title,
                    Count = selectedItem.Count,
                    Cost = Math.Truncate((int)selectedItem.Cost * UsersSale * selectedItem.Count),
                    MenuItemImage = selectedItem.Image != null ? new Bitmap(selectedItem.Image
                    .Trim()) : new Bitmap(".\\MenuItems\\logo.png"),
                    Tag = DataBaseClass.basketClassList.Count + 1
                });
            }
            else
            {
                var selectedItemInBasket = DataBaseClass.basketClassList.Where(f => f.ID == menuItemId).First();
                selectedItemInBasket.Count++;
            }
        }

        public void LoadUserData()
        {
            var username = DataBaseClass.DataBase.Users.Where(f => f.UserId == currentuser).First();
            usernameTextBlock.Text = $"{username.FirstName} {username.Patronymic}";
            userphotoImage.Source = username.UserPhoto;
            bonusCount.Text = $"Количество бонусов: {username.BonusCount}";
        }

        private void MenuButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            adScrollViewer.IsVisible = false;
            menuStackPanel.IsVisible = true;

            List<DataBase.Menu> menu = new List<DataBase.Menu>();
            menu = DataBaseClass.DataBase.Menus.ToList();

            menuListBox.Items = menu.Select(f => new
            {
                Id = f.MenuItemId,
                f.Title,
                f.Description,
                f.MenuItemImage
            }).ToList();

        }

        private async void UserOfficeClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            DirectorProfile directorProfile = new DirectorProfile(currentuser);
            await directorProfile.ShowDialog(this);
            if (DataBaseClass.isLogined == false)
                this.Close();
            LoadUserData();
        }

        public void LoadMenuItemTypeData()
        {
            int itemCount = DataBaseClass.DataBase.MenuItemTypes.Count();
            for (int i = 1; i <= itemCount; i++)
            {
                var currentType = DataBaseClass.DataBase.MenuItemTypes.Where(f => f.MenuItemTypeId == i).First();
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
            menuListBox.Items = menu.Select(f => new
            {
                Id = f.MenuItemId,
                f.Title,
                f.Description,
                f.MenuItemImage
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
