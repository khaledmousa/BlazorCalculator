using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace BlazorCalculator.Web.ViewModels
{
    public class CyclicVertexException : Exception
    {
        public CyclicVertexException() { }
        public CyclicVertexException(string message) : base(message) { }
        public CyclicVertexException(string message, Exception innerException) : base(message, innerException) { }
        protected CyclicVertexException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
