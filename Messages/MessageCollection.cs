using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ClausaComm.Messages;

/// <summary>
/// A collection of ChatMessages. No duplicates. Sorted by message send time (index 0 = latest).
/// </summary>
public class MessageCollection
{
    private readonly List<ChatMessage> Messages = new();

    public void Add(ChatMessage message)
    {
        for (int i = 0; i < Messages.Count; ++i)
        {
            if (Messages[i] == message)
                return;
            
            if (Messages[i].Time < message.Time)
            {
                Messages.Insert(i, message);
                return;
            }
        }
        
        Messages.Add(message);
    }
    
    public IEnumerable<ChatMessage> GetAmount(int amount)
    {
        for (int i = 0; i < (amount == 0 ? Messages.Count : amount); ++i) 
            yield return Messages[i];
    }
}