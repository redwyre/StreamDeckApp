using StreamDeckDevice;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StreamDeckApp
{
    public class StreamDeckModel
    {
        public StreamDeck StreamDeck;

        const int ButtonCount = 15;

        public const int ImageWidth = 72;
        public const int ImageHeight = 72;

        public BitmapSource[] ImageSources { get; } = new BitmapSource[ButtonCount];

        private void StreamDeck_ButtonChanged(int button, bool state)
        {
            Debug.WriteLine($"Button: {button} State: {state}");
        }

        public StreamDeckModel()
        {
            for (int i = 0; i < ButtonCount; ++i)
            {
                var imageSource = new WriteableBitmap(ImageWidth, ImageHeight, 96, 96, PixelFormats.Bgr24, null);
                imageSource.Lock();

                unsafe
                {
                    for (int y = 0; y < ImageHeight; ++y)
                    {
                        var buffer = (byte*)(imageSource.BackBuffer.ToPointer()) + (imageSource.BackBufferStride * y);
                        for (int x = 0; x < ImageWidth * 3; x += 3)
                        {
                            buffer[x + 0] = 0xFF;
                            buffer[x + 1] = 0x8F;
                            buffer[x + 2] = 0x00;
                        }
                    }
                }

                imageSource.AddDirtyRect(new Int32Rect(0, 0, ImageWidth, ImageHeight));
                imageSource.Unlock();

                ImageSources[i] = imageSource;
            }

            StreamDeck = StreamDeck.Get();
            StreamDeck.ButtonChanged += StreamDeck_ButtonChanged;
            StreamDeck.Open();
        }
    }
}
