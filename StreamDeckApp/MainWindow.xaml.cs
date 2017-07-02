using StreamDeckDevice;
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

        private async void Button_Click_Red(object sender, RoutedEventArgs e)
        {
            byte[] buffer = new byte[72 * 72 * 3];

            byte r = 255, g = 0, b = 0;

            for (int y = 0; y < 72; ++y)
            {
                int yOff = y * 72 * 3;
                for (int x = 0; x < 72; ++x)
                {
                    buffer[yOff + (x * 3) + 0] = (byte)(x * 2);
                    buffer[yOff + (x * 3) + 1] = g;
                    buffer[yOff + (x * 3) + 2] = r;
                }
            }

            await StreamDeck.SetButtonImage(0, buffer);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            StreamDeck.SetBrightness(10);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            StreamDeck.SetBrightness(100);
        }

        private async void Button_Click_Black(object sender, RoutedEventArgs e)
        {
            byte[] buffer = new byte[72 * 72 * 3];

            byte r = 0, g = 0, b = 0;

            for (int y = 0; y < 72; ++y)
            {
                int yOff = y * 72 * 3;
                for (int x = 0; x < 72; ++x)
                {
                    buffer[yOff + (x * 3) + 0] = b;
                    buffer[yOff + (x * 3) + 1] = g;
                    buffer[yOff + (x * 3) + 2] = r;
                }
            }

            await StreamDeck.SetButtonImage(0, buffer);
        }

        private async void Button_Click_A(object sender, RoutedEventArgs e)
        {
            using (ButtonImageBuilder ib = new ButtonImageBuilder())
            {
                ib.Fill(0x0000FF);
                ib.DrawText("A", 0xFFFFFF, true, true);

                await StreamDeck.SetButtonImage(0, ib.Bitmap);
            }
        }

        private async void Button_Click_Redwyre(object sender, RoutedEventArgs e)
        {
            using (ButtonImageBuilder ib = new ButtonImageBuilder())
            {
                ib.Fill(0xFFFFFF);
                ib.DrawText("R", 0xFF0000, true, true);

                await StreamDeck.SetButtonImage(4, ib.Bitmap);

                ib.Fill(0xFFFFFF);
                ib.DrawText("E", 0xFF0000, true, true);

                await StreamDeck.SetButtonImage(3, ib.Bitmap);

                ib.Fill(0xFFFFFF);
                ib.DrawText("D", 0xFF0000, true, true);

                await StreamDeck.SetButtonImage(2, ib.Bitmap);

                ib.Fill(0xFFFFFF);
                ib.DrawText("W", 0xFF0000, true, true);

                await StreamDeck.SetButtonImage(1, ib.Bitmap);

                ib.Fill(0xFFFFFF);
                ib.DrawText("Y", 0xFF0000, true, true);

                await StreamDeck.SetButtonImage(0, ib.Bitmap);

                ib.Fill(0xFFFFFF);
                ib.DrawText("R", 0xFF0000, true, true);

                await StreamDeck.SetButtonImage(9, ib.Bitmap);

                ib.Fill(0xFFFFFF);
                ib.DrawText("E", 0xFF0000, true, true);

                await StreamDeck.SetButtonImage(8, ib.Bitmap);
            }
        }

        private void Button_Click_Reset(object sender, RoutedEventArgs e)
        {
            StreamDeck.ResetScreen();
        }
    }
}
