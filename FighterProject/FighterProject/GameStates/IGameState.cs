using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FighterProject.GameStates {
    interface IGameState {
        
        /// <summary>
        /// Initialize the state's settings.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Load the state's content.
        /// </summary>
        /// <param name="content">The content manager.</param>
        void LoadContent(ContentManager content);

        /// <summary>
        /// Unload the state's content.
        /// </summary>
        void UnloadContent();

        /// <summary>
        /// Update the state's content.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Draw the state's content.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        void Draw(SpriteBatch spriteBatch);
    }
}
