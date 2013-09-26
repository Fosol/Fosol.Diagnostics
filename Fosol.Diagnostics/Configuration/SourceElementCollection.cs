using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Configuration
{
    /// <summary>
    /// SourceElementCollection is a collection of SourceElement objects.
    /// </summary>
    [System.Configuration.ConfigurationCollection(
        typeof(SourceElement),
        CollectionType = System.Configuration.ConfigurationElementCollectionType.AddRemoveClearMap,
        AddItemName = "add",
        RemoveItemName = "remove",
        ClearItemsName = "clear")]
    internal class SourceElementCollection
        : Fosol.Common.Configuration.ConfigurationElementCollection<SourceElement>
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
