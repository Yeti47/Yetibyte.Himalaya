using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yetibyte.Himalaya;
using Yetibyte.Himalaya.GameElements;
using Yetibyte.Himalaya.Graphics;
using Yetibyte.Himalaya.Procedural;
using Yetibyte.Himalaya.Controls;

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
            controlSettings.ControlMap.Add("Left", new GameControl { Key = Keys.Left, AlternativeKey = Keys.A });
            controlSettings.ControlMap.Add("Right", new GameControl { Key = Keys.Right, AlternativeKey = Keys.D });
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

            GunTexture = Content.Load<Texture2D>("gunHands1");
            Sprite gunSprite = new Sprite(GunTexture);

            CurrentScene = new TestScene1(this);
            CurrentScene.Initialize();

            Player player = new Player("player", new Vector2(50, 50), playerSprite);
            player.DrawOrder = -1;
            CurrentScene.AddGameEntity(player);

            Gun gun = new Gun("playerGun", player.Transform.Position, gunSprite);
            gun.DrawOrder = 0;

            player.AddChildEntity(gun);

            player.Transform.LocalScale = new Vector2(4f, 4f);

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
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            CurrentScene.Draw(spriteBatch, gameTime);

            spriteBatch.End();

            base.Draw(gameTime);
        }
                
    }

}
