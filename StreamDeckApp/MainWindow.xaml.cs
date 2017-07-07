using BlazingHeart.StreamDeck;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public StreamDeck StreamDeck;
        public StreamDeckModel Model = new StreamDeckModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = Model;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
        }

        private async void Button_Click_Redwyre(object sender, RoutedEventArgs e)
        {
            using (KeyImageBuilder ib = new KeyImageBuilder())
            {
                ib.Fill(0xFFFFFF);
                ib.DrawText("R", 0xFF0000, true, true);

                await Model.SetKeyImage(4, ib.Bitmap);

                ib.Fill(0xFFFFFF);
                ib.DrawText("E", 0xFF0000, true, true);

                await Model.SetKeyImage(3, ib.Bitmap);

                ib.Fill(0xFFFFFF);
                ib.DrawText("D", 0xFF0000, true, true);

                await Model.SetKeyImage(2, ib.Bitmap);

                ib.Fill(0xFFFFFF);
                ib.DrawText("W", 0xFF0000, true, true);

                await Model.SetKeyImage(1, ib.Bitmap);

                ib.Fill(0xFFFFFF);
                ib.DrawText("Y", 0xFF0000, true, true);

                await Model.SetKeyImage(0, ib.Bitmap);

                ib.Fill(0xFFFFFF);
                ib.DrawText("R", 0xFF0000, true, true);

                await Model.SetKeyImage(9, ib.Bitmap);

                ib.Fill(0xFFFFFF);
                ib.DrawText("E", 0xFF0000, true, true);

                await Model.SetKeyImage(8, ib.Bitmap);
            }
        }

        private void Button_Click_Reset(object sender, RoutedEventArgs e)
        {
            Model.ShowLogo();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Model.SetBrightness((int)e.NewValue);
        }

        private async void Button_Click_Image(object sender, RoutedEventArgs e)
        {
            Font font = new Font(System.Drawing.FontFamily.GenericSansSerif, 300, System.Drawing.FontStyle.Bold);
            Bitmap bitmap = new Bitmap(StreamDeck.ScreenGapsImageWidth, StreamDeck.ScreenGapsImageHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(System.Drawing.Color.White);

                var text = "////////";
                var measure = g.MeasureString(text, font);
                float x = (bitmap.Width - measure.Width) / 2.0f;
                float y = (bitmap.Height - measure.Height) / 2.0f;

                g.DrawString(text, font, new System.Drawing.SolidBrush(System.Drawing.Color.Red), new PointF(x, y));

            }
            await Model.SetScreenImageGaps(bitmap);
        }
    }
}
