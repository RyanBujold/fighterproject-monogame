using System.Collections.Generic;
using static FighterProject.Entities.Player;
using static FighterProject.Entities.Characters.TestChar;
using static FighterProject.Entities.Characters.Character;

namespace FighterProject.Entities.Characters.TestCharacter {
    class TestChar_IdleState : Character_IdleState {
        private TestChar _char;

        /// <summary>
        /// An idle state defining actions for a TestChar.
        /// </summary>
        /// <param name="character">The TestChar.</param>
        public TestChar_IdleState(int id, TestChar character) : base (id, character){
            _char = character;
        }

        public override void Update(List<Input> inputs) {

            if (!_char.IsAirborne) {
                // Turn around logic
                if (_char.IsFacingRight && _char.Position.X > _char.Opponent.Position.X) {
                    _char.ChangeState((int)Actions.Turn_Around);
                }
                else if(!_char.IsFacingRight && _char.Position.X < _char.Opponent.Position.X) {
                    _char.ChangeState((int)Actions.Turn_Around);
                }
                // Jump
                else if (inputs.Contains(Input.Up)) {
                    _char.ChangeAnimation((int)Actions.Jump);
                    _char.Velocity.Y = -_char.JumpHeight;
                }
                // Jab
                else if (inputs.Contains(Input.Button_1)) {
                    _char.Velocity.X = 0;
                    _char.ChangeState((int)TestChar_Actions.Jab);
                }
                // Low Kick
                else if (inputs.Contains(Input.Button_2)) {
                    _char.Velocity.X = 0;
                    _char.ChangeState((int)TestChar_Actions.Low_Kick);
                }
                // Fireball
                else if (inputs.Contains(Input.Button_3)) {
                    _char.Velocity.X = 0;
                    _char.ChangeState((int)TestChar_Actions.Fireball);
                }
                // Throw
                else if (inputs.Contains(Input.Down) && !_char.Opponent.IsAirborne && (
                    (_char.Position.X + _char.ScaledWidth/2 + 10> _char.Opponent.Position.X && _char.IsFacingRight) ||
                    (_char.Position.X - _char.ScaledWidth/2 - 10 < _char.Opponent.Position.X && !_char.IsFacingRight) )
                    ) {
                    _char.ChangeState((int)Actions.Throw);
                }
                // Walk Right
                else if (inputs.Contains(Input.Right) && !inputs.Contains(Input.Left)) {
                    MoveRight();
                }
                // Walk Left
                else if (inputs.Contains(Input.Left) && !inputs.Contains(Input.Right)) {
                    MoveLeft();
                }
                // Idle
                else {
                    _char.Velocity.X = 0;
                    _char.ChangeAnimation((int)Actions.Idle, reset:false);
                }
            }
            else {
                // Jump Attack
                if (inputs.Contains(Input.Button_1) || inputs.Contains(Input.Button_2)) {
                    _char.ChangeState((int)TestChar_Actions.Jump_Attack);
                }

                // Air Drift
                if (inputs.Contains(Input.Right) && !inputs.Contains(Input.Left)) {
                    _char.Velocity.X += 0.05f;
                }
                else if (inputs.Contains(Input.Left) && !inputs.Contains(Input.Right)) {
                    _char.Velocity.X += -0.05f;
                }
            }
        }
    }
}
