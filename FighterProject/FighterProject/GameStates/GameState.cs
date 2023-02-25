using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FighterProject.GameStates {
    abstract class GameState : IGameState {

        protected GraphicsDevice _graphicsDevice;

        /// <summary>
        /// A Game State.
        /// </summary>
        /// <param name="graphicsDevice">A graphics device.</param>
        public GameState(GraphicsDevice graphicsDevice) {
            _graphicsDevice = graphicsDevice;
        }

        public abstract void Initialize();

        public abstract void LoadContent(ContentManager content);

        public abstract void  UnloadContent();

        public abstract void  Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
