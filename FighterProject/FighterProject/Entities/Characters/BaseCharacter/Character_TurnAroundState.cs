using FighterProject.Library;
using System.Collections.Generic;
using static FighterProject.Entities.Characters.Character;
using static FighterProject.Entities.Player;

namespace FighterProject.Entities.Characters {
    class Character_TurnAroundState : ICharacterState{

        private Character _char;

        public int ID { get; set; }

        /// <summary>
        /// A state that turns the Character around.
        /// </summary>
        /// <param name="character">The Character.</param>
        public Character_TurnAroundState(int id, Character character) {
            ID = id;
            _char = character;
        }

        public void Enter() {
            _char.TurnAround();
            _char.ChangeAnimation((int)Actions.Turn_Around);
        }

        public void Exit() {
            _char.FlipSprite();
            _char.ChangeAnimation((int)Actions.Idle);
        }

        public void Update(List<Input> inputs) {
            // Don't move while turning around.
            _char.Velocity.X = 0;

            // When the animation is finished, return to idle state.
            if (_char.CurrentAnimation.IsFinished) {
                _char.ChangeState((int)Actions.Idle);
            }
            // If there are any inputs, go to idle state.
            else if (inputs.Count > 0) {
                _char.ChangeState((int)Actions.Idle);
            }
        }
    }
}
