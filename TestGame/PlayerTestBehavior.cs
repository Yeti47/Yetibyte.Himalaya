using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Yetibyte.Himalaya.Collision;
using Yetibyte.Himalaya.GameElements;
using Yetibyte.Himalaya.Controls;
using System.Diagnostics;
using Yetibyte.Himalaya.Extensions;

namespace TestGame {

    public class PlayerTestBehavior : Behavior {

        private ControlListener _controlListener;
        private CollisionController _collisionController;

        public float Speed { get; set; } = 150f;

        public override void Awake() {

            _controlListener = GameEntity.GetComponent<ControlListener>();
            _collisionController = GameEntity.GetComponent<CollisionController>();

        }

        public override void Update(GameTime gameTime, float globalTimeScale) {

            float deltaTime = gameTime.DeltaTime();

            _collisionController.Velocity = new Vector2(_controlListener.GetAxisValue("Horizontal") * Speed * deltaTime, _controlListener.GetAxisValue("Vertical") * Speed * deltaTime);

        }

        protected override void OnTrigger(Collider ownCollider, Collider otherCollider) {
            
        }

        protected override void OnTriggerEnter(Collider ownCollider, Collider otherCollider) {
            
        }

        protected override void OnTriggerLeave(Collider ownCollider, Collider otherCollider) {
            
        }
    }

}
