using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClausaComm.Messages
{
    public abstract class MessageFile
    {
        public readonly string Filename;
        public readonly bool IsImage;

        protected MessageFile(string filename, bool isImage)
        {
            Filename = filename;
            IsImage = isImage;
        }
    }
}
