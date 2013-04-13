﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Diagnostics.Listeners
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class TraceListenerAttributeAttribute
        : Attribute
    {
        #region Variables
        #endregion

        #region Properties
        public string Name { get; private set; }
        #endregion

        #region Constructors
        public TraceListenerAttributeAttribute(string name)
        {
            this.Name = name;
        }
        #endregion

        #region Methods

        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}