using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace ClausaComm.Messages;

/// <summary>
/// A collection of ChatMessages. No duplicates. Sorted by message send time (index 0 = latest).
/// </summary>
public class MessageCollection
{
    private readonly List<ChatMessage> Messages = new();

    /// <summary>Adds a message to the collection in a sorted manner.</summary>
    /// <param name="message">The message to add</param>
    /// <returns>True if the message is unique and successfully added; false otherwise.</returns>
    public bool Add(ChatMessage message)
    {
        for (int i = 0; i < Messages.Count; ++i)
        {
            if (Messages[i] == message)
                return false;

            if (Messages[i].Time < message.Time)
            {
                Messages.Insert(i, message);
                return true;
            }
        }
        
        Messages.Add(message);
        return true;
    }
    
    /// <param name="amount">The amount of messages to return from the latest (0 for all).</param>
    /// <returns>First {amount} latest messages.</returns>
    public IEnumerable<ChatMessage> Get(int amount)
    {
        int cycles = amount == 0 || amount > Messages.Count ? Messages.Count : amount;
        for (int i = 0; i < cycles; ++i) 
            yield return Messages[i];
    }
}