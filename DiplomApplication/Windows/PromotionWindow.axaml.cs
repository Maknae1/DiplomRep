using Avalonia.Controls;
using DiplomApplication.Classes;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia;
using System;
using System.Linq;
using Avalonia.Media.Imaging;

namespace DiplomApplication.Windows
{
    public partial class PromotionWindow : Window
    {
        public PromotionWindow()
        {
            InitializeComponent();
            CheckAutorization();
            EnterButton.Click += EnterButtonClick;
            registrationButton.Click += RegistrationButtonClick;
        }

        private async void RegistrationButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {

            DataBaseClass.WasGuestRegistered = false;
            AddMenuUserWindow addMenuUserWindow = new AddMenuUserWindow();
            await addMenuUserWindow.ShowDialog(this);
            if (DataBaseClass.WasGuestRegistered == true)
                this.Close();
        }

        private void EnterButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            DataBaseClass.WasGuestRegistered = true;
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        public void CheckAutorization()
        {
            if(DataBaseClass.isLogined == false)
            {
                noAutorizationStackPanel.IsVisible = true;
                logginesUserStackPanel.IsVisible = false;
            }
            else
            {
                noAutorizationStackPanel.IsVisible = false;
                logginesUserStackPanel.IsVisible = true;
            }
        }
        private async void GetPromotionButttonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            string promoId = (sender as Button).Tag.ToString();
            switch (promoId) 
            {
                case "1":
                    if(DataBaseClass.basketClassList.Select(f => f.Cost).Sum() >= 685)
                    {
                        DataBaseClass.basketClassList.Add(new BasketClass
                        {
                            ID = DataBaseClass.basketClassList.Count + 1,
                            Title = "Картофель фри по акции",
                            Cost = 1,
                            MenuItemImage = new Bitmap(".\\MenuItems\\logo.png"),
                            Tag = DataBaseClass.basketClassList.Count + 1
                        });
                    }
                    else
                    {
                        var result = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
                        {
                            ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                            ContentTitle = "Предупреждение",
                            ContentMessage = "Сумма заказа не достигла необходимой"
                        }).ShowDialog(this);
                    }
                break;

                case "2":
                    DataBaseClass.basketClassList.Add(new BasketClass
                    {
                        ID = DataBaseClass.basketClassList.Count + 1,
                        Title = "2 Биг Xut по цене одного",
                        Cost = 1,
                        MenuItemImage = new Bitmap(".\\MenuItems\\logo.png"),
                        Tag = DataBaseClass.basketClassList.Count + 1
                    });
                    break;
            }
        }
    }
}
