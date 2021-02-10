using ClausaComm.Messages;
using ClausaComm.Utils;
using System;
using System.Drawing;

namespace ClausaComm
{
    public class LocalMessageFile : MessageFile
    {
        public readonly string Path;


        public LocalMessageFile(string path) : base(System.IO.Path.GetFileName(path), ImageUtils.IsFileImage(path))
        {
            Path = path;
        }
    }
}
