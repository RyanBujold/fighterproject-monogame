using System.Collections.Generic;

namespace FighterProject.Entities.Characters.TestCharacter {
    class TestChar_FireballState : Character_ActionState{

        private bool _projectileCreated;

        public TestChar_FireballState(int id, TestChar character) : base(id, character) {

        }

        public override void Enter() {
            _char.ChangeAnimation(ID);
            _projectileCreated = false;
        }

        public override void Update(List<Player.Input> inputs) {
            // Create our projectile on the specified frame.
            if(_char.CurrentAnimation.CurrentIndex == 4 && !_projectileCreated) {
                _char.Projectiles.Add(new TestChar_Fireball((TestChar)_char));
                _projectileCreated = true;
            }

            base.Update(inputs);
        }

    }
}
