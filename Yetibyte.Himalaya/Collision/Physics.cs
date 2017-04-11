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

        // Methods

        /// <summary>
        /// Enumerates all the <see cref="Collider"/>s attached to GameEntities in this <see cref="Scene"/> that are active and not destroyed.
        /// </summary>
        /// <returns>A collection of all Colliders attached to GameEntities that live in the current Scene.</returns>
        public IEnumerable<Collider> GetCollidersInActiveEntities() => _scene.LiveGameEntities.SelectMany(e => e.GetComponents<Collider>());

        /// <summary>
        /// Enumerates all the <see cref="Collider"/>s that match the given base Type and are
        /// attached to active GameEntities in this <see cref="Scene"/>.
        /// </summary>
        /// <typeparam name="T">The type to match the colliders with.</typeparam>
        /// <returns>A collection of all Colliders that match the given Type and are attached to GameEntities that live in the current Scene.</returns>
        public IEnumerable<T> GetCollidersInActiveEntities<T>() where T : EntityComponent => _scene.LiveGameEntities.SelectMany(e => e.GetComponents<T>());

    }

}
