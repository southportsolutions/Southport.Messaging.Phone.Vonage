using System;

namespace Southport.Messaging.Phone.Vonage.Shared;

public class VonageException : Exception
{
    public VonageException(string message) : base(message) { }
}