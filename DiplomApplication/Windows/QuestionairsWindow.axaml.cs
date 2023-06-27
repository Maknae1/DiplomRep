using Avalonia.Controls;
using DiplomApplication.Classes;
using DiplomApplication.DataBase;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia;
using System;
using System.Linq;

namespace DiplomApplication.Windows
{
    public partial class QuestionairsWindow : Window
    {
        public Questionaire questionaire;
        public QuestionairsWindow()
        {
            InitializeComponent();
            LoadData();
            questionaire = new Questionaire();
            DataContext = questionaire;
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
            questionaire.DateOfBirth = DateOnly.Parse(dateOfBirthTextBox.Text);
            if(questionaire.DateOfBirth > new DateOnly(2005,01, 01) || questionaire.DateOfBirth < new DateOnly(1973, 01, 01))
            {
                var warning = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Предупреждение",
                    ContentMessage = $"К сожалению, вы не подходите по возрасту"
                }).ShowDialog(this);
            }
            else
            {
                try
                {
                    questionaire.PboId = pboAddressComboBox.SelectedIndex + 8;
                    questionaire.Questionairstatus = "Ожидает";
                    DataBaseClass.DataBase.Questionaires.Add(questionaire);
                    DataBaseClass.DataBase.SaveChanges();

                    var warning = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Подтверждение",
                        ContentMessage = $"Успешно"
                    }).ShowDialog(this);
                    this.Close();
                }
                catch(Exception ex)
                {
                    var warning = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Подтверждение",
                        ContentMessage = $"{ex.Message}"
                    }).ShowDialog(this);
                }
            }
        }

        public void LoadData()
        {
            pboAddressComboBox.Items = DataBaseClass.DataBase.Pbos.Select(f => new { f.Address });

        }
    }
}
