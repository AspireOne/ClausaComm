namespace ClausaComm
{
    public class FileObj
    {
        public readonly string Filename;
        public readonly byte[] FileBytes;


        public FileObj(string filename, byte[] fileBytes)
        {
            this.Filename = filename;
            this.FileBytes = fileBytes;
        }
    }
}
