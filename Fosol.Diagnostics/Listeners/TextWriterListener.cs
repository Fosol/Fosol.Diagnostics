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
        : TraceListener
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

        public override void Dispose()
        {
            if (_Writer != null)
                _Writer.Close();
            _Writer = null;
        }

        public override void Flush()
        {
            _Writer.Flush();
        }

        public override void Write(string trace)
        {
            _Writer.Write(trace);
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
