using Avalonia.Controls;
using DiplomApplication.Windows;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomApplication.Classes
{
    internal class EnterOrRegistrationButtonClickClass
    {
        public static void EnterButtonClick()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
        public static async void RegistrationButtonClick(Window window)
        {
            AddMenuUserWindow addMenuUserWindow = new AddMenuUserWindow();
            await addMenuUserWindow.ShowDialog(window);
        }
    }
}
