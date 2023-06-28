﻿namespace Southport.Messaging.Phone.Vonage.Shared;

public class VonageSmsResponseException : VonageException
{
    public VonageSmsResponseException(string message) : base(message) { }

    public SendSmsResponse Response { get; set; }
}