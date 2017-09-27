using System;
using System.Collections.Generic;
using System.Text;

namespace Crab.DataModel.Data
{
    /// <summary>
    /// The interface for entity
    /// </summary>
    public interface IExtensibleEntity
    {
        /// <summary>
        /// Gets or sets the key of the entity
        /// </summary>
        EntityKey Key { get; set; }
    }
}
