using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WhomIsThisFace.UC.Item
{
    /// <summary>
    /// Interaction logic for UCResultItem.xaml
    /// </summary>
    public partial class UCResultItem : UserControl
    {

        public UCResultItem(IImage L, String StudentID)
        {
            InitializeComponent();
            LPic.Source = Display.Bitmap.ToBitmapSource(L);

        }
        public UCResultItem(IImage L, IImage R ,String Text)
        {
            InitializeComponent();
            LPic.Source = Display.Bitmap.ToBitmapSource(L);
            RPic.Source = Display.Bitmap.ToBitmapSource(R);

            Name.Content = Text;
        }
    }
}
