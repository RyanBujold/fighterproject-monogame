using FighterProject.Entities.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace FighterProject.Entities {
    /// <summary>
    /// A player with an ID, Character and Controller. Used for
    /// managing controller inputs to control Characters and other.
    /// </summary>
    class Player {

        /// <summary>
        /// The current Player's identifier.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// The current Player's Character.
        /// </summary>
        public Character Character { get; private set; }

        /// <summary>
        /// The number of wins the player has.
        /// </summary>
        public int Wins { get; set; }

        /// <summary>
        /// The player's current combo count.
        /// </summary>
        public int ComboCount { get; set; }

        /// <summary>
        /// The possible inputs from the controller.
        /// </summary>
        public enum Input {
            Up,
            Down,
            Left,
            Right,
            Button_1,
            Button_2,
            Button_3,
        }

        private KeyboardState _keyboardState = new KeyboardState();
        private GamePadState _gamePadState = new GamePadState();

        /// <summary>
        /// A Player entitiy.
        /// </summary>
        /// <param name="id">An id.</param>
        /// <param name="character">A Character.</param>
        public Player(int id, Character character) {
            Id = id;
            Character = character;
            Wins = 0;
            ComboCount = 0;
        }

        /// <summary>
        /// A Player entity.
        /// </summary>
        /// <param name="id">An id.</param>
        /// <param name="character">A Character.</param>
        /// <param name="startPos">The starting position.</param>
        public Player(int id, Character character, Vector2 startPos) {
            Id = id;
            Character = character;
            Character.Position = startPos;
            Wins = 0;
            ComboCount = 0;
        }

        /// <summary>
        /// Updates the Character's and entities the Player
        /// has control of.
        /// </summary>
        /// <param name="timepassed">The time passed.</param>
        public void Update(float timepassed) {
            // Get the controllers.
            _keyboardState = Keyboard.GetState();
            _gamePadState = GamePad.GetState(PlayerIndex.One);

            List<Input> inputs = new List<Input>(); 

            // -- Inputs -- //
            if (_gamePadState.IsConnected && Id == 1) {
               // Gamepad (SWITCH Arcade Stick Based)
                if (_gamePadState.IsButtonDown(Buttons.DPadRight)) { inputs.Add(Input.Right); }
                if (_gamePadState.IsButtonDown(Buttons.DPadLeft)) { inputs.Add(Input.Left); }
                if (_gamePadState.IsButtonDown(Buttons.DPadUp)) { inputs.Add(Input.Up); }
                if (_gamePadState.IsButtonDown(Buttons.DPadDown)) { inputs.Add(Input.Down); }
                if (_gamePadState.IsButtonDown(Buttons.Y)) { inputs.Add(Input.Button_1); }
                if (_gamePadState.IsButtonDown(Buttons.X)) { inputs.Add(Input.Button_2); }
                if (_gamePadState.IsButtonDown(Buttons.RightShoulder)) { inputs.Add(Input.Button_3); }
            }
            else if(Id == 1){ //Player 1
                // Keyboard
                if (_keyboardState.IsKeyDown(Keys.D)) { inputs.Add(Input.Right); }
                if (_keyboardState.IsKeyDown(Keys.A)) { inputs.Add(Input.Left); }
                if (_keyboardState.IsKeyDown(Keys.W)) { inputs.Add(Input.Up); }
                if (_keyboardState.IsKeyDown(Keys.S)) { inputs.Add(Input.Down); }
                if (_keyboardState.IsKeyDown(Keys.G)) { inputs.Add(Input.Button_1); }
                if (_keyboardState.IsKeyDown(Keys.H)) { inputs.Add(Input.Button_2); }
                if (_keyboardState.IsKeyDown(Keys.J)) { inputs.Add(Input.Button_3); }
            }
            else if(Id == 2) { //Player 2
                // Keyboard
                if (_keyboardState.IsKeyDown(Keys.Right)) { inputs.Add(Input.Right); }
                if (_keyboardState.IsKeyDown(Keys.Left)) { inputs.Add(Input.Left); }
                if (_keyboardState.IsKeyDown(Keys.Up)) { inputs.Add(Input.Up); }
                if (_keyboardState.IsKeyDown(Keys.Down)) { inputs.Add(Input.Down); }
                if (_keyboardState.IsKeyDown(Keys.NumPad0)) { inputs.Add(Input.Button_1); }
                if (_keyboardState.IsKeyDown(Keys.Decimal)) { inputs.Add(Input.Button_2); }
                if (_keyboardState.IsKeyDown(Keys.NumPad3)) { inputs.Add(Input.Button_3); }
            }

            // Update the Character we are Controlling.
            Character.Update(timepassed, inputs);
        }

        /// <summary>
        /// Draw the Player.
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch.</param>
        public void Draw(SpriteBatch spriteBatch, bool isPlayer2 = false) {
            Character.Draw(spriteBatch, isPlayer2);
        }

        /// <summary>
        /// Reset the Player.
        /// </summary>
        public void Reset(bool isPlayer2 = false) {
            Character.Reset(isPlayer2);
        }

    }
}
