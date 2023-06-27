using Aspose.Cells.Charts;
using Avalonia.Controls;
using DiplomApplication.Classes;
using System.Collections.Generic;
using System.Linq;

namespace DiplomApplication.Windows
{
    public partial class CompositionWindow : Window
    {
        public CompositionWindow()
        {
            InitializeComponent();
        }
        public CompositionWindow(int menuItemId)
        {
            InitializeComponent();
            List<DataBase.Menu> selectedItem = DataBaseClass.DataBase.Menus.Where(f => f.MenuItemId == menuItemId).ToList();
            compositionListBox.Items = selectedItem.Select(f => new
            {
                f.Title,
                Composition = $"{f.Composition}",
                Proteins = $"�����: {f.Proteins}",
                Fats = $"����: {f.Fats}",
                Calories = $"��������: {f.Calories}",
                Carbohydrates = $"������������: {f.Carbohydrates}",
                Energyvalue = $"�������������� ��������: {f.Energyvalue}"
            });
        }
    }
}
