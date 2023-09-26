using System;

namespace Southport.Messaging.Phone.Vonage.Shared.Vonage;

public class VonageException : Exception
{
    public VonageException(string message) : base(message) { }
}