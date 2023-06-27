using Avalonia.Controls;
using DiplomApplication.Classes;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia;
using System.Collections.Generic;
using System.Linq;
using DiplomApplication.DataBase;
using System;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp;

namespace DiplomApplication
{
    public partial class BasketWindow : Window
    {
        int selectedPBO;
        public Order order;
        Random random = new Random();
        public BasketWindow()
        {
            InitializeComponent();
            orderButton.Click += OrderButtonClick;
            pboAddressComboBox.SelectionChanged += PboAddressComboBoxSelectionChanged;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            order = new Order();
            DataContext = order;
            LoadData();
        }

        private void PboAddressComboBoxSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            var selectedaddress = DataBaseClass.DataBase.Pbos.Where(f => f.PboId == (pboAddressComboBox.SelectedIndex + 8))
                .First();
            selectedPBO = selectedaddress.PboId;
        }

        private async void OrderButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (DataBaseClass.basketClassList.Count() > 0)
            {
                //сохранение заказа в базу данных
                try
                {
                    order.Cost = (int)DataBaseClass.basketClassList.Select(f => f.Cost).Sum();
                    order.PboId = selectedPBO;
                    order.OrderDate = DateOnly.FromDateTime(DateTime.Now.Date);
                    order.UserId = DataBaseClass.isLogined != false ? DataBaseClass.currentUserId : 44;
                    order.SecurityCode = RandomString(10);
                    DataBaseClass.DataBase.Orders.Add(order);
                    DataBaseClass.DataBase.SaveChanges();

                    //генерация PDF-файла
                    PdfDocument document = new PdfDocument();
                    PdfPage page = document.AddPage();
                    page.Size = PageSize.A5;
                    XGraphics toWriteValue = XGraphics.FromPdfPage(page);

                    double posX = 0;
                    double posY = 0;

                    XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
                    XFont titleFont = new XFont("Comic Sans MS", 22, XFontStyle.Bold, options);
                    XFont mainContentFont = new XFont("Comic Sans MS", 16, XFontStyle.Regular, options);


                    toWriteValue.DrawString($"Чек от {DateTime.Now}"
                        , titleFont
                        , XBrushes.Black
                        , new XRect(0, 0, page.Width, page.Height)
                        , XStringFormats.TopCenter);

                    int currentYpositionValues = 100;

                    //генерация дополнительных страниц в PDF-документе
                    if (DataBaseClass.basketClassList.Count <= 15)
                    {
                        for (int i = 0; i < DataBaseClass.basketClassList.Count; i++)
                        {
                            var basketItem = DataBaseClass.basketClassList[i];

                            toWriteValue.DrawString($"{basketItem.Title} ................ {basketItem.Cost}"
                                , mainContentFont
                                , XBrushes.Black
                                , new XPoint(100, currentYpositionValues));

                            currentYpositionValues += 25;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            var basketItem = DataBaseClass.basketClassList[i + 1];

                            toWriteValue.DrawString($"{basketItem.Title} ................ {basketItem.Cost}"
                                , mainContentFont
                                , XBrushes.Black
                                , new XPoint(100, currentYpositionValues));

                            currentYpositionValues += 25;
                            DataBaseClass.basketClassList.Remove(DataBaseClass.basketClassList[i]);
                        }
                        page = document.AddPage();
                        page.Size = PageSize.A5;
                        toWriteValue = XGraphics.FromPdfPage(page);
                        currentYpositionValues = 33;

                        for (int i = 0; i < DataBaseClass.basketClassList.Count; i++)
                        {
                            if (i != 0 && i % 30 == 0)
                            {
                                page = document.AddPage();
                                page.Size = PageSize.A5;
                                toWriteValue = XGraphics.FromPdfPage(page);
                                currentYpositionValues = 33;
                            }

                            var basketItem = DataBaseClass.basketClassList[i];

                            toWriteValue.DrawString($"{basketItem.Title} ................ {basketItem.Cost}"
                                , mainContentFont
                                , XBrushes.Black
                                , new XPoint(100, currentYpositionValues));

                            currentYpositionValues += 25;
                        }
                    }
                    currentYpositionValues += 25;

                    toWriteValue.DrawString($"Итого: {DataBaseClass.basketClassList.Select(f => f.Cost).Sum()}"
                                , titleFont
                                , XBrushes.Black
                                , new XPoint(10, currentYpositionValues + 10));

                    toWriteValue.DrawString("Спасибо за покупку!"
                                , titleFont
                                , XBrushes.Black
                                , new XRect(0, 0, page.Width, page.Height)
                                , XStringFormats.BottomCenter);

                    string fileName = $"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}.pdf";
                    document.Save(fileName);

                    //начисление бонусов
                    if (DataBaseClass.isLogined != false)
                    {
                        var currentUser = DataBaseClass.DataBase.Users.Where(f => f.UserId == DataBaseClass.currentUserId)
                        .First();
                        currentUser.BonusCount += ((uint)DataBaseClass.basketClassList.Select(f => f.Cost).Sum()) / 100 * 5;
                        DataBaseClass.DataBase.Users.Update(currentUser);
                        DataBaseClass.DataBase.SaveChanges();
                    }


                    //Уведомление пользователя об успешности операции
                    var confirm = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Подтверждение",
                        ContentMessage = DataBaseClass.isLogined != false ? $"Чек сохранен на ваш компьютер, начислено бонусов: {(DataBaseClass.basketClassList.Select(f => f.Cost).Sum()) / 100 * 5}"
                        : "Спасибо за заказа! Чек сохранен на ваш компьютер"
                    }).ShowDialog(this);

                    this.Close();
                }
                catch
                {
                    var warning = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Подтверждение",
                        ContentMessage = "Проверьте правильность заполнения всех полей"
                    }).ShowDialog(this);
                }
            }
            else
            {
                var warning = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Предупреждение",
                    ContentMessage = "Корзина пустая"
                }).ShowDialog(this);
            }

            
        }

        private void DeleteBasketItemClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            int selectedItem = (int)(sender as Button).Tag;
            DataBaseClass.basketClassList.Remove(DataBaseClass.basketClassList.Where(f => f.Tag == selectedItem).First());
            LoadData();
        }

        public void LoadData()
        {
            basketListBox.Items = DataBaseClass.basketClassList.Select(f => new
            {
                f.ID,
                f.Title,
                Count = $"X{f.Count}",
                f.Cost,
                f.MenuItemImage,
                f.Tag
            });
            itemCountTextBlock.Text = $"Всего позиций в заказе: {DataBaseClass.basketClassList.Count()}";
            commonCostTextBlock.Text = $"Итого: {DataBaseClass.basketClassList.Select(f => f.Cost).Sum()}";
            pboAddressComboBox.Items = DataBaseClass.DataBase.Pbos.Select(f => new { f.Address });
        }

        //генерация уникального кода
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
