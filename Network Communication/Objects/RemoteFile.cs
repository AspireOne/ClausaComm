using System;
using System.IO;
using System.Text.Json.Serialization;
using NAudio.Wave;
using Newtonsoft.Json;

namespace ClausaComm.Network_Communication.Objects;

public class RemoteFile : ISendable
{
    public RemoteObject.ObjectType ObjectType => RemoteObject.ObjectType.File;
    
    public string FileName { get; set; }
    [Newtonsoft.Json.JsonIgnore] public string FilePath { get; set; }

    public RemoteFile(string filePath)
    {
        FilePath = filePath;
        FileName = Path.GetFileName(filePath);
    }

    [Newtonsoft.Json.JsonConstructor]
    private RemoteFile(string filePath, string filename)
    {
        this.FilePath = filePath;
        this.FileName = filename;
    }
}