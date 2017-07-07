using BlazingHeart.StreamDeck;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        Bitmap maskBitmap = new Bitmap(StreamDeck.ImageWidth, StreamDeck.ImageHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

        const int KeyCount = 15;

        public const int ImageWidth = 72;
        public const int ImageHeight = 72;

        public WriteableBitmap[] ImageSources { get; } = new WriteableBitmap[KeyCount];

        private void StreamDeck_KeyChanged(int key, bool state)
        {
            Debug.WriteLine($"Key: {key} State: {state}");
        }

        public StreamDeckModel()
        {
            for (int i = 0; i < KeyCount; ++i)
            {
                ImageSources[i] = new WriteableBitmap(ImageWidth, ImageHeight, 96, 96, PixelFormats.Bgr24, null);
            }

            using (Graphics g = Graphics.FromImage(maskBitmap))
            {
                g.Clear(System.Drawing.Color.Black);


                int diameter = 32;
                var arc = new Rectangle(0, 0, diameter, diameter);
                var path = new GraphicsPath();

                // top left arc  
                path.AddArc(arc, 180, 90);

                // top right arc  
                arc.X = ImageWidth - diameter;
                path.AddArc(arc, 270, 90);

                // bottom right arc  
                arc.Y = ImageHeight - diameter;
                path.AddArc(arc, 0, 90);

                // bottom left arc 
                arc.X = 0;
                path.AddArc(arc, 90, 90);

                path.CloseFigure();

                using (path)
                {
                    g.FillPath(new System.Drawing.SolidBrush(System.Drawing.Color.White), path);
                }
            }

            StreamDeck = StreamDeck.Get();
            StreamDeck.KeyChanged += StreamDeck_KeyChanged;
            StreamDeck.Open();
        }

        public void SetBrightness(int brightness)
        {
            StreamDeck.SetBrightness(brightness);
        }

        public async Task SetScreenImageGaps(Bitmap bitmap)
        {
            if (bitmap.Width != StreamDeck.ScreenGapsImageWidth || bitmap.Height != StreamDeck.ScreenGapsImageHeight)
            {
                throw new InvalidOperationException("Image Width and Height must be 72");
            }

            if (bitmap.PixelFormat != System.Drawing.Imaging.PixelFormat.Format24bppRgb)
            {
                throw new InvalidOperationException("Image format must be PixelFormat.Format24bppRgb");
            }

            var keyBitmap = new Bitmap(ImageWidth, ImageHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            var destRect = new Rectangle(0, 0, ImageWidth, ImageHeight);
            using (Graphics g = Graphics.FromImage(keyBitmap))
            {
                for (int row = 0; row < 3; ++row)
                {
                    for (int col = 0; col < 5; ++col)
                    {
                        int x = col * ImageWidth + Math.Max(col - 1, 0) * StreamDeck.Gap;
                        int y = row * ImageHeight + Math.Max(row - 1, 0) * StreamDeck.Gap;
                        g.DrawImage(bitmap, destRect, new Rectangle(x, y, ImageWidth, ImageHeight), GraphicsUnit.Pixel);

                        await SetKeyImage((row * 5) + (4 - col), keyBitmap);
                    }
                }
            }
        }

        public void ShowLogo()
        {
            var kb = new KeyImageBuilder();
            kb.Fill(0);

            StreamDeck.ShowLogo();
            for (int i = 0; i < KeyCount; ++i)
            {
                UpdateKeyImage(i, kb.Bitmap);
            }
        }

        public async Task SetKeyImage(int keyId, Bitmap bitmap)
        {
            await StreamDeck.SetKeyImage(keyId, bitmap);

            UpdateKeyImage(keyId, bitmap);
        }

        private void UpdateKeyImage(int keyId, Bitmap bitmap)
        {
            var src = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, StreamDeck.ImageWidth, StreamDeck.ImageHeight),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            try
            {
                ImageSources[keyId].WritePixels(new Int32Rect(0, 0, bitmap.Width, bitmap.Height), src.Scan0, src.Stride * src.Height * 3, src.Stride);
            }
            finally
            {
                bitmap.UnlockBits(src);
            }
        }
    }
}
