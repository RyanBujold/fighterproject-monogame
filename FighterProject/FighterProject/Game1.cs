using FighterProject.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace FighterProject {
    public class Game1 : Game {
        /* Tutorials used
         * - https://www.youtube.com/watch?v=-5ELPrIJNvA&ab_channel=Brent%27sTechTutorials
         * - https://www.youtube.com/watch?v=dNQg3RnK3Po&ab_channel=Brent%27sTechTutorials
         * - https://www.youtube.com/watch?v=dU_QnaGtNOk&t=6s&ab_channel=Brent%27sTechTutorials
         * - https://rareelementgames.wordpress.com/2017/04/21/game-state-management/
         */

        /* TODO LIST 
         * - Make art assets.
         */

        /* BACKLOG
         * - Add complex hitboxes/hurtboxes.
         * - Clean up pipeline ressources.
         * - Make it so that when a player walks into another, it
         *   pushes the idle player away.
         * - Modify how jump arcs function?
         * - Add complexity to fireballs (appear, collide).
         * - Add a camera and scorlling backgrounds.
         * - Add hitbox priority.
         * - Give throws a hitbox instead of checking distance between players.
         * - Modify how throwing/second animation logic works with characters with different sprites.
         * - Add fade in and fade out between rounds.
         */

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D renderTarget;
        private static readonly int renderWidth = 1920;
        private static readonly int renderHeight = 1080;

        public float scale = 0.44444f;
        public float timepassed;
        public static int Floor = 900;
        public static int Left_Bound = 0;
        public static int Right_Bound = 1920;
        public static int Top_Bound = 0;
        public static int Bottom_Bound = 1080;
        public static Vector2 Center_Bound { 
            get {
                return new Vector2(Right_Bound / 2, Bottom_Bound / 2);
            } 
        }


        // TEST SPRITE
        public Texture2D testSprite;

        // Keyboard Debug
        private KeyboardState _debugKeyboardState = new KeyboardState();

        // Fullscreen toggle
        private bool _fullScreenPressed = false;
        private Keys _fullScreenKey = Keys.F11;

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0f);

            // updates with the specified framerate 
            IsFixedTimeStep = true;

            base.Initialize();

            // 720p window
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            // Fullscreen
            /*_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.HardwareModeSwitch = true;
            _graphics.IsFullScreen = true;*/

            _graphics.ApplyChanges();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            // Make sure content is defined first in order for add state to load the state's content. 
            GameStateManager.Instance.SetContent(Content);
            GameStateManager.Instance.AddState(new Battle_GameState(GraphicsDevice));

            renderTarget = new RenderTarget2D(GraphicsDevice, renderWidth, renderHeight);
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            GameStateManager.Instance.Update(gameTime);

            // Fullscreen Toggle
            _debugKeyboardState = Keyboard.GetState();
            if (_debugKeyboardState.IsKeyDown(_fullScreenKey) && !_fullScreenPressed) {
                if (!_graphics.IsFullScreen) {
                    // Make Fullscreen
                    _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                    _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                    _graphics.IsFullScreen = true;
                }
                else {
                    // Make window
                    _graphics.PreferredBackBufferWidth = 1280;
                    _graphics.PreferredBackBufferHeight = 720;
                    _graphics.IsFullScreen = false;
                }
                _graphics.ApplyChanges();
                _fullScreenPressed = true;
            }
            else if (_debugKeyboardState.IsKeyUp(_fullScreenKey)) {
                _fullScreenPressed = false;
            }

            // Debug state
            if (_debugKeyboardState.IsKeyDown(Keys.F2)) {
                GameStateManager.Instance.ChangeStates(new Debug_GameState(GraphicsDevice));
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            scale = 1f / (1080f / _graphics.GraphicsDevice.Viewport.Height);

            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            GameStateManager.Instance.Draw(_spriteBatch);

            // Scales and adjusts what is rendered to window
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
