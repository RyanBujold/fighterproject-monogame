using FighterProject.Entities.Characters.ShadowManCharacter;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FighterProject.GameStates {
    class Debug_GameState : GameState {

        private readonly float _scale = 1f;
        private Texture2D _testCharSprite;
        private Texture2D _testSprite;
        private SpriteFont _debugFont;

        private int _x = 0;
        private int _y = 0;
        private int _width = 100;
        private int _height = 100;

        public Debug_GameState(GraphicsDevice graphicsDevice) : base(graphicsDevice) { }

        public override void Initialize() {
            
        }

        public override void LoadContent(ContentManager content) {
            _testCharSprite = content.Load<Texture2D>(ShadowMan.SpriteSheet);
            _debugFont = content.Load<SpriteFont>("fonts/default");

            // TEST SPRITE
            _testSprite = new Texture2D(_graphicsDevice, 1, 1);
            _testSprite.SetData(new[] { Color.White });
        }

        public override void UnloadContent() {
            
        }

        public override void Update(GameTime gameTime) {
            KeyboardState _keyboardState = Keyboard.GetState();

            if (_keyboardState.IsKeyDown(Keys.Right))
                _x++;
            if (_keyboardState.IsKeyDown(Keys.Down))
                _y++;
            if (_keyboardState.IsKeyDown(Keys.Left))
                _x--;
            if (_keyboardState.IsKeyDown(Keys.Up))
                _y--;

            if (_keyboardState.IsKeyDown(Keys.D))
                _width++;
            if (_keyboardState.IsKeyDown(Keys.A))
                _width--;
            if (_keyboardState.IsKeyDown(Keys.S))
                _height++;
            if (_keyboardState.IsKeyDown(Keys.W))
                _height--;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            spriteBatch.Draw(
                _testSprite, 
                new Vector2(0, 0),
                new Rectangle(0, 0, _width, _height),
                Color.Gray,
                0f,
                Vector2.Zero,
                _scale,
                SpriteEffects.None,
                0.5f);
            spriteBatch.Draw(
                _testCharSprite,
                new Vector2(0, 0),
                new Rectangle(_x,_y,_width,_height),
                Color.White,
                0f,
                Vector2.Zero,
                _scale,
                SpriteEffects.None,
                1f);
            spriteBatch.DrawString(_debugFont, $"X: {_x}, Y: {_y}, W: {_width}, H: {_height}, S: {_scale}", new Vector2(1500, 30), Color.Black);

            spriteBatch.End();
        }
    }
}
