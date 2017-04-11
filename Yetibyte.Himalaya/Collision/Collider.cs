using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yetibyte.Himalaya.GameElements;
using Microsoft.Xna.Framework;

namespace Yetibyte.Himalaya.Collision {

    public abstract class Collider : EntityComponent {

        // Fields



        // Properties

        public override bool AllowMultiple => true;
        public abstract Rectangle Bounds { get; set; }

        // Constructor


        // Methods

    }

}
