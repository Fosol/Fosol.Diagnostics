using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
    public class TextWriterListener
        : TraceListener
    {
        #region Variables
        private TextWriter _Writer;
        #endregion

        #region Properties
        public TextWriter Writer
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
            this.Writer = new StreamWriter(stream);
        }

        public TextWriterListener(TextWriter writer)
        {
            this.Writer = writer;
        }
        #endregion

        #region Methods
        public override void Write(string message)
        {
            this.Writer.Write(message);
        }

        public override void Close()
        {
            if (this.Writer != null)
                this.Writer.Close();
        }

        public override void Flush()
        {
            if (this.Writer != null)
                this.Writer.Flush();
        }

        public override void Dispose()
        {
            if (this.Writer != null)
            {
                Close();
                this.Writer = null;
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
