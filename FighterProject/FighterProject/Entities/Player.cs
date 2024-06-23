using FighterProject.Entities.Characters;
using FighterProject.Library;
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
        /// True if the player won.
        /// </summary>
        public bool DidWin { get; set; }

        /// <summary>
        /// The player's current combo count.
        /// </summary>
        public int ComboCount { get; set; }

        /// <summary>
        /// Collision helper for when hitboxes already hit.
        /// </summary>
        public int HitboxGroupId { get; set; }

        /// <summary>
        /// True if we hit the opponent.
        /// </summary>
        public bool DidHitOpponent { get; set; }

        /// <summary>
        /// The possible inputs from the controller.
        /// </summary>
        public enum Input {
            U,
            D,
            L,
            R,
            UL,
            UR,
            DL,
            DR,
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

            List<Input> directions = new List<Input>();
            List<Input> buttons = new List<Input>();

            // -- Inputs -- //
            if (_gamePadState.IsConnected && Id == 1) {
               // Gamepad (SWITCH Arcade Stick Based)
                if (_gamePadState.IsButtonDown(Buttons.DPadRight)) { directions.Add(Input.R); }
                if (_gamePadState.IsButtonDown(Buttons.DPadLeft)) { directions.Add(Input.L); }
                if (_gamePadState.IsButtonDown(Buttons.DPadUp)) { directions.Add(Input.U); }
                if (_gamePadState.IsButtonDown(Buttons.DPadDown)) { directions.Add(Input.D); }
                if (_gamePadState.IsButtonDown(Buttons.Y)) { buttons.Add(Input.Button_1); }
                if (_gamePadState.IsButtonDown(Buttons.X)) { buttons.Add(Input.Button_2); }
                if (_gamePadState.IsButtonDown(Buttons.RightShoulder)) { buttons.Add(Input.Button_3); }
            }
            else if(Id == 1){ //Player 1
                // Keyboard
                if (_keyboardState.IsKeyDown(Keys.D)) { directions.Add(Input.R); }
                if (_keyboardState.IsKeyDown(Keys.A)) { directions.Add(Input.L); }
                if (_keyboardState.IsKeyDown(Keys.W)) { directions.Add(Input.U); }
                if (_keyboardState.IsKeyDown(Keys.S)) { directions.Add(Input.D); }
                if (_keyboardState.IsKeyDown(Keys.G)) { buttons.Add(Input.Button_1); }
                if (_keyboardState.IsKeyDown(Keys.H)) { buttons.Add(Input.Button_2); }
                if (_keyboardState.IsKeyDown(Keys.J)) { buttons.Add(Input.Button_3); }
            }
            else if(Id == 2) { //Player 2
                // Keyboard
                if (_keyboardState.IsKeyDown(Keys.Right)) { directions.Add(Input.R); }
                if (_keyboardState.IsKeyDown(Keys.Left)) { directions.Add(Input.L); }
                if (_keyboardState.IsKeyDown(Keys.Up)) { directions.Add(Input.U); }
                if (_keyboardState.IsKeyDown(Keys.Down)) { directions.Add(Input.D); }
                if (_keyboardState.IsKeyDown(Keys.NumPad0)) { buttons.Add(Input.Button_1); }
                if (_keyboardState.IsKeyDown(Keys.Decimal)) { buttons.Add(Input.Button_2); }
                if (_keyboardState.IsKeyDown(Keys.NumPad3)) { buttons.Add(Input.Button_3); }
            }

            // Clean up directional inputs
            if(directions.Count >= 2) {
                // Simultanious Opposite Cardinal Directions Check
                if (directions.Contains(Input.L) && directions.Contains(Input.R)) { directions = new List<Input>(); }
                if (directions.Contains(Input.U) && directions.Contains(Input.D)) { directions = new List<Input>(); }

                // Corner input checks
                if (directions.Contains(Input.U) && directions.Contains(Input.L)) { directions = new List<Input> { Input.UL }; }
                if (directions.Contains(Input.U) && directions.Contains(Input.R)) { directions = new List<Input> { Input.UR }; }
                if (directions.Contains(Input.D) && directions.Contains(Input.L)) { directions = new List<Input> { Input.DL }; }
                if (directions.Contains(Input.D) && directions.Contains(Input.R)) { directions = new List<Input> { Input.DR }; }
            }

            // Add the verified inputs
            List<Input> inputs = new List<Input>();
            inputs.AddRange(directions);
            inputs.AddRange(buttons);

            // Update the Character we are Controlling.
            Character.Update(timepassed, inputs);
        }

        /// <summary>
        /// Draw the Player.
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch.</param>
        public void Draw(SpriteBatch spriteBatch) {
            Character.Draw(spriteBatch, Id == 2);
        }

        /// <summary>
        /// Reset the Player.
        /// </summary>
        public void Reset() {
            Character.Reset(Id == 2);
        }

    }
}
