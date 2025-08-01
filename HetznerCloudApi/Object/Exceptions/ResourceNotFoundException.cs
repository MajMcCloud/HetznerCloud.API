using System;
using System.Collections.Generic;
using System.Text;

namespace HetznerCloudApi.Object.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a requested resource cannot be found.
    /// </summary>
    /// <remarks>This exception is typically used to indicate that an operation failed because the specified
    /// resource does not exist or is unavailable. It can be used in scenarios such as querying a database for a
    /// non-existent record or attempting to access a file that does not exist.</remarks>
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException()
        {
        }

        public ResourceNotFoundException(string message) : base(message)
        {
        }
    }
}
