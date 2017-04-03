using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yetibyte.Himalaya.GameElements;

namespace Yetibyte.Himalaya.Collision {

    public abstract class Collider {

        // Fields

        private CollisionController _controller;

        // Properties

        public GameEntity GameEntity => _controller?.GameEntity;

        // Constructor

        public Collider(CollisionController controller) {

            this._controller = controller;

        }

        // Methods

    }

}
