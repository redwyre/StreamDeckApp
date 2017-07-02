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

namespace StreamDeckApp
{
    /// <summary>
    /// Interaction logic for StreamDeckButton.xaml
    /// </summary>
    public partial class StreamDeckButton : UserControl
    {
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(WriteableBitmap), typeof(StreamDeckButton));

        public WriteableBitmap ImageSource
        {
            get { return (WriteableBitmap)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public StreamDeckButton()
        {
            InitializeComponent();
        }
    }
}
