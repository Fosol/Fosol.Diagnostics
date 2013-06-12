﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
    public class DatabaseListener
        : TraceListener
    {
        #region Variables
        private string _ProviderName;
        private string _ConnectionString;
        private bool _KeepOpen;
        private DbConnection _ActiveConnection;
        private TraceFormatter _CommandText;
        private int _CommandTimeout;
        private System.Data.CommandType _CommandType;
        private System.Data.IDbCommand _Command;
        private DbProviderFactory _ProviderFactory;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - The Db provider invariant name.
        /// </summary>
        [TraceSetting("ProviderName", true)]
        public string ProviderName
        {
            get { return _ProviderName; }
            set { _ProviderName = value; }
        }

        /// <summary>
        /// get/set - ConnectionString or the name of the ConnectionString.
        /// </summary>
        [TraceSetting("ConnectionString")]
        public string ConnectionString
        {
            get { return _ConnectionString; }
            set { _ConnectionString = value; }
        }

        /// <summary>
        /// get/set - Controls whether the connection is left open.
        /// </summary>
        [DefaultValue(true)]
        [TraceSetting("KeepOpen")]
        public bool KeepOpen
        {
            get { return _KeepOpen; }
            set { _KeepOpen = value; }
        }

        /// <summary>
        /// get/set - The command that will be executed against the database.
        /// </summary>
        [TraceSetting("CommandText", true, typeof(Converters.TraceFormatterConverter))]
        public TraceFormatter CommandText
        {
            get { return _CommandText; }
            set { _CommandText = value; }
        }

        /// <summary>
        /// get/set - The length of time in seconds before a command times out.
        /// 0 means not timeout.
        /// </summary>
        [TraceSetting("CommandTimeout")]
        public int CommandTimeout
        {
            get { return _CommandTimeout; }
            set { _CommandTimeout = value; }
        }

        /// <summary>
        /// get/set - The type of command that will be executed.
        /// </summary>
        [DefaultValue(System.Data.CommandType.Text)]
        [TraceSetting("CommandType", typeof(EnumConverter), typeof(System.Data.CommandType))]
        public System.Data.CommandType CommandType
        {
            get { return _CommandType; }
            set { _CommandType = value; }
        }
        #endregion

        #region Constructors

        #endregion

        #region Methods
        public override void Initialize()
        {
        }

        protected override bool OnBeforeWrite(TraceEvent traceEvent)
        {
            OpenConnection();
            return base.OnBeforeWrite(traceEvent);
        }

        protected override void OnWrite(TraceEvent traceEvent)
        {
            if (_ProviderFactory.CanCreateDataSourceEnumerator)
            {
                var command_text = this.CommandText.Render(traceEvent);
                _Command = _ProviderFactory.CreateCommand();
                _Command.CommandText = command_text;
                _Command.Connection = _ActiveConnection;
                _Command.CommandTimeout = this.CommandTimeout;
                _Command.CommandType = this.CommandType;

                foreach (var param in this.CommandText.Keywords.OfType<Keywords.ParameterKeyword>())
                {
                    var value = param.Value.Render(traceEvent);
                    System.Data.SqlClient.SqlParameter parameter = new System.Data.SqlClient.SqlParameter(param.ParameterName, value);
                    _Command.Parameters.Add(parameter);
                }
                _Command.ExecuteNonQuery();
            }
        }

        protected override void OnAfterWrite(TraceEvent traceEvent)
        {
            base.OnAfterWrite(traceEvent);

            if (!this.KeepOpen 
                && _ActiveConnection.State != System.Data.ConnectionState.Closed)
                _ActiveConnection.Close();
        }

        protected void OpenConnection()
        {
            if (_ActiveConnection == null)
            {
                _ProviderFactory = DbProviderFactories.GetFactory(this.ProviderName);
                if (_ProviderFactory.CanCreateDataSourceEnumerator)
                {
                    var cs = System.Configuration.ConfigurationManager.ConnectionStrings[this.ConnectionString] ?? new System.Configuration.ConnectionStringSettings("default", this.ConnectionString);
                    _ActiveConnection = _ProviderFactory.CreateConnection();
                    _ActiveConnection.ConnectionString = cs.ConnectionString;
                    _ActiveConnection.Open();
                }
            }
            else if (_ActiveConnection.State == System.Data.ConnectionState.Closed)
            {
                _ActiveConnection.Open();
            }
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}