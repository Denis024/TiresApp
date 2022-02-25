using System.Collections.Generic;
using System.Linq;
using Tires.Model;

namespace Tires.Class
{
    public class PageList
    {
        public List<Product> Products { get; set; }
        public List<Product> OffsetProducts => Products.Skip(CurrentPage * CountInPage).Take(CountInPage).ToList();
        public int CountInPage { get; set; } = 20;
        public int CurrentPage { get; set; } = 0;
        public bool IsFirstPage => CurrentPage != 0;
        public bool IsLastPage => Products.Count - ((CurrentPage + 2) * CountInPage) > -CountInPage;
        public PageList(List<Product> products) => Products = products;
    }
}
