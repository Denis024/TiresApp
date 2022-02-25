using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Tires.Class;
using Tires.Model;

namespace Tires.Pages
{
    public partial class AddEditPage : Page
    {
        private Product GetProduct = new Product();

        List<CustomMaterial> customMaterials;
        List<Material> materials;

        public AddEditPage(int id)
        {
            InitializeComponent();

            Product EditProduct = IgishevTiresEntities1.GetContext().Product.Find(id);

            if (EditProduct != null)
            {
                GetProduct = EditProduct;
                BtnDelete.Visibility = Visibility.Visible;
            }

            ComboBoxType.ItemsSource = IgishevTiresEntities1.GetContext().ProductType.ToList();

            AddInComboBoxProductionPersonCount();
            AddInComboBoxProductionWorkshopNumber();

            DataContext = GetProduct;

            materials = IgishevTiresEntities1.GetContext().Material.ToList();
            customMaterials = new List<CustomMaterial>();
            materials.ForEach(material => customMaterials.Add(new CustomMaterial(material)));
            customMaterials.ForEach(material =>
            {
                if (GetProduct.ProductMaterial.Any(t => t.MaterialID == material.ID))
                    material.IsSelected = true;
            });
            DataGridMaterials.ItemsSource = customMaterials;
        }

        private void AddInComboBoxProductionPersonCount()
        {
            ComboBoxProductionPersonCount.Items.Add("1");
            ComboBoxProductionPersonCount.Items.Add("2");
            ComboBoxProductionPersonCount.Items.Add("3");
            ComboBoxProductionPersonCount.Items.Add("4");
            ComboBoxProductionPersonCount.Items.Add("5");
            ComboBoxProductionPersonCount.Items.Add("6");
            ComboBoxProductionPersonCount.Items.Add("7");
            ComboBoxProductionPersonCount.Items.Add("8");
            ComboBoxProductionPersonCount.Items.Add("9");
            ComboBoxProductionPersonCount.Items.Add("10");
        }

        private void AddInComboBoxProductionWorkshopNumber()
        {
            ComboBoxProductionWorkshopNumber.Items.Add("1");
            ComboBoxProductionWorkshopNumber.Items.Add("2");
            ComboBoxProductionWorkshopNumber.Items.Add("3");
            ComboBoxProductionWorkshopNumber.Items.Add("4");
            ComboBoxProductionWorkshopNumber.Items.Add("5");
            ComboBoxProductionWorkshopNumber.Items.Add("6");
            ComboBoxProductionWorkshopNumber.Items.Add("7");
            ComboBoxProductionWorkshopNumber.Items.Add("8");
            ComboBoxProductionWorkshopNumber.Items.Add("9");
            ComboBoxProductionWorkshopNumber.Items.Add("10");
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.GetFrame.GoBack();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var ArticleNumberTest = IgishevTiresEntities1.GetContext().Product.Where(c => c.ArticleNumber == TextBoxArticle.Text).FirstOrDefault();

            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(GetProduct.Title))
                errors.AppendLine("Укажите наименование товара.");
            if (ArticleNumberTest != null)
                errors.AppendLine("Указанный артикул уже существует,\nукажите другой.");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (GetProduct.ID == 0)
            {
                GetProduct.ProductMaterial.Clear();
                customMaterials.ForEach(material =>
                {
                    if (material.IsSelected)
                        GetProduct.ProductMaterial.Add(IgishevTiresEntities1.GetContext().ProductMaterial.Find(material.ID));
                });
                IgishevTiresEntities1.GetContext().Product.Add(GetProduct);
            }
            else
            {
                GetProduct = IgishevTiresEntities1.GetContext().Product.Find(GetProduct.ID);
                IgishevTiresEntities1.GetContext().Entry(GetProduct).State = EntityState.Modified;
            }

            try
            {
                IgishevTiresEntities1.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!");
                Manager.GetFrame.Navigate(new ProductPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void BtnPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == true)
            {
                ImagePhoto.Source = new BitmapImage(new Uri(openFile.FileName));
            }

            var FileNameToSave = DateTime.Now.ToFileTime() + Path.GetExtension(openFile.FileName);
            var Img = Path.Combine($"{AppDomain.CurrentDomain.BaseDirectory}products\\{FileNameToSave}");
            TextBoxPathImage.Text = Img;
            TextBoxPathImage.Focus();
            File.Copy(openFile.FileName, Img);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            int i = int.Parse(TextBlockID.Text);
            Product DeleteProduct = IgishevTiresEntities1.GetContext().Product.Find(i);
            if (DeleteProduct.ProductSale.Count != 0)
            {
                MessageBox.Show("Невозможно удалить продукт, так как есть связанные данные!");
                return;
            }
            if (MessageBox.Show("Вы действительно хотите удалить продукт?", "Удаление", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            IgishevTiresEntities1.GetContext().Product.Remove(DeleteProduct);
            IgishevTiresEntities1.GetContext().SaveChanges();
            MessageBox.Show("Продукт успешно удален.");
            Manager.GetFrame.Navigate(new ProductPage());
        }
    }
}
