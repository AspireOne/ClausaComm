using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace ClausaComm.Utils
{
    internal static class ImageUtils
    {
        public static Image ClipToCircle(Image img)
        {
            if (img is null)
                return null;

            int x = img.Width / 2;
            int y = img.Height / 2;
            int radius = Math.Min(x, y);
            int radiusMultiplied = 2 * radius;

            Bitmap tmp = new(radiusMultiplied, radiusMultiplied);
            using var g = Graphics.FromImage(tmp);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TranslateTransform(tmp.Width / 2, tmp.Height / 2);
            using GraphicsPath gp = new();
            gp.AddEllipse(0 - radius, 0 - radius, radiusMultiplied, radiusMultiplied);
            using Region rg = new(gp);
            g.SetClip(rg, CombineMode.Replace);
            using Bitmap bmp = new(img);
            g.DrawImage(
                bmp,
                new Rectangle(-radius, -radius, radiusMultiplied, radiusMultiplied),
                new Rectangle(x - radius, y - radius, radiusMultiplied, radiusMultiplied),
                GraphicsUnit.Pixel);

            return tmp;
        }

        public static Image ForEveryPixel(this Image img, Func<Color, Color> func)
        {
            if (img is null)
                return null;

            Bitmap originalImage = new(img);
            Bitmap alteredImage = new(img.Width, img.Height);

            for (int x = 0; x < originalImage.Width; ++x)
            {
                for (int y = 0; y < originalImage.Height; ++y)
                {
                    Color pixel = originalImage.GetPixel(x, y);
                    Color alteredPixel = func(pixel);
                    alteredImage.SetPixel(x, y, alteredPixel);
                }
            }

            return alteredImage;
        }

        public static Image AlterColor(Image image, Color newColor)
        {
            return image.ForEveryPixel(pixel =>
            {
                if (pixel.A == 0)
                    return pixel;

                return Color.FromArgb(pixel.A, newColor.R, newColor.G, newColor.B);
            });
        }

        public static Image AlterTransparency(Image image, byte alpha)
        {
            return image.ForEveryPixel(pixel =>
            {
                if (pixel.A == 0)
                    return pixel;

                return Color.FromArgb(alpha, pixel.R, pixel.G, pixel.B);
            });
        }

        public static Bitmap HQResize(Image image, int width, int height)
        {
            if (image is null)
                return null;

            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using var wrapMode = new ImageAttributes();
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }

            return destImage;
        }

        public static bool IsFileImage(string path)
        {
            // Image.FromFile throws an OutOfMemoryException when the file is not an image.
            // Reference: https://stackoverflow.com/questions/9354747/how-can-i-determine-if-a-file-is-an-image-file-in-net
            try
            {
                Image.FromFile(path).Dispose();
                return true;
            }
            catch (OutOfMemoryException)
            {
                return false;
            }
        }

        public static string? ImageToBase64String(Image? img)
        {
            if (img is null)
                return null;

            using (MemoryStream ms = new())
            {
                img.Save(ms, ImageFormat.Png);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public static Image ImageFromBase64String(string imgStr)
        {
            byte[] array = Convert.FromBase64String(imgStr);

            using (MemoryStream ms = new(array))
                return Image.FromStream(ms);
        }
    }
}