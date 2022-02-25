using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Tires.Model;

namespace Tires.Class
{
    class PageDataManager : INotifyPropertyChanged
    {
        private int number;
        public int Number
        {
            get { return number; }
            set
            {
                number = value;
                NotifyPropertyChanged("Number");
            }
        }

        private int currentsize;
        public int Currentsize
        {
            get { return currentsize; }
            set
            {
                currentsize = value;
                NotifyPropertyChanged("Currentsize");
            }
        }

        private int total;
        public int Total
        {
            get { return total; }
            set
            {
                total = value;
                NotifyPropertyChanged("Total");
            }
        }

        private List<Pages> pages;
        public List<Pages> Pages
        {
            get { return pages; }
            set
            {
                pages = value;
                NotifyPropertyChanged("Pages");
            }
        }

        private List<Product> lst_product;
        public List<Product> Lst_product
        {
            get { return lst_product; }
            set
            {
                lst_product = value;
                NotifyPropertyChanged("Lst_product");
            }
        }

        private List<Product> lst_bind;
        public List<Product> Lst_bind
        {
            get { return lst_bind; }
            set
            {
                lst_bind = value;
                NotifyPropertyChanged("Lst_bind");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string Propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(Propertyname));
            }
        }

        public PageDataManager()
        {
            this.Number = 20;
            this.Lst_product = IgishevTiresEntities1.GetContext().Product.ToList();
            this.Total = Lst_product.Count() / Number;
            this.Pages = new List<Pages>();
            for (int i = 1; i <= Total; i++)
            {
                this.Pages.Add(new Pages()
                {
                    Name = i.ToString(),
                    PageSize = i
                });
            }
            this.Currentsize = 1;
            Pager(Currentsize);
        }

        public void Pager(int cize)
        {
            this.Currentsize = cize;
            this.Lst_bind = this.Lst_product.Take(this.Number * cize).Skip(this.Number * (cize - 1)).ToList();
        }
    }
}
