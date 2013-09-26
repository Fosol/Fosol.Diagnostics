using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Configuration
{
    [System.Configuration.ConfigurationCollection(
        typeof(FilterElement), 
        CollectionType = System.Configuration.ConfigurationElementCollectionType.AddRemoveClearMap,
        AddItemName = "add",
        RemoveItemName = "remove",
        ClearItemsName = "clear")]
    internal class FilterElementCollection
        : Fosol.Common.Configuration.ConfigurationElementCollection<FilterElement>
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
