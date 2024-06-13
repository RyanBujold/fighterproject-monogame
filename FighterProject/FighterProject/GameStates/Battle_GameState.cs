using FighterProject.Entities;
using FighterProject.Entities.Characters;
using FighterProject.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FighterProject.GameStates {
    class Battle_GameState : GameState {

        public Battle_GameState(GraphicsDevice graphicsDevice) : base(graphicsDevice) { }

        private readonly string sunsetStage = "sunsetBackground1080";

        private readonly float playerScale = 4f;

        private Battlefield battlefield;
        private Texture2D _testSprite;

        public override void Initialize() {
            
        }

        public override void LoadContent(ContentManager content) {
            Texture2D stageTexture = content.Load<Texture2D>(sunsetStage);
            SpriteFont timerFont = content.Load<SpriteFont>("fonts/default");
            SpriteFont winFont = content.Load<SpriteFont>("fonts/default");

            Player player1 = new Player(1, new TestChar(content.Load<Texture2D>(TestChar.SpriteSheet), playerScale), Battlefield.Player1StartingPos);
            Player player2 = new Player(2, new TestChar(content.Load<Texture2D>(TestChar.SpriteSheet), playerScale, false), Battlefield.Player2StartingPos);

            battlefield = new Battlefield(stageTexture, timerFont, winFont, player1, player2);

            // TEST SPRITE
            _testSprite = new Texture2D(_graphicsDevice, 1, 1);
            _testSprite.SetData(new[] { Color.White });
        }

        public override void UnloadContent() {
            
        }

        public override void Update(GameTime gameTime) {
            float timepassed = gameTime.ElapsedGameTime.Milliseconds;

            // Check if the battle is over
            if (battlefield.IsBattlefieldDone())
                GameStateManager.Instance.ChangeStates(new MainMenu_GameState(_graphicsDevice));
            // Battlefield
            battlefield.Update(timepassed);
        }

        public override void Draw(SpriteBatch spriteBatch) {

            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            // what image to render and where
            battlefield.Draw(spriteBatch, _testSprite);

            spriteBatch.End();
        }
    }
}
