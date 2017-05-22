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
using Yetibyte.Himalaya.Debugging;
using Yetibyte.Utilities;
using Yetibyte.Utilities.Extensions;

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

            ControlSettings controlSettings = new ControlSettings();
            controlSettings.RegisterControlAxis("Horizontal", AxisDirection.Horizontal, Keys.Right, Keys.Left, 0, 0, Keys.D, Keys.A, 0, 0, GamePadAxes.LeftThumbstick);
            controlSettings.RegisterControlAxis("Vertical", AxisDirection.Vertical, Keys.Down, Keys.Up, 0, 0, Keys.S, Keys.W, 0, 0, GamePadAxes.LeftThumbstick);

            Player player = new Player("player", new Vector2(0, 0));
            player.DrawOrder = -1;
            player.AddComponent(playerSprite);
            player.AddComponent(new ControlListener(PlayerIndex.One, controlSettings));
            CurrentScene.AddGameEntity(player);

            Gun gun = new Gun("playerGun", player.Transform.Position);
            gun.DrawOrder = 0;
            gun.Transform.X -= 7;
            gun.Transform.Y += 10;
            gun.AddComponent(gunSprite);
            
            player.AddChildEntity(gun);

            player.Transform.LocalScale = new Vector2(4f, 4f);

            // vvvvv Collider-Test vvvvvvv

            player.AddComponent(new RectCollider() { Width = 16, Height = 16 });
            player.AddComponent(new CollisionController());

            EmptyTestEntity testEntity = new EmptyTestEntity("testEntity", new Vector2(-80, 0));
            CurrentScene.AddGameEntity(testEntity);
            testEntity.AddComponent(new RectCollider() { Width = 20, Height = 20 });
            testEntity.Transform.LocalScale = new Vector2(4, 4);

            EmptyTestEntity testEntityB = new EmptyTestEntity("testEntity", new Vector2(250, 60));
            CurrentScene.AddGameEntity(testEntityB);
            testEntityB.AddComponent(new RectCollider() { Width = 25, Height = 60 });
            testEntityB.Transform.LocalScale = new Vector2(4, 4);

            // vvvvv LineSegment-Test vvvvvvv

            LineSegment lineSeg = new LineSegment(-5, 2, 5, 2);
            Vector2 point = new Vector2(3, 2);

            Debug.WriteLine("These are the line segment's bounds: " + lineSeg.Bounds);
            Debug.WriteLine("The line segment's slope is: " + lineSeg.Slope);
            Debug.WriteLine("The point is on the line segment: " + LineSegment.IsPointOnLineSegment(point, lineSeg));

            int[] randomNumbers = CollectionUtil.GenerateUniqueIntegers(6, 1, 49, true);

            Debug.WriteLine("The random numbers are: ");

            for (int i = 0; i < randomNumbers.Length; i++) {

                Debug.Write(randomNumbers[i] + " ");

            }

            randomNumbers.Shuffle();

            Debug.WriteLine("\nAfter shuffling the random numbers are: ");

            for (int i = 0; i < randomNumbers.Length; i++) {

                Debug.Write(randomNumbers[i] + " ");

            }

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

            //DebugUtility.DrawLine(spriteBatch, new LineSegment(5, 5, 30, 20), Color.Red, 2);
            //DebugUtility.DrawRectangle(spriteBatch, new RectangleF(20, 20, 100, 80), Color.Green);
            //DebugUtility.DrawFilledRectangle(spriteBatch, new RectangleF(40, 130, 60, 100), Color.Cyan);

            spriteBatch.End();

            base.Draw(gameTime);
        }
                
    }

}
