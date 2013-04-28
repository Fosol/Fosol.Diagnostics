using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Keywords
{
    /// <summary>
    /// Thread identity information (name and authentication information).
    /// </summary>
    [TraceKeyword("identity")]
    public sealed class IdentityKeyword
        : DynamicKeyword
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - Value to separate identity values.
        /// </summary>
        [TraceKeywordProperty("delimiter")]
        public string Delimiter { get; set; }

        /// <summary>
        /// get/set - Whether to include the name.
        /// </summary>
        [TraceKeywordProperty("name")]
        public bool ShowName { get; set; }

        /// <summary>
        /// get/set - Whether to include the authentication type.
        /// </summary>
        [TraceKeywordProperty("type")]
        public bool ShowAuthType { get; set; }

        /// <summary>
        /// get/set - Whether to include the is authenticated value.
        /// </summary>
        [TraceKeywordProperty("auth")]
        public bool ShowIsAuthenticated { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a IdentityKeyword object.
        /// </summary>
        /// <param name="attributes">StringDictionary object.</param>
        public IdentityKeyword(StringDictionary attributes)
            : base(attributes)
        {
            var show_name = true;
            var show_authtype = true;
            var show_is_auth = true;

            bool.TryParse(this.Attributes["name"], out show_name);
            bool.TryParse(this.Attributes["type"], out show_authtype);
            bool.TryParse(this.Attributes["auth"], out show_is_auth);

            this.Delimiter = this.Attributes["delimiter"] ?? ":";
            this.ShowName = show_name;
            this.ShowAuthType = show_authtype;
            this.ShowIsAuthenticated = show_is_auth;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns Thread identity information (name and authentication information).
        /// </summary>
        /// <param name="traceEvent">TraceEvent object.</param>
        /// <returns>Thread identity information (name and authentication information).</returns>
        public override string Render(TraceEvent traceEvent)
        {
            var principal = System.Threading.Thread.CurrentPrincipal;

            if (principal != null)
            {
                var identity = principal.Identity;
                if (identity != null)
                {
                    var builder = new StringBuilder(string.Empty);

                    if (this.ShowIsAuthenticated)
                    {
                        if (identity.IsAuthenticated)
                            builder.Append("auth");
                        else
                            builder.Append("notauth");
                    }

                    if (this.ShowAuthType)
                    {
                        builder.Append(this.Delimiter);
                        builder.Append(identity.AuthenticationType);
                    }

                    if (this.ShowName)
                    {
                        builder.Append(this.Delimiter);
                        builder.Append(identity.Name);
                    }

                    return builder.ToString();
                }
            }

            return null;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
