using Avalonia.Controls;
using DiplomApplication.Classes;
using DiplomApplication.DataBase;
using System.Collections.Generic;
using System.Linq;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using Avalonia.Media.Imaging;

namespace DiplomApplication.Windows
{
    public partial class SpendingBonusesWindow : Window
    {
        int currentUserId;
        int usersBonusCount;
        public User user;
        public SpendingBonusesWindow()
        {
            InitializeComponent();
        }
        public SpendingBonusesWindow(int userId)
        {
            InitializeComponent();            
            currentUserId = userId;
            user = new User();
            DataContext = user;
            LoadData();

        }
        public void LoadData()
        {
            List<DataBase.Menu> menu = new List<DataBase.Menu>();
            menu = DataBaseClass.DataBase.Menus.OrderBy(f => f.Cost)
            .ToList();

            var currentuser = DataBaseClass.DataBase.Users.Where(f => f.UserId == currentUserId).First();
            usersBonusCount = (int)currentuser.BonusCount;



            menuItemsListBox.Items = menu.Select(f => new
            {
                f.Title,
                Id = f.MenuItemId,
                f.MenuItemImage,
                BonusInfo = $"{usersBonusCount} из {(int)f.Cost}",
                IsExchangeButtonVisible = usersBonusCount >= f.Cost ? true : false
            });

        }
        private async void SpendBonusesButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var result = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.OkCancel,
                ContentTitle = "Подтверждение",
                ContentMessage = "Вы уверены, что хотите обменять бонусы?"
            }).ShowDialog(this);

            if(result == MessageBox.Avalonia.Enums.ButtonResult.Ok)
            {
                int menuItemId = (int)(sender as Button).Tag;
                var selectedItem = DataBaseClass.DataBase.Menus.Where(f => f.MenuItemId == menuItemId)
                    .First();

                DataBaseClass.basketClassList.Add(new BasketClass
                {
                    ID = menuItemId,
                    Title = selectedItem.Title,
                    Cost = 0,
                    MenuItemImage = selectedItem.Image != null ? new Bitmap(selectedItem.Image
                    .Trim()) : new Bitmap(".\\MenuItems\\logo.png"),
                    Tag = DataBaseClass.basketClassList.Count + 1
                });
                user = DataBaseClass.DataBase.Users.Find(currentUserId);
                user.BonusCount = (uint)(usersBonusCount - selectedItem.Cost);
                DataBaseClass.DataBase.Users.Update(user);
                DataBaseClass.DataBase.SaveChanges();

                var confirm = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Подтверждение",
                    ContentMessage = "Обмен произведен. Продукция добавлена в корзину"
                }).ShowDialog(this);
            }
        }   
    }
}
