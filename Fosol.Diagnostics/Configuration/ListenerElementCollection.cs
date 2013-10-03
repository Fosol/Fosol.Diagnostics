using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Configuration
{
    /// <summary>
    /// A ListenerElementCollection contains a collection of ListenerElement configuration details.
    /// </summary>
    [System.Configuration.ConfigurationCollection(
        typeof(ListenerElement),
        CollectionType = System.Configuration.ConfigurationElementCollectionType.AddRemoveClearMap,
        AddItemName = "add",
        RemoveItemName = "remove",
        ClearItemsName = "clear")]
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

        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
