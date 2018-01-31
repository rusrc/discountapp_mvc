using System;

namespace Discountapp.Infrastructure.ImageLibrary.Exceptions
{

    [Serializable]
    public class ImageLibraryException : Exception
    {
        public ImageLibraryException() { }
        public ImageLibraryException(string message) : base(message) { }
        public ImageLibraryException(string message, Exception inner) : base(message, inner) { }
        protected ImageLibraryException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
