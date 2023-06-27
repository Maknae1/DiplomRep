using Avalonia.Controls;
using DiplomApplication.Classes;
using DiplomApplication.DataBase;
using JetBrains.Annotations;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia;
using System.Linq;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using System;

namespace DiplomApplication.Windows
{
    public partial class FeedBackWindow : Window
    {
        public FeedBackWindow()
        {
            InitializeComponent();
            LoadData();
            sendButton.Click += SendButtonClick;
            EnterButton.Click += EnterButtonClick;
            registrationButton.Click += RegistrationButtonClick;
            CheckAutorization();
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
            if (DataBaseClass.isLogined == false)
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

        private async void SendButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            bool isSecurityCodeCorrect = false;

            foreach (var item in DataBaseClass.DataBase.Orders)
            {
                if (item.SecurityCode == securiryCodeTextBox.Text)
                {
                    isSecurityCodeCorrect = true;
                    break;
                }
                else
                {
                    isSecurityCodeCorrect= false;
                }
            }
            if (isSecurityCodeCorrect == true)
            {
                try
                {
                    DataBaseClass.DataBase.FeedBacks.Add(new FeedBack
                    {
                        FeedbackRateId = rateComboBox.SelectedIndex + 1,
                        PboId = pboAddressComboBox.SelectedIndex + 8,
                        FeedBackText = feedbackTextBox.Text,
                    });
                    DataBaseClass.DataBase.SaveChanges();
                    var warning = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Предупреждение",
                        ContentMessage = $"Успешно"
                    }).ShowDialog(this);
            }
                    catch (Exception ex)
            {
                var warning = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Предупреждение",
                    ContentMessage = $"Проверьте правильность заполнения всех полей"
                }).ShowDialog(this);
            }
        }
            else
            {
                var warning = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Предупреждение",
                    ContentMessage = $"Проверьте код из чека"
                }).ShowDialog(this);
            }

        }

        public void LoadData()
        {
            pboAddressComboBox.Items = DataBaseClass.DataBase.Pbos.Select(f => new { f.Address });
            rateComboBox.Items = DataBaseClass.DataBase.FeedBackRates.Select(f => new { f.Title });

        }
    }
}
