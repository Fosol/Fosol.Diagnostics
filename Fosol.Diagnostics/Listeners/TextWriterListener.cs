using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
    /// <summary>
    /// Directs trace events to the stream specified.
    /// </summary>
    public class TextWriterListener
        : TraceListenerBase
    {
        #region Variables
        private TextWriter _Writer;
        #endregion

        #region Properties
        internal TextWriter Writer
        {
            get { return _Writer; }
            set { _Writer = value; }
        }
        #endregion

        #region Constructors
        public TextWriterListener()
        {

        }

        public TextWriterListener(Stream stream)
        {
            Fosol.Common.Validation.Assert.IsNotNull(stream, "stream");
            this.Writer = new StreamWriter(stream);
        }

        public TextWriterListener(TextWriter writer)
        {
            Fosol.Common.Validation.Assert.IsNotNull(writer, "writer");
            this.Writer = writer;
        }
        #endregion

        #region Methods
        public override void Close()
        {
            if (_Writer != null)
            {
                try
                {
                    _Writer.Close();
                }
                catch (ObjectDisposedException)
                {
                }
            }

            _Writer = null;
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                    this.Close();
                else
                {
                    if (_Writer != null)
                    {
                        try
                        {
                            _Writer.Close();
                        }
                        catch (ObjectDisposedException)
                        {
                        }
                    }
                    _Writer = null;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        public override void Flush()
        {
            try
            {
                _Writer.Flush();
            }
            catch (ObjectDisposedException)
            {
            }
        }

        public override void Write(string message)
        {
            if (NeedIndent) WriteIndent();
            try
            {
                _Writer.Write(message);
            }
            catch (ObjectDisposedException)
            {
            }
        }

        public override void WriteLine(string message)
        {
            if (NeedIndent) WriteIndent();
            try
            {
                _Writer.WriteLine(message);
            }
            catch (ObjectDisposedException)
            {
            }
        }

        private static Encoding GetEncodingWithFallback(Encoding encoding)
        {
            var fallback_encoding = (Encoding)encoding.Clone();
            fallback_encoding.EncoderFallback = EncoderFallback.ReplacementFallback;
            fallback_encoding.DecoderFallback = DecoderFallback.ReplacementFallback;

            return fallback_encoding;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
