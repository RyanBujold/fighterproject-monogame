using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FighterProject.GameStates {
    class MainMenu_GameState : GameState {

        public MainMenu_GameState(GraphicsDevice graphicsDevice) : base(graphicsDevice) { }

        private SpriteFont menuTextFont;
        private KeyboardState menuKeyboardState;

        public override void Initialize() {
            menuKeyboardState = new KeyboardState();
        }

        public override void LoadContent(ContentManager content) {
            menuTextFont = content.Load<SpriteFont>("fonts/default");
        }

        public override void UnloadContent() {
            
        }

        public override void Update(GameTime gameTime) {
            menuKeyboardState = Keyboard.GetState();

            if (menuKeyboardState.IsKeyDown(Keys.Enter))
                GameStateManager.Instance.ChangeStates(new Battle_GameState(_graphicsDevice));

        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            spriteBatch.DrawString(menuTextFont,
                "TEST MENU: PRESS ENTER TO START GAME",
                new Vector2(20,20),
                Color.White);

            spriteBatch.End();
        }

    }
}
