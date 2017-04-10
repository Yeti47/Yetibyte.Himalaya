using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yetibyte.Himalaya.GameElements;

namespace Yetibyte.Himalaya.GameElements {

    public abstract class EntityComponent {

        // Properties

        public virtual bool AllowMultiple => false;
        public GameEntity GameEntity { get; set; }

        // Constructor
        
        // Methods

    }
}
