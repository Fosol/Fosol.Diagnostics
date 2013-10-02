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
        /// <summary>
        /// get - The name of the log file (may also include path).
        /// </summary>
        public Format Filename
        {
            get { return _Filename; }
            private set { _Filename = value; }
        }

        /// <summary>
        /// get/set - The FileStream object used to create and write to the log file.
        /// </summary>
        private FileStream Stream
        {
            get { return _Stream; }
            set { _Stream = value; }
        }

        /// <summary>
        /// get/set - Whether The TextFileListener should create the directories that the file will be created in.
        /// </summary>
        [TraceSetting("CreateDirectory")]
        [DefaultValue(true)]
        public bool CreateDirectory
        {
            get { return _CreateDirectory; }
            set { _CreateDirectory = value; }
        }

        /// <summary>
        /// get/set - Whether the log file should be deleted every time the FileStream is initialized.
        /// </summary>
        [TraceSetting("DeleteFileOnStart")]
        public bool DeleteFileOnStart
        {
            get { return _DeleteFileOnStart; }
            set { _DeleteFileOnStart = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TextFileListener object.
        /// The default location of the log file is the same directory as the 
        /// </summary>
        /// <param name="filename">The name of the log file (may also include the path).</param>
        public TextFileListener(string filename)
        {
            Fosol.Common.Validation.Assert.IsNotNullOrEmpty(filename, "filename");

            var parser = new ElementParser();
            this.Filename = parser.Parse(filename);
        }

        /// <summary>
        /// Creates a new instance of a TextFileListener object.
        /// </summary>
        /// <param name="filename">The name of the log file (may also include the path).</param>
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
            var stream = new StreamWriter(this.Stream, this.Encoding, this.BufferSize);

            switch (this.AutoFlush)
            {
                case (AutoFlushOption.BufferFull):
                    stream.AutoFlush = true;
                    break;
                case (AutoFlushOption.EveryWrite):
                default:
                    stream.AutoFlush = false;
                    break;
            }
            this.Writer = stream;
        }

        /// <summary>
        /// Write the TraceEvent message to the TextWriter stream.
        /// </summary>
        /// <param name="trace">TraceEvent object.</param>
        protected override void OnWrite(TraceEvent trace)
        {
            if (this.Writer == null)
                return;

            var message = this.Render(trace);
            try
            {
                // The StreamWriter automatically does flushing, so we don't have to do it manually.
                if (this.AutoFlush == AutoFlushOption.BufferFull)
                    this.Writer.Write(message);
                else
                {
                    var length = this.Encoding.GetByteCount(message);

                    if (this.AutoFlush == AutoFlushOption.BufferFull
                        && this.BufferUsed + length >= this.BufferSize)
                        this.Flush();

                    this.Writer.Write(message);

                    this.BufferUsed += length;

                    if (this.AutoFlush == AutoFlushOption.EveryWrite)
                        this.Flush();
                }
            }
            catch (ObjectDisposedException)
            {

            }
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
