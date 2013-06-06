using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
    /// <summary>
    /// A FileListener writes messages to a file.
    /// </summary>
    public class TextFileListener
        : TextWriterListener
    {
        #region Variables
        private const string _DefaultFileName = "fosol.diagnostics.log";

        private Fosol.Common.Formatters.StringFormatter _FileName;
        private bool _Append;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The file name to write messages to.
        /// </summary>
        [DefaultValue(_DefaultFileName)]
        [TraceSetting("FileName", typeof(Fosol.Common.Converters.StringFormatterConverter))]
        public Fosol.Common.Formatters.StringFormatter FileName
        {
            get 
            {
                _ChildLock.EnterReadLock();
                try
                {
                    return _FileName;
                }
                finally
                {
                    _ChildLock.ExitReadLock();
                }
            }
            set 
            {
                _ChildLock.EnterWriteLock();
                try
                {
                    _FileName = value;
                }
                finally
                {
                    _ChildLock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// get/set - Controls whether message will be appended to an existing file.
        /// </summary>
        [DefaultValue(true)]
        [TraceSetting("Append")]
        public bool Append
        {
            get
            {
                _ChildLock.EnterReadLock();
                try
                {
                    return _Append;
                }
                finally
                {
                    _ChildLock.ExitReadLock();
                }
            }
            set
            {
                _ChildLock.EnterWriteLock();
                try
                {
                    _Append = value;
                }
                finally
                {
                    _ChildLock.ExitWriteLock();
                }
            }
        }
        #endregion

        #region Constructors
        public TextFileListener()
            : base()
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Ensures that this listener has initialized the Stream that will be written to.
        /// This will create a file in the executable directory if a new path is not specified.
        /// </summary>
        /// <returns>'True' if the Writer has been initialized.</returns>
        protected override bool EnsureWriter()
        {
            if (this.Writer == null)
            {
                var generated_file_name = this.FileName.Render(null);
                if (string.IsNullOrEmpty(generated_file_name))
                    return false;

                var path = System.IO.Path.GetFullPath(generated_file_name);
                var directory_name = System.IO.Path.GetDirectoryName(path);
                var file_name = System.IO.Path.GetFileName(path);

                // Attempt to create the file at the path location.  
                // If it fails it may attempt to append a Guid to the file name and try once more.
                for (int i = 0; i < 2; i++)
                {
                    try
                    {
                        this.Writer = new StreamWriter(path, this.Append, this.Encoding, this.BufferSize);
                        return true;
                    }
                    catch (IOException)
                    {
                        file_name = Guid.NewGuid().ToString() + file_name;
                        path = System.IO.Path.Combine(directory_name, file_name);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        return false;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
