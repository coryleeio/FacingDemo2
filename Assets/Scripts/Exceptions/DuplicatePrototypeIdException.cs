using System;

public class DuplicatePrototypeIdException : Exception
{
    public DuplicatePrototypeIdException()
    {
    }

    public DuplicatePrototypeIdException(string message)
        : base(message)
    {
    }

    public DuplicatePrototypeIdException(string message, Exception inner)
        : base(message, inner)
    {
    }
}