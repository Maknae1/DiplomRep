using Avalonia.Controls;
using DiplomApplication.Classes;
using DiplomApplication.DataBase;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia;
using System.Collections.Generic;
using System.Linq;
using Aspose.Cells;

namespace DiplomApplication.Windows
{
    public partial class DirectorProfile : Window
    {
        public DirectorProfile()
        {
            InitializeComponent();
            
        }

        int switchValue = 0;
        int currentPBOid;
        List<Order> ordersForXLSX;

        public DirectorProfile(int r)
        {
            InitializeComponent();
            searchTextBox.KeyUp += SearchTextBoxKeyUp;
            sortComboBox.SelectionChanged += SortComboBoxSelectionChanged;
            employeesButton.Click += UsersButtonClick;
            ordersButton.Click += OrdersButtonClick;
            questionairsButton.Click += QuestionairsButtonClick;
            editDirectorDataButton.Click += EditDirectorDataButtonClick;
            int currentdir = DataBaseClass.currentDir;
            var currentpbo = DataBaseClass.DataBase.Pbos.Where(f => f.DirectorId == currentdir).First();
            currentPBOid = currentpbo.PboId;
            downloadButton.Click += DownloadButtonClick;
            


        }

        private void DownloadButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            LoadXLS();
        }

        private void QuestionairsButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            switchValue = 3;
            ordersStackPanel.IsVisible = false;
            employeeListBox.IsVisible = false;
            questionairsListBox.IsVisible = true;
            LoadData();
        }

        private async void WelcomeButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            int questionairesId = (int)(sender as Button).Tag;
            var selectedItem = DataBaseClass.DataBase.Questionaires.First(f => f.QuestionairId == questionairesId);
            Questionaire questionaire;
            questionaire = selectedItem;
            DataContext = questionaire;
            questionaire.Questionairstatus = "Приглашен";
            DataBaseClass.DataBase.Questionaires.Update(questionaire);
            DataBaseClass.DataBase.SaveChanges();
            var confirm = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                ContentTitle = "Подтверждение",
                ContentMessage = $"Вы пригласили соискателя на работу, ему отправлено приглашение"
            }).ShowDialog(this);
            LoadData();
        }

        private async void RejectButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            int questionairesId = (int)(sender as Button).Tag;
            DataBaseClass.DataBase.Questionaires.Remove(DataBaseClass.DataBase.Questionaires.Find(questionairesId));
            DataBaseClass.DataBase.SaveChanges();
            var confirm = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                ContentTitle = "Подтверждение",
                ContentMessage = $"Вы отклонили анкету соискателя"
            }).ShowDialog(this);
            LoadData();
        }

        private async void EditDirectorDataButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            DataBaseClass.switchValue = 1;
            UsersProfile usersProfile = new UsersProfile();
            await usersProfile.ShowDialog(this);
            if (DataBaseClass.isLogined == false)
                this.Close();
            LoadData();
        }

        private void OrdersButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            switchValue = 2;
            ordersStackPanel.IsVisible = true;
            employeeListBox.IsVisible = false;
            questionairsListBox.IsVisible = false;
            LoadData();
        }

        private void UsersButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            switchValue = 1;
            ordersStackPanel.IsVisible = false;
            questionairsListBox.IsVisible = false;
            employeeListBox.IsVisible = true;
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

            if(switchValue == 1)
            {
                List<Employee> employees = new List<Employee>();
                employees = DataBaseClass.DataBase.Employees.Where(f => f.PboId == currentPBOid).ToList();

                //сщртировка
                switch (selectedSort)
                {
                    default:
                        employees = employees.ToList();
                        break;

                    case 1:
                        employees = employees.OrderBy(f => f.LastName)
                            .ToList();
                        break;
                    case 2:
                        employees = employees.OrderByDescending(f => f.LastName)
                            .ToList();
                        break;
                }
                //поиск
                if (searchedText != null)
                    employees = employees.Where(f => f.FirstName
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

                employeeListBox.Items = employees.Select(f => new
                {
                    FIO = $"{f.LastName} {f.FirstName} {f.Patronymic}",
                    Phone = $"Телефон: {f.Phone}",
                    Email = $"Email: {f.Email}",
                    DateOfBirth = $"Дата рождения: {f.DateOfBirth}",
                    Inn = $"Количество бонусов: {f.Inn}",
                    Passport = $"Серия и номер паспорта: {f.PassportSeries} {f.PassportNumber}",
                    Address = $"Роль: {f.Address}",
                    LaborContractNumber = $"Номер трудового договора {f.LaborContractNumber}",
                    f.EmployeeImage,
                    EmployeePosition = $"Должность: {f.EmployeePosition}"
                });
            }

            if (switchValue == 2)
            {
                List<Order> orders = new List<Order>();
                orders = DataBaseClass.DataBase.Orders.Where(f => f.PboId == currentPBOid)
                    .ToList();
                ordersForXLSX = orders;
                //сщртировка
                switch (selectedSort)
                {
                    default:
                        orders = orders.ToList();
                        break;

                    case 1:
                        orders = orders.OrderBy(f => f.OrderDate)
                            .ToList();
                        break;
                    case 2:
                        orders = orders.OrderByDescending(f => f.OrderDate)
                            .ToList();
                        break;
                }
                //поиск
                if (searchedText != null)
                    orders = orders.Where(f => f.UserName
                    .ToLower()
                    .Contains(searchedText
                    .ToLower()))
                    .ToList();


                
                proceedTextBlock.Text = $"Общая выручка ресторана: {orders.Sum(f => f.Cost)}";
                ordersListBox.Items = orders.Select(f => new
                {
                    Username = $"{f.UserName}",
                    Cost = $"Сумма: {f.Cost}",
                    OrderDate = $"Дата заказа: {f.OrderDate}",
                    SecurityCode = $"Защитный код: {f.SecurityCode}"
                });
            }

            if (switchValue == 3)
            {
                List<Questionaire> questionaires = new List<Questionaire>();
                questionaires = DataBaseClass.DataBase.Questionaires.Where(f => f.PboId == currentPBOid).ToList();

                //сщртировка
                switch (selectedSort)
                {
                    default:
                        questionaires = questionaires.ToList();
                        break;

                    case 1:
                        questionaires = questionaires.OrderBy(f => f.Surname)
                            .ToList();
                        break;
                    case 2:
                        questionaires = questionaires.OrderByDescending(f => f.Surname)
                            .ToList();
                        break;
                }
                //поиск
                if (searchedText != null)
                    questionaires = questionaires.Where(f => f.Name
                    .ToLower()
                    .Contains(searchedText
                    .ToLower()) || f.Surname
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

                questionairsListBox.Items = questionaires.Select(f => new
                {
                    FIO = $"{f.Surname} {f.Name} {f.Patronymic}",
                    Phone = $"Телефон: {f.Phone}",
                    Email = $"Email: {f.Email}",
                    DateOfBirth = $"Дата рождения: {f.DateOfBirth}",
                    Status = f.Questionairstatus,
                    f.QuestionairId,
                    f.Color
                });
            }

        }
        public async void LoadXLS()
        {
            try
            {
                Workbook workbook = new Workbook();
                Worksheet sheet = workbook.Worksheets[0];
                ordersForXLSX = DataBaseClass.DataBase.Orders.Where(f => f.PboId == currentPBOid)
                        .ToList();

                Cell A1 = sheet.Cells["A1"];
                Style styleA = A1.GetStyle();
                styleA.Font.IsBold = true;
                A1.SetStyle(styleA);
                A1.PutValue("Id заказа");

                Cell B1 = sheet.Cells["B1"];
                Style styleB = B1.GetStyle();
                styleB.Font.IsBold = true;
                B1.SetStyle(styleB);
                B1.PutValue("Дата заказа");

                Cell C1 = sheet.Cells["C1"];
                Style styleC = C1.GetStyle();
                styleC.Font.IsBold = true;
                C1.SetStyle(styleC);
                C1.PutValue("Имя пользователя");

                Cell D1 = sheet.Cells["D1"];
                Style styleD = D1.GetStyle();
                styleD.Font.IsBold = true;
                D1.SetStyle(styleD);
                D1.PutValue("Сумма заказа");

                for (int i = 0; i <= ordersForXLSX.Count() - 1; i++)
                {
                    for (int j = 0; j <= 3; j++)
                    {
                        Cell cellA = sheet.Cells[$"A{i + 2}"];
                        cellA.PutValue(ordersForXLSX[i].OrderId);
                        Cell cellB = sheet.Cells[$"B{i + 2}"];
                        cellB.PutValue(ordersForXLSX[i].OrderDate);
                        Cell cellC = sheet.Cells[$"C{i + 2}"];
                        cellC.PutValue(ordersForXLSX[i].UserName);
                        Cell cellD = sheet.Cells[$"D{i + 2}"];
                        cellD.PutValue(ordersForXLSX[i].Cost);
                    }
                }
                workbook.Save($"{currentPBOid}RestorauntSales.xlsx", SaveFormat.Xlsx);

                var warning = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Подтверждение",
                    ContentMessage = "XLSX-файл сохранен на ваш компьютер"
                }).ShowDialog(this);
            }
            catch
            {
                var warning = await MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Предупреждение",
                    ContentMessage = "Произошла ошибка"
                }).ShowDialog(this);
            }
        }
    }
}
