using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yetibyte.Himalaya;
using Yetibyte.Himalaya.GameElements;
using Yetibyte.Himalaya.Graphics;
using Yetibyte.Himalaya.Procedural;
using Yetibyte.Himalaya.Controls;
using Yetibyte.Himalaya.Collision;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestGame {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        KeyboardState previousKeyboardState;

        // ----

        public Scene CurrentScene { get; private set; }

        public Texture2D PlayerTexture { get; set; }
        public Texture2D GunTexture { get; set; }

        public ControlListener ControlListenerPlayer1 { get; private set; }

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here

            ControlSettings controlSettings = new ControlSettings();
            controlSettings.ControlAxesMap.Add("Horizontal", new GameControlAxis { PositiveKey = Keys.Right, AlternativePositiveKey = Keys.D, NegativeKey = Keys.Left, AlternativeNegativeKey = Keys.A, GamePadAxis = GamePadAxes.LeftThumbstick});
            controlSettings.ControlMap.Add("Up", new GameControl { Key = Keys.Up, AlternativeKey = Keys.W });
            controlSettings.ControlMap.Add("Down", new GameControl { Key = Keys.Down, AlternativeKey = Keys.S });

            ControlListenerPlayer1 = new ControlListener(PlayerIndex.One, controlSettings);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            PlayerTexture = Content.Load<Texture2D>("axeGun1");
            Sprite playerSprite = new Sprite(PlayerTexture, 0, 0, 16, 16);
            playerSprite.SetOrigin(Sprite.OriginPoint.Center);

            GunTexture = Content.Load<Texture2D>("gunHands1");
            Sprite gunSprite = new Sprite(GunTexture);
            gunSprite.SetOrigin(Sprite.OriginPoint.Center);

            CurrentScene = new TestScene1(this);
            CurrentScene.LoadContent();
            CurrentScene.Initialize();

            Player player = new Player("player", new Vector2(0, 0), playerSprite);
            player.DrawOrder = -1;
            CurrentScene.AddGameEntity(player);

            Gun gun = new Gun("playerGun", player.Transform.Position, gunSprite);
            gun.DrawOrder = 0;
            gun.Transform.X -= 7;
            gun.Transform.Y += 10;
            
            player.AddChildEntity(gun);

            player.Transform.LocalScale = new Vector2(4f, 4f);

            /* CollisionController Testing
            EmptyTestEntity testEntityA = new EmptyTestEntity("child 1 of gun", Vector2.Zero);
            gun.AddChildEntity(testEntityA);

            EmptyTestEntity testEntityB = new EmptyTestEntity("child 2 of gun", Vector2.Zero);
            gun.AddChildEntity(testEntityB);

            EmptyTestEntity testEntityC = new EmptyTestEntity("another child of player", Vector2.Zero);
            player.AddChildEntity(testEntityC);

            EmptyTestEntity testEntityD = new EmptyTestEntity("testEntity D as a child of testEntityB", Vector2.Zero);
            testEntityB.AddChildEntity(testEntityD);

            List<CollisionController> childCollisionControllers = player.CollisionController.GetAllChildControllers(false);

            Debug.WriteLine("I found child collision controllers in the following entities:");

            foreach (var collisionController in childCollisionControllers) {

                Debug.WriteLine(collisionController.GameEntity.Name);

            }
            */

            // vvvvv ParentChildHierarchy-Test vvvvvvv

            PCTest testParent = new PCTest() { TestProperty = "I am the ancestor of everyone!" };
            PCTest testChildA = new PCTest();
            PCTest testChildB = new PCTest();
            PCTest testSubChild = new PCTest { TestProperty = "I am Test Sub Child" };

            testParent.AddChild(testChildA);
            testParent.AddChild(testChildB);
            testChildA.AddChild(testSubChild);

            Debug.WriteLine(testParent.Children[0].Children[0].TestProperty);
            Debug.WriteLine(testSubChild.GetAncestor().TestProperty);

            testChildB.AddChild(testSubChild);

            Debug.WriteLine("After changing Sub Test Child's parent from Test Child A to Test Child B:");
            Debug.WriteLine(testParent.Children[1].Children[0].TestProperty);

            // vvvvv LineSegment-Test vvvvvvv

            LineSegment lineSeg = new LineSegment(-5, 2, 5, 2);
            Vector2 point = new Vector2(3, 2);

            Debug.WriteLine("These are the line segment's bounds: " + lineSeg.Bounds);
            Debug.WriteLine("The line segment's slope is: " + lineSeg.Slope);
            Debug.WriteLine("The point is on the line segment: " + LineSegment.IsPointOnLineSegment(point, lineSeg));

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            
            KeyboardState currentKeyboardState = Keyboard.GetState();
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            ControlListenerPlayer1.Update(gameTime);
            CurrentScene.Update(gameTime);

            previousKeyboardState = currentKeyboardState;

            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            CurrentScene.Draw(gameTime);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
                
    }

}
