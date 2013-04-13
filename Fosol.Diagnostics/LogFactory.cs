using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fosol.Diagnostics
{
    /// <summary>
    /// Internal class that maintains all the LogWriters.
    /// </summary>
    sealed class LogFactory
        : IDisposable
    {
        #region Variables
        private readonly LogWriterCache _Writers = new LogWriterCache();
        private readonly string _ApplicationName;
        #endregion

        #region Properties
        public LogWriterCache Writers
        {
            get
            {
                return _Writers;
            }
        }
        #endregion

        #region Constructors
        public LogFactory()
        {
            _ApplicationName = GetApplicationName();
        }
        #endregion

        #region Methods
        public LogWriter GetWriter()
        {
            return GetWriter(_ApplicationName);
        }

        public LogWriter GetWriter(string name)
        {
            Common.Validation.Assert.IsNotNullOrEmpty(name, "name");

            if (this.Writers.ContainsKey(name))
            {
                var cache = this.Writers.Get(name);
                if (cache != null)
                    return cache;
            }

            // Create a new LogWriter and add it to cache.
            var writer = new LogWriter(name);
            this.Writers.Add(name, writer);
            return writer;
        }

        private static string GetApplicationName()
        {
            var assembly = Assembly.GetEntryAssembly();

            if (assembly == null)
            {
                var module_file_name = new StringBuilder(260);
                var size = NativeMethods.GetModuleFileName(NativeMethods.NullHandleRef, module_file_name, module_file_name.Capacity);
                if (size > 0)
                {
                    return Path.GetFileNameWithoutExtension(module_file_name.ToString());
                }
            }

            return Path.GetFileNameWithoutExtension(assembly.EscapedCodeBase);
        }

        public void Close()
        {
            foreach (var key in this.Writers.Keys)
            {
                this.Writers[key].Close();
            }
        }

        public void Flush()
        {
            foreach (var key in this.Writers.Keys)
            {
                this.Writers[key].Flush();
            }
        }

        public void Dispose()
        {
            foreach (var key in this.Writers.Keys)
            {
                this.Writers[key].Dispose();
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
