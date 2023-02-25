using FighterProject.Library;
using FighterProject.Objects;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FighterProject.Entities.Characters.TestCharacter {
    class TestChar_Fireball : Projectile{

        public enum TestChar_Fireball_Actions {
            Appear,
            Travel,
            Dissapear,
        }

        private readonly static Dictionary<int, Rectangle[]> _sprites = new Dictionary<int, Rectangle[]>() {
            {
                (int)TestChar_Fireball_Actions.Travel,
                new Rectangle[] {
                    new Rectangle(695, 1430, 60, 60),
                    new Rectangle(760, 1430, 60, 60),
                    new Rectangle(825, 1430, 65, 60),
                    new Rectangle(894, 1429, 65, 60),
                    new Rectangle(960, 1430, 60, 60),
                    new Rectangle(1025, 1430, 60, 60),
                }
            }
        };

        private readonly Dictionary<int, Animation> _testCharFireballAnimations = new Dictionary<int, Animation>() {
            {
                (int)TestChar_Fireball_Actions.Travel,
                new Animation(new List<Frame>() {
                        new Frame(_sprites[(int)TestChar_Fireball_Actions.Travel][0], hitbox: new Hitbox(20,10,40,40,damage:10)),
                        new Frame(_sprites[(int)TestChar_Fireball_Actions.Travel][1], hitbox: new Hitbox(20,10,40,40,damage:10)),
                        new Frame(_sprites[(int)TestChar_Fireball_Actions.Travel][2], hitbox: new Hitbox(20,10,40,40,damage:10)),
                        new Frame(_sprites[(int)TestChar_Fireball_Actions.Travel][3], hitbox: new Hitbox(20,10,40,40,damage:10)),
                        new Frame(_sprites[(int)TestChar_Fireball_Actions.Travel][4], hitbox: new Hitbox(20,10,40,40,damage:10)),
                        new Frame(_sprites[(int)TestChar_Fireball_Actions.Travel][5], hitbox: new Hitbox(20,10,40,40,damage:10)),
                }, frameRate:5, loop:true, stopOnFinish:false)
            }
        };

        private readonly float _fireballMoveSpeed = 0.7f;
        private static readonly float _Xoffset = 75;
        private static readonly float _Yoffset = 70;

        /// <summary>
        /// A testChar's Fireball.
        /// </summary>
        /// <param name="character">The TestChar.</param>
        public TestChar_Fireball(TestChar character) : base(character, _Xoffset, _Yoffset) {
            CurrentAnimation = _testCharFireballAnimations[(int)TestChar_Fireball_Actions.Travel];
            SpritePos = _testCharFireballAnimations[(int)TestChar_Fireball_Actions.Travel].CurrentLocation;
            Hitbox = _testCharFireballAnimations[(int)TestChar_Fireball_Actions.Travel].CurrentHitbox;
            MoveSpeed = _fireballMoveSpeed;
        }

    }
}
