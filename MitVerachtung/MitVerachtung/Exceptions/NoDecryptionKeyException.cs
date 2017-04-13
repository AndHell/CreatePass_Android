using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CreatePass.Exceptions
{


    [Serializable]
    public class NoDecryptionKeyException : Exception
    {
        public NoDecryptionKeyException() { }
        public NoDecryptionKeyException(string message) : base(message) { }
        public NoDecryptionKeyException(string message, Exception inner) : base(message, inner) { }
        protected NoDecryptionKeyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    
}