using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiPo.ViewModels
{
    public class ImageModel
    {
        public ImageModel(string placeHolder, string source, double width = double.MaxValue, double height = double.MaxValue)
        {
            PlaceHolder = placeHolder;
            Source = source;
            Width = width;
            Height = height;
        }

        public double Width { get; }
        public double Height { get; }
        public string PlaceHolder { get; }
        public string Source { get; }
    }

    public class ImageViewModel : ViewModelBase
    {
        public ImageViewModel(ImageModel[] images, int selectedIndex = 0)
        {
            Images = images;
            SelectedIndex = selectedIndex;
        }

        public ImageModel[] Images { get; set; }
        public int SelectedIndex { get; set; } = 0;
    }
}
