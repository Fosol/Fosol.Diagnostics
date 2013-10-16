using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// Provides a way to enforce strong function and constructor definitions while allowing generic object data to be passed to them.
    /// Contains a collection of data within a dictionary.
    /// </summary>
    public sealed class TraceTag
    {
        #region Variables
        private readonly Dictionary<string, object> _Tags;
        private bool _IsReadonly = false;
        #endregion

        #region Properties
        /// <summary>
        /// get - The data stored within this object.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Cannot set a value if IsReadonly = 'true'.</exception>
        public object this[string key]
        {
            get { return _Tags[key]; }
            set 
            {
                if (this.IsReadonly)
                    throw new InvalidOperationException();

                _Tags[key] = value; 
            }
        }

        /// <summary>
        /// get - The keys that identify each piece of data within the collection.
        /// </summary>
        public Dictionary<string, object>.KeyCollection Keys
        {
            get { return _Tags.Keys; }
        }

        /// <summary>
        /// get/set - Whether the data is readonly.
        /// </summary>
        public bool IsReadonly
        {
            get { return _IsReadonly; }
            internal set { _IsReadonly = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TraceData object.
        /// </summary>
        /// <param name="data">Generic data to be stored within this object.</param>
        public TraceTag()
        {
            _Tags = new Dictionary<string, object>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Checks if the collection contains the specified key.
        /// </summary>
        /// <param name="key">Key name to search for.</param>
        /// <returns>'True' if the key exists within the collection.</returns>
        public bool ContainsKey(string key)
        {
            return _Tags.ContainsKey(key);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
