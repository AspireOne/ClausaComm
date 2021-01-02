using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClausaComm
{
    public static class IdGenerator
    {
        private static readonly StringBuilder IdBuilder = new(Length);
        private static readonly char[] AllowedChars = {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q',
                'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h',
                'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y',
                'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };
        public const byte Length = 64;

        public static string GenerateId()
        {
            for (var i = 0; i < Length; ++i)
                IdBuilder.Append(AllowedChars[Constants.Random.Next(0, AllowedChars.Length)]);

            string id = IdBuilder.ToString();
            IdBuilder.Clear();
            return id;
        }
    }
}
