using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TWeibullMarkovLibrary
{
    /// <summary>
    /// Extends EvntArgs
    /// </summary>
    public class MyEventArgs: EventArgs
    {
        /// <summary>
        /// Any object that needs to be passed.
        /// </summary>
        public Object Cargo = null;
    }
}
