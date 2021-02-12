using ClausaComm.Communication;
using ClausaComm.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClausaComm.Network_Communication
{
    [Serializable]
    public readonly struct RemoteContactData : ISendable
    {
        RemoteObject.ObjectType ISendable.ObjType => RemoteObject.ObjectType.ContactData;
        // Convert it so that it¨s serializable.
        public readonly string Base64ProfilePicture;
        public readonly string Name;
        //public readonly string Id;


        public RemoteContactData(/*string id = null, */ string name = null, Image profilePicture = null)
        {
            //  Id = id;
            Name = name;
            Base64ProfilePicture = ImageUtils.ImageToBase64String(profilePicture);
            // TODO FOR TOMORROW: FIND OUT IF profilePicture IS SERIALIZABLE AS AN IMAGE INSTEAD OF A BYTE ARRAY
            // Probably just convert it to base64 string.
        }
    }
}
