using Fosol.Common.Parsers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Listeners
{
    /// <summary>
    /// The TextFileListener outputs TraceEvent messages to a text file.
    /// </summary>
    public class TextFileListener
        : TextWriterListener
    {
        #region Variables
        private Format _Filename;
        private FileStream _Stream;
        private bool _DeleteFileOnStart;
        private bool _CreateDirectory;
        #endregion

        #region Properties
        public Format Filename
        {
            get { return _Filename; }
            private set { _Filename = value; }
        }

        private FileStream Stream
        {
            get { return _Stream; }
            set { _Stream = value; }
        }

        [TraceSetting("CreateDirectory")]
        [DefaultValue(true)]
        public bool CreateDirectory
        {
            get { return _CreateDirectory; }
            set { _CreateDirectory = value; }
        }

        [TraceSetting("DeleteFileOnStart")]
        public bool DeleteFileOnStart
        {
            get { return _DeleteFileOnStart; }
            set { _DeleteFileOnStart = value; }
        }
        #endregion

        #region Constructors
        public TextFileListener(string filename)
        {
            Fosol.Common.Validation.Assert.IsNotNullOrEmpty(filename, "filename");

            var parser = new ElementParser();
            this.Filename = parser.Parse(filename);
        }

        [TraceSetting("Filename", typeof(Fosol.Common.Parsers.Converters.FormatConverter))]
        public TextFileListener(Format filename)
            : base()
        {
            Fosol.Common.Validation.Assert.IsNotNull(filename, "filename");
            this.Filename = filename;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Ensure the file has been created and open it.
        /// </summary>
        /// <param name="trace">TraceEvent object.</param>
        /// <returns>True if the TraceEvent should be sent to the TraceListeners.</returns>
        protected override bool OnBeforeWrite(TraceEvent trace)
        {
            if (this.Writer == null)
            {
                InitializeWriter(trace);
            }
            return base.OnBeforeWrite(trace);
        }

        /// <summary>
        /// Create and/or open the file that will be written to.
        /// </summary>
        /// <param name="trace">TraceEvent object.</param>
        private void InitializeWriter(TraceEvent trace)
        {
            var filename = this.Filename.Render(trace);

            // Delete the file if configured to.
            if (this.DeleteFileOnStart)
            {
                if (File.Exists(filename))
                    File.Delete(filename);
            }

            // Only create the directory path to the log file if configured to.
            if (this.CreateDirectory)
            {
                var path = Path.GetDirectoryName(filename);

                if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
                    Directory.CreateDirectory(path);

            }

            // Create or open the file at the specified path.
            this.Stream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            this.Stream.Position = this.Stream.Length;
            this.Writer = new StreamWriter(this.Stream, this.Encoding);
        }

        /// <summary>
        /// Close the FileStream.
        /// </summary>
        public override void Close()
        {
            base.Close();

            if (this.Stream != null)
            {
                this.Stream.Close();
                this.Stream = null;
            }
        }

        /// <summary>
        /// Dispose the FileStream.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            if (this.Stream != null)
            {
                Close();
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
