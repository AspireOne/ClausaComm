using System;

namespace ClausaComm.Exceptions
{
    [Serializable]
    public class MultipleInstancesException : Exception
    {
        public string ClassName { get; }

        public MultipleInstancesException() : base()
        {
        }

        public MultipleInstancesException(string className) : base($"An attempt was made to create a second instance of {className}. There can only be one instance.")
        {
            ClassName = className;
        }

        public MultipleInstancesException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}