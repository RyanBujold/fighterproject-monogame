using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FighterProject.GameStates {
    class GameStateManager {

        /// <summary>
        /// The instance of a Game State Manager.
        /// </summary>
        public static GameStateManager Instance {
            get {
                if(_instance == null) {
                    _instance = new GameStateManager();
                }
                return _instance;
            }
        }

        private static GameStateManager _instance;

        private Stack<GameState> _stateStack = new Stack<GameState>();

        private ContentManager _content;

        /// <summary>
        /// Set the content for the Game State Manager.
        /// </summary>
        /// <param name="content">The content.</param>
        public void SetContent(ContentManager content) {
            _content = content;
        }

        /// <summary>
        /// Adds a new state to the stack.
        /// </summary>
        /// <param name="state"></param>
        public void AddState(GameState state) {
            // Add the state to the stack.
            _stateStack.Push(state);
            // Initialize the state.
            _stateStack.Peek().Initialize();
            // Load the state's content.
            if (_content != null)
                _stateStack.Peek().LoadContent(_content);
        }

        /// <summary>
        /// Remove the top state from the stack.
        /// </summary>
        public void RemoveState() {
            if(_stateStack.Count > 0) {
                _stateStack.Pop();
            }
        }

        /// <summary>
        /// Clear all the states from the stack.
        /// </summary>
        public void ClearStateStack() {
            _stateStack.Clear();
        }

        /// <summary>
        /// Removes all states from the stack and adds a new one.
        /// </summary>
        /// <param name="state">The state.</param>
        public void ChangeStates(GameState state) {
            ClearStateStack();
            AddState(state);
        }

        /// <summary>
        /// Update the state at the top of the stack.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void Update(GameTime gameTime) {
            if(_stateStack.Count > 0) {
                _stateStack.Peek().Update(gameTime);
            }
        }

        /// <summary>
        /// Draw the state at the top of the stack.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        public void Draw(SpriteBatch spriteBatch) {
            if(_stateStack.Count > 0) {
                _stateStack.Peek().Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Unload all the content from the state stack.
        /// </summary>
        public void UnloadContent() {
            foreach(GameState state in _stateStack) {
                state.UnloadContent();
            }
        }

    }
}
