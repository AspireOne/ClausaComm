using ClausaComm.Utils;

namespace ClausaComm.Messages
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
