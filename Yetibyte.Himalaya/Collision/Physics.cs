using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yetibyte.Himalaya.GameElements;

namespace Yetibyte.Himalaya.Collision {

    public class Physics {

        // Fields

        private Scene _scene;

        // Properties

        public bool IgnoreGravity { get; set; }
        public float Gravity { get; set; } = 9f;

        // Constructor

        public Physics(Scene scene) {

            this._scene = scene;

        }

    }

}
