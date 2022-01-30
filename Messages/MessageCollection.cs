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

    /// <summary>
    /// Adds a message to the collection in a sorted manner.
    /// </summary>
    /// <param name="message">The message to add</param>
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
    
    /// <param name="amount">The amount of messages to return (0 for all).</param>
    /// <returns>First {amount} latest messages.</returns>
    public IEnumerable<ChatMessage> GetAmount(int amount)
    {
        int cycles = amount == 0 || amount > Messages.Count ? Messages.Count : amount;
        for (int i = 0; i < cycles; ++i) 
            yield return Messages[i];
    }
}