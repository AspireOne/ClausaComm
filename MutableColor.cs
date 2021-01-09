using System.Drawing;

namespace ClausaComm
{
    public class MutableColor
    {
        public Color Color { get; private set; }
        private byte _a = 255;
        private byte _r;
        private byte _g;
        private byte _b;

        public byte A
        {
            get => _a;
            set
            {
                _a = value;
                Color = Color.FromArgb(A, Color.R, Color.G, Color.B);
            }
        }

        public byte R
        {
            get => _r;
            set
            {
                _r = value;
                Color = Color.FromArgb(Color.A, R, Color.G, Color.B);
            }
        }

        public byte G
        {
            get => _g;
            set
            {
                _g = value;
                Color = Color.FromArgb(Color.A, Color.R, G, Color.B);
            }
        }

        public byte B
        {
            get => _b;
            set
            {
                _b = value;
                Color = Color.FromArgb(Color.A, Color.R, Color.G, B);
            }
        }

        public MutableColor(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public MutableColor(byte a, byte r, byte g, byte b) : this(r, g, b) => A = a;
    }
}
