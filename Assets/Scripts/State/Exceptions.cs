using System;

namespace Gamepackage
{
    public class GamePackageException : Exception
    {
        public GamePackageException() : base() { }
        public GamePackageException(string message) : base(message) { }
    }

    public class CouldNotResolvePrototypeException : GamePackageException
    {
        public CouldNotResolvePrototypeException() : base() { }
        public CouldNotResolvePrototypeException(string message) : base(message) { }
    }

    public class DuplicateAssembliesException : GamePackageException
    {
        public DuplicateAssembliesException() : base() { }
        public DuplicateAssembliesException(string message) : base(message) { }
    }

    public class DuplicateBundleNameException : GamePackageException
    {
        public DuplicateBundleNameException() : base() { }
        public DuplicateBundleNameException(string message) : base(message) { }
    }

    public class DuplicatePrototypeIdException : GamePackageException
    {
        public DuplicatePrototypeIdException() : base() { }
        public DuplicatePrototypeIdException(string message) : base(message) { }
    }

    public class NotImplementedException : GamePackageException
    {
        public NotImplementedException() : base() { }
        public NotImplementedException(string message) : base(message) { }
    }

    public class InvariantBrokenException : GamePackageException
    {
        public InvariantBrokenException() : base() { }
        public InvariantBrokenException(string message) : base(message) { }
    }
}
