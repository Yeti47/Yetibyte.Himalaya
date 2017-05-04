using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Yetibyte.Himalaya;
using Yetibyte.Himalaya.GameElements;
using Yetibyte.Himalaya.Graphics;
using Yetibyte.Himalaya.Controls;
using Yetibyte.Himalaya.Extensions;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Yetibyte.Himalaya.Debugging;
using Yetibyte.Himalaya.Collision;

namespace TestGame {

    public class Player : Actor {

        private ControlListener _controlListener;
        private CollisionController _collisionController;

        public float Speed { get; set; } = 150f;

        // Constructor

        public Player(string name, Vector2 position, Sprite sprite, int renderLayer = 1, SpriteAnimator animator = null) : base(name, position, sprite, renderLayer, animator) {
            
        }

        // Methods

        public override void Initialize() {
            base.Initialize();

        }

        public override void Awake() {
            base.Awake();

            _controlListener = Scene.GetGame<Game1>().ControlListenerPlayer1;
            _collisionController = GetComponent<CollisionController>();

        }

        public override void Update(GameTime gameTime, float globalTimeScale) {
            base.Update(gameTime, globalTimeScale);

            float deltaTime = gameTime.DeltaTime();

            //Transform.Position += new Vector2(_controlListener.GetAxisValue("Horizontal") * Speed * deltaTime, 0f);
            _collisionController.Move(new Vector2(_controlListener.GetAxisValue("Horizontal") * Speed * deltaTime, _controlListener.GetAxisValue("Vertical") * Speed * deltaTime));



        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
            base.Draw(spriteBatch, gameTime);

            DebugUtility.VisualizeColliders(spriteBatch, Scene, Color.Red);

            //LineSegment lineA = new LineSegment(new Vector2(30, -200), new Vector2(30, -100));
            //LineSegment lineB = new LineSegment(new Vector2(10, -140), new Vector2(80, -140));

            //DebugUtility.DrawLine(spriteBatch, lineA, Color.Green);
            //DebugUtility.DrawLine(spriteBatch, lineB, Color.Orange);

            //bool intersect = LineSegment.Intersect(lineA, lineB, out Vector2 intersection);

            //Debug.WriteLine("LineA and LineB intersect: " + intersect + ". Intersection point: " + intersection);
            
        }

    }

}
