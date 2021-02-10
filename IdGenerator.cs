using System.Text;

namespace ClausaComm
{
    public static class IdGenerator
    {
        // Amount of possible permutations = AllowedChars^length.
        // 61^8 = 191,707,312,997,281 (191 bilionů (trillion) 707 miliard (billion) 312 milionů (million)...)
        private const byte DefaultLength = 8;
        private static readonly StringBuilder IdBuilder = new(DefaultLength);
        private static readonly char[] AllowedChars = {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q',
                'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h',
                'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y',
                'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };

        public static string GenerateId(int length = DefaultLength)
        {
            for (var i = 0; i < length; ++i)
                IdBuilder.Append(AllowedChars[Constants.Random.Next(0, AllowedChars.Length)]);

            var id = IdBuilder.ToString();
            IdBuilder.Clear();
            return id;
        }
    }
}
