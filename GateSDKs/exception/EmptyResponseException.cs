using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GateSDKs.exception
{
    [Serializable()]
    public class EmptyResponseException : Exception
    {
        public EmptyResponseException(string message) : base(message) { }
        public EmptyResponseException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected EmptyResponseException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }
}
