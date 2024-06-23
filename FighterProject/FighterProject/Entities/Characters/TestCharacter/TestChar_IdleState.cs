using System.Collections.Generic;
using static FighterProject.Entities.Player;
using static FighterProject.Entities.Characters.TestChar;
using static FighterProject.Entities.Characters.Character;
using FighterProject.Library;

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
                // Check motion inputs
                foreach(MotionInput motion in _char.MotionInputs) {
                    if (motion.CheckInputs(inputs,_char.IsFacingRight)) {
                        _char.Velocity.X = 0;
                        _char.ChangeState((int)TestChar_Actions.Fireball);
                        return;
                    }
                }

                // Turn around logic
                if (_char.IsFacingRight && _char.Position.X > _char.Opponent.Position.X) {
                    _char.ChangeState((int)Actions.Turn_Around);
                }
                else if(!_char.IsFacingRight && _char.Position.X < _char.Opponent.Position.X) {
                    _char.ChangeState((int)Actions.Turn_Around);
                }
                // Jump
                else if (inputs.Contains(Input.U) || inputs.Contains(Input.UL) || inputs.Contains(Input.UR)) {
                    _char.ChangeAnimation((int)Actions.Jump);
                    _char.Velocity.Y = -_char.JumpHeight;
                    // Make sure we are jumping in the desired direction;
                    if(inputs.Contains(Input.UR)) { _char.Velocity.X = 1; }
                    if(inputs.Contains(Input.UL)) { _char.Velocity.X = -1; }
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
                // Throw
                else if (inputs.Contains(Input.Button_3)) {
                    _char.ChangeState((int)Actions.Throw);
                }
                // Crouch
                else if (inputs.Contains(Input.DL) || inputs.Contains(Input.D) || inputs.Contains(Input.DR)) {
                    _char.Velocity.X = 0;
                    _char.ChangeAnimation((int)Actions.Crouch, reset: false);
                }
                // Walk Right
                else if (inputs.Contains(Input.R)) {
                    MoveRight();
                }
                // Walk Left
                else if (inputs.Contains(Input.L)) {
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
                if (inputs.Contains(Input.R) && !inputs.Contains(Input.L)) {
                    _char.Velocity.X += _char.AerialDrift;
                }
                else if (inputs.Contains(Input.L) && !inputs.Contains(Input.R)) {
                    _char.Velocity.X += -_char.AerialDrift;
                }
            }
        }
    }
}
