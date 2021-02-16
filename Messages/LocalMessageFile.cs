using ClausaComm.Utils;
using System;
using System.IO;
using System.Windows.Forms;

namespace ClausaComm.Messages
{
    public class LocalMessageFile : MessageFile
    {
        public readonly string Path;

        public LocalMessageFile(string path) : base(System.IO.Path.GetFileName(path), ImageUtils.IsFileImage(path))
        {
            PerformValidityChecks();
            Path = path;

            void PerformValidityChecks()
            {
                if (!File.Exists(path))
                    ThrowArgumentException($"The path ({path}) doesn't exist");

                if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
                    ThrowArgumentException("The specified file is not a file, but a directory.");

                if (new FileInfo(path).Length > MaxFileSizeBytes)
                    ThrowArgumentException("The file is too large.");

                void ThrowArgumentException(string msg) => throw new ArgumentException(msg, nameof(path));
            }
        }
    }
}