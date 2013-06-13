using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
    /// <summary>
    /// A TextFileListener writes messages to a file.
    /// Provides a way to automatically archive log files.
    /// </summary>
    public class TextFileListener
        : TextWriterListener
    {
        #region Variables
        private const string _DefaultFileName = "fosol.diagnostics.log";
        private const string _DefaultArchiveFileName = "fosol.diagnostics.archive.log";
        
        private Fosol.Common.Formatters.StringFormatter _FileName;
        private bool _Append;
        private bool _CreateDirectory;
        private bool _KeepOpen;
        private long _ArchiveAboveSize;
        private Fosol.Common.Formatters.StringFormatter _ArchiveFileName;
        private int _MaxArchiveFiles;
        private bool _OverwriteArchive;

        /// <summary>
        /// Simple class to contain information about the file.
        /// </summary>
        protected class FileInformation
        {
            public string FullPath { get; private set; }
            public string Directory { get; private set; }
            public string FileName { get; private set; }
            public string Extension { get; private set; }
            public long Length { get; set; }
            public DateTime LastWriteTime { get; set; }

            /// <summary>
            /// Creates a new instance of a FileInformation.
            /// </summary>
            /// <param name="path">Path to the logging file.</param>
            public FileInformation(string path)
            {
                this.FullPath = System.IO.Path.GetFullPath(path);
                this.Directory = System.IO.Path.GetDirectoryName(this.FullPath);
                this.FileName = System.IO.Path.GetFileName(this.FullPath);
                this.Extension = System.IO.Path.GetExtension(this.FullPath);

                var file = new FileInfo(this.FullPath);
                if (file.Exists)
                {
                    this.Length = file.Length;
                    this.LastWriteTime = file.LastWriteTime;
                }
            }
        }
        private FileInformation _File;
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
                return _FileName;
            }
            set
            {
                _FileName = value;
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
                return _Append;
            }
            set
            {
                _Append = value;
            }
        }

        /// <summary>
        /// get/set - Controls whether the directory is created if it doesn't exist.
        /// </summary>
        [DefaultValue(true)]
        [TraceSetting("CreateDirectory")]
        public bool CreateDirectory
        {
            get
            {
                return _CreateDirectory;
            }
            set
            {
                _CreateDirectory = value;
            }
        }

        /// <summary>
        /// get/set - Controls whether the file will be kept open 'true', or if it will close after each write 'false'.
        /// </summary>
        [DefaultValue(true)]
        [TraceSetting("KeepOpen")]
        public bool KeepOpen
        {
            get { return _KeepOpen; }
            set { _KeepOpen = value; }
        }

        /// <summary>
        /// get/set - When a file exceeds the size in bytes defined it will archive it and create a new file.
        /// </summary>
        [DefaultValue(0)]
        [TraceSetting("ArchiveAboveSize")]
        public long ArchiveAboveSize
        {
            get
            {
                return _ArchiveAboveSize;
            }
            set
            {
                Fosol.Common.Validation.Assert.MinRange(value, 0, "ArchiveAboveSize");

                _ArchiveAboveSize = value;
            }
        }

        /// <summary>
        /// get/set - The file name to use when archiving.
        /// </summary>
        [DefaultValue(_DefaultArchiveFileName)]
        [TraceSetting("ArchiveFileName", typeof(Fosol.Common.Converters.StringFormatterConverter))]
        public Fosol.Common.Formatters.StringFormatter ArchiveFileName
        {
            get
            {
                return _ArchiveFileName;
            }
            set
            {
                _ArchiveFileName = value;
            }
        }

        /// <summary>
        /// get/set - The maximum number of archive files before the first one is deleted.
        /// </summary>
        [DefaultValue(7)]
        [TraceSetting("MaxArchiveFiles")]
        public int MaxArchiveFiles
        {
            get
            {
                return _MaxArchiveFiles;
            }
            set
            {
                Fosol.Common.Validation.Assert.MinRange(value, 0, "MaxArchiveFiles");

                _MaxArchiveFiles = value;
            }
        }

        /// <summary>
        /// get/set - Controls whether archive files should be overwritten if the new archive file has the same name.
        /// </summary>
        [DefaultValue(true)]
        [TraceSetting("OverwriteArchive")]
        public bool OverwriteArchive
        {
            get { return _OverwriteArchive; }
            set { _OverwriteArchive = value; }
        }

        /// <summary>
        /// get/set - Information about the file that the stream is writting to.
        /// </summary>
        protected FileInformation File
        {
            get { return _File; }
            set { _File = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TextFileListener.
        /// </summary>
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
        /// <param name="traceEvent">TraceEvent object being passed to the listener.</param>
        /// <returns>'True' if the Writer has been initialized.</returns>
        protected override bool OnBeforeWrite(TraceEvent traceEvent)
        {
            if (EnsureWriter())
                return base.OnBeforeWrite(traceEvent);
            return false;
        }

        /// <summary>
        /// Performs the task fo writing the data to the listener.
        /// Check if the file should be archived.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object being passed to the listener.</param>
        protected override void OnWrite(TraceEvent traceEvent)
        {
            var message = this.Render(traceEvent);

            // Check to see if the file needs to be archived.
            var length = this.Encoding.GetByteCount(message);

            if (this.ArchiveAboveSize > 0
                && (this.File.Length + length) >= this.ArchiveAboveSize)
            {
                try
                {
                    ArchiveFile();
                }
                catch
                {
                    // Ignore archiving errors.
                }
            }

            base.OnWrite(traceEvent);
        }

        /// <summary>
        /// Close the Writer after every write if KeepOpen = false.
        /// </summary>
        /// <param name="traceEvent">TraceEvent object being passed to the listener.</param>
        protected override void OnAfterWrite(TraceEvent traceEvent)
        {
            base.OnAfterWrite(traceEvent);

            if (!this.KeepOpen)
                this.Close();
        }

        /// <summary>
        /// Flush the buffer.
        /// Update the file length.
        /// </summary>
        public override void Flush()
        {
            if (this.File != null)
                this.File.Length += this.BufferUsed;
            base.Flush();
        }

        /// <summary>
        /// Close the TextWriter stream.
        /// Update the file information.
        /// </summary>
        public override void Close()
        {
            base.Close();

            if (this.File != null)
            {
                // Get the file information.
                var info = new FileInfo(this.File.FullPath);
                if (info.Exists)
                {
                    this.File.Length = info.Length;
                    this.File.LastWriteTime = info.LastWriteTime;
                }
            }
        }

        /// <summary>
        /// Ensure the writer has been initialized.
        /// </summary>
        /// <returns>'True' if the writer has been intialized successfully.</returns>
        private bool EnsureWriter()
        {
            var stream_writer = this.Writer as StreamWriter;
            if (this.Writer == null
                || (stream_writer != null
                && (stream_writer.BaseStream == null
                || !stream_writer.BaseStream.CanWrite)))
            {
                var generated_file_name = this.FileName.Render(null);
                if (string.IsNullOrEmpty(generated_file_name))
                    return false;

                this.File = new FileInformation(generated_file_name);

                if (!System.IO.Directory.Exists(this.File.Directory))
                {
                    if (!this.CreateDirectory)
                        return false;

                    // Create the directory.
                    System.IO.Directory.CreateDirectory(this.File.Directory);
                }

                try
                {
                    this.Writer = new StreamWriter(this.File.FullPath, this.Append, this.Encoding, this.BufferSize);
                    return true;
                }
                catch (IOException)
                {
                    return false;
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

            return true;
        }

        /// <summary>
        /// Archive the current file.
        /// It will attempt to automatically number the archive file name by checking prior archive files.
        /// If the archive file name is not numbered it will append a Guid instead.
        /// </summary>
        private void ArchiveFile()
        {
            var temp_file_name = this.ArchiveFileName.Render(null);

            // Do not archive if a file name has not be specified.
            if (string.IsNullOrEmpty(temp_file_name))
                return;

            var archive_path = System.IO.Path.GetFullPath(temp_file_name);
            var archive_directory = System.IO.Path.GetDirectoryName(archive_path);
            var archive_file_name = System.IO.Path.GetFileName(archive_path);
            var archive_file_ext = System.IO.Path.GetExtension(archive_file_name);
            var search_file_name = archive_file_name.Replace(archive_file_ext, ".*" + archive_file_ext);

            // Fetch all archived files.
            var files = (
                from f in new System.IO.DirectoryInfo(archive_directory).GetFiles(search_file_name, SearchOption.TopDirectoryOnly)
                orderby f.CreationTime
                select f).ToList();

            string generated_file_name = null;
            if (files != null && files.Count() > 0)
            {
                // Extract the last one and determine the number.
                var last_file_name = files.Last().Name;
                var wildcard = search_file_name.LastIndexOf("*");

                var value = last_file_name.Replace(search_file_name.Substring(0, wildcard), "").Replace(search_file_name.Substring(wildcard + 1, search_file_name.Length - wildcard - 1), "");
                var number = 0;
                if (int.TryParse(value, out number))
                    generated_file_name = archive_file_name.Replace(archive_file_ext, string.Format(".{0:000}{1}", number + 1, archive_file_ext));

                CleanArchive(files);
            }

            var full_path = System.IO.Path.GetFullPath(generated_file_name ?? archive_file_name.Replace(archive_file_ext, string.Format(".{0:000}{1}", 0, archive_file_ext)));
            var directory_path = System.IO.Path.GetDirectoryName(full_path);
            var file_name = System.IO.Path.GetFileName(full_path);

            var writer = this.Writer;
            if (writer != null)
            {
                writer.Flush();
                writer.Close();
            }
            
            // Check if the archive file exists, if it does and OverwriteArchive is 'true' delete it first.
            var archive_file = new FileInfo(full_path);
            if (this.OverwriteArchive && archive_file.Exists)
            {
                archive_file.Delete();
            }

            // Move the current log file into the archive.
            System.IO.File.Move(this.File.FullPath, full_path);

            EnsureWriter();
        }

        /// <summary>
        /// Deletes the archive files if they exceed the MasArchiveFiles number.
        /// It will delete in ascending order, so submit the files appropriately.
        /// </summary>
        /// <param name="files">List of FileInfo objects.</param>
        private void CleanArchive(List<FileInfo> files)
        {
            // Archive needs to be cleaned up.  Delete the earlier files.
            if (files.Count() >= this.MaxArchiveFiles)
            {
                for (int i = 0; i <= (files.Count() - this.MaxArchiveFiles); i++)
                {
                    try
                    {
                        System.IO.File.Delete(files[i].FullName);
                    }
                    catch
                    {
                        // Ignore error and move on.
                    }
                }
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
