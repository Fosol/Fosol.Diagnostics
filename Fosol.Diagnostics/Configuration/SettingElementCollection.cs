using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Diagnostics.Configuration
{
    /// <summary>
    /// SettingElementCollection is a collection of SettingElement objects.
    /// </summary>
    [System.Configuration.ConfigurationCollection(
        typeof(SettingElement),
        CollectionType = System.Configuration.ConfigurationElementCollectionType.AddRemoveClearMap,
        AddItemName = "set",
        RemoveItemName = "remove",
        ClearItemsName = "clear")]
    internal class SettingElementCollection
        : Fosol.Common.Configuration.ConfigurationElementCollection<SettingElement>
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
