using Tires.Model;

namespace Tires.Class
{
    public class CustomMaterial : Material
    {
        public bool IsSelected { get; set; }
        public CustomMaterial(Material material, bool isSelected = false)
        {
            ID = material.ID;
            Title = material.Title;
            IsSelected = isSelected;
        }
    }
}
