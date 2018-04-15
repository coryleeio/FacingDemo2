using System;

public class CouldNotResolvePrototypeException : Exception
{
    public CouldNotResolvePrototypeException()
    {
    }

    public CouldNotResolvePrototypeException(string message)
        : base(message)
    {
    }

    public CouldNotResolvePrototypeException(string message, Exception inner)
        : base(message, inner)
    {
    }
}