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
                Proteins = $"Белки: {f.Proteins}",
                Fats = $"Жиры: {f.Fats}",
                Calories = $"Углеводы: {f.Calories}",
                Carbohydrates = $"Калорийность: {f.Carbohydrates}",
                Energyvalue = $"Энергетическая ценность: {f.Energyvalue}"
            });
        }
    }
}
