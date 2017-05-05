using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yetibyte.Himalaya.GameElements {

    public class ComponentEventArgs : EventArgs {
        
        public EntityComponent Component { get; }

        public ComponentEventArgs(EntityComponent component) {

            this.Component = component;

        }

    }

}
