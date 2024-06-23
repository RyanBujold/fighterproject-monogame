using FighterProject.Library;
using System.Collections.Generic;
using static FighterProject.Entities.Characters.Character;
using static FighterProject.Entities.Player;

namespace FighterProject.Entities.Characters {
    class Character_IdleState : ICharacterState{
        private Character _char;

        public int ID { get; set; }

        /// <summary>
        /// Defines the state when Character is idle.
        /// </summary>
        /// <param name="character">The Character.</param>
        public Character_IdleState(int id, Character character) {
            ID = id;
            _char = character;
        }

        public virtual void Enter() { }

        public virtual void Exit() { }

        public virtual void Update(List<Input> inputs) { }

        /// <summary>
        /// Move the Character Right.
        /// </summary>
        protected void MoveRight() {
            if (_char.IsFacingRight) {
                _char.Velocity.X = 1;
                _char.ChangeAnimation((int)Actions.Walk_Forward, reset: false);
            }
            else {
                _char.Velocity.X = 0.8f;
                _char.ChangeAnimation((int)Actions.Walk_Backward, reset: false);
            }
        }

        /// <summary>
        /// Move the Character Left.
        /// </summary>
        protected void MoveLeft() {
            if (_char.IsFacingRight) {
                _char.Velocity.X = -0.8f;
                _char.ChangeAnimation((int)Actions.Walk_Backward, reset: false);
            }  
            else {
                _char.Velocity.X = -1;
                _char.ChangeAnimation((int)Actions.Walk_Forward, reset: false);
            }
                
        }

    }
}
