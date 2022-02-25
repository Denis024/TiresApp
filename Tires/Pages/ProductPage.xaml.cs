using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Tires.Class;
using Tires.Model;

namespace Tires.Pages
{
    public partial class ProductPage : Page
    {
        PageList PageList;

        public ProductPage()
        {
            InitializeComponent();

            var AllType = IgishevTiresEntities1.GetContext().ProductType.ToList();
            AllType.Insert(0, new ProductType
            {
                Title = "Все типы"
            });
            ComboBoxFiltration.ItemsSource = AllType;

            Update();
        }

        private void Update()
        {
            var currentProduct = IgishevTiresEntities1.GetContext().Product.ToList();

            //Поиск
            currentProduct = currentProduct.Where(p => p.Title.ToLower().Contains(TextBoxFind.Text.ToLower())).ToList();

            //Фильтрация
            if (ComboBoxFiltration.SelectedIndex > 0)
                currentProduct = currentProduct.FindAll(s => s.ProductType == ComboBoxFiltration.SelectedItem);

            //Сортировка
            var sort = currentProduct.ToList();

            switch (ComboBoxSorting.SelectedIndex)
            {
                case 0: { sort = currentProduct.ToList(); break; }
                case 1: { sort = currentProduct.OrderBy(s => s.Title).ToList(); break; }
                case 2: { sort = currentProduct.OrderByDescending(s => s.Title).ToList(); break; }
                case 3: { sort = currentProduct.OrderBy(s => s.ProductionWorkshopNumber).ToList(); break; }
                case 4: { sort = currentProduct.OrderByDescending(s => s.ProductionWorkshopNumber).ToList(); break; }
                case 5: { sort = currentProduct.OrderBy(s => s.MinCostForAgent).ToList(); break; }
                case 6: { sort = currentProduct.OrderByDescending(s => s.MinCostForAgent).ToList(); break; }
            }

            PageList = new PageList(sort);
            ListViewTires.ItemsSource = PageList.OffsetProducts;
        }

        private void TextBoxFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void ComboBoxSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void ComboBoxFiltration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            PageList.CurrentPage--;
            ListViewTires.ItemsSource = PageList.OffsetProducts;
            BtnBack.IsEnabled = PageList.IsFirstPage;
            BtnNext.IsEnabled = PageList.IsLastPage;
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            PageList.CurrentPage++;
            ListViewTires.ItemsSource = PageList.OffsetProducts;
            BtnBack.IsEnabled = PageList.IsFirstPage;
            BtnNext.IsEnabled = PageList.IsLastPage;
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            TextBoxFind.Text = "";
            ComboBoxSorting.SelectedIndex = 0;
            ComboBoxFiltration.SelectedIndex = 0;
        }

        private void ListViewTires_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBlockID.DataContext = ListViewTires.SelectedItem;
            Product EditProduct = IgishevTiresEntities1.GetContext().Product.Find(int.Parse(TextBlockID.Text));
            Manager.GetFrame.Navigate(new AddEditPage(int.Parse(TextBlockID.Text)));
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.GetFrame.Navigate(new AddEditPage(0));
        }
    }
}
