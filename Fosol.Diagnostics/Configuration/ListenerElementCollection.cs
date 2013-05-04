using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Configuration
{
    [ConfigurationCollection(typeof(ListenerElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    internal class ListenerElementCollection
        : Fosol.Common.Configuration.ConfigurationElementCollection<ListenerElement>
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        internal void InitializeDefaultInternal()
        {
            this.BaseAdd(new ListenerElement()
            {
                Name = "Default",
                TypeName = typeof(Listeners.DefaultListener).FullName
            });
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
