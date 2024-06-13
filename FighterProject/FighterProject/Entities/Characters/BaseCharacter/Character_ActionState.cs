using FighterProject.Library;
using System.Collections.Generic;
using static FighterProject.Entities.Characters.Character;
using static FighterProject.Entities.Player;

namespace FighterProject.Entities.Characters {
    /// <summary>
    /// This state performs an animation for the given character.
    /// </summary>
    class Character_ActionState : ICharacterState {
        protected Character _char;
        protected Animation _animation;

        public int ID { get; set; }

        /// <summary>
        /// A Character Action State.
        /// </summary>
        /// <param name="character">The Character.</param>
        /// <param name="id">The animation identifier.</param>
        public Character_ActionState(int id, Character character) {
            ID = id;
            _char = character;
        }

        /// <summary>
        /// A Character Action State.
        /// </summary>
        /// <param name="character">The Character.</param>
        /// <param name="animation">An animation.</param>
        public Character_ActionState(Character character, Animation animation) {
            _char = character;
            _animation = animation;
            ID = -1;
        }

        public virtual void Enter() {
            // Based on the arguments given, change the character's animation.
            if (ID != -1)
                _char.ChangeAnimation(ID);
            else
                _char.ChangeAnimation(_animation);


            // If we have an opponent animation, change the opponent's animation to that animation.
            if (_char.CurrentAnimation.OpponentAnimation != null) {
                _char.Opponent.ChangeState(new Character_ActionState(_char.Opponent, _char.CurrentAnimation.OpponentAnimation));
            }
        }

        public virtual void Exit() { }

        public virtual void Update(List<Input> inputs) {

            // If we want to stop when histun stops.
            if (_char.CurrentAnimation.StopOnHistun) {
                if (_char.Hitstun <= 0)
                    _char.ChangeState((int)Actions.Idle);
            }
            // If we want to stop when finished animating.
            else if(_char.CurrentAnimation.IsFinished && _char.CurrentAnimation.StopOnFinish) {
                _char.ChangeState((int)Actions.Idle);
            }
            // If we want to stop when landing.
            else if (_char.CurrentAnimation.IsFinished && _char.CurrentAnimation.StopOnLanding && !_char.IsAirborne) {
                _char.ChangeState((int)Actions.Idle);
            }
            // If we want to stop when landing before the animation is finished.
            else if(_char.CurrentAnimation.CheckForLandingBeforeFinish && _char.CurrentAnimation.StopOnLanding && !_char.IsAirborne) {
                _char.ChangeState((int)Actions.Idle);
            }

        }
    }
}
