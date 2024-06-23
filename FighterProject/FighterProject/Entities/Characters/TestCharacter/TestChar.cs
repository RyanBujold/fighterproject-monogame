using FighterProject.Entities.Characters.TestCharacter;
using FighterProject.Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static FighterProject.Library.Hurtbox;
using static FighterProject.Library.MotionInput;

namespace FighterProject.Entities.Characters {
    /// <summary>
    /// A test character with definitions of their spritesheet,
    /// sprites and animations.
    /// </summary>
    class TestChar : Character {
        /// <summary>
        /// The the path for the Character's spritesheet
        /// </summary>
        public static readonly string SpriteSheet = "sprites/ryuSpritesheet";

        public enum TestChar_Actions {
            Jab = 11,
            Low_Kick = 12,
            Jump_Attack = 13,
            Fireball = 14,
        }

        /// <summary>
        /// The Test Character's sprites.
        /// </summary>
        private static readonly Dictionary<int, Rectangle[]> _sprites = new Dictionary<int, Rectangle[]>() {
            {
                (int)Actions.Idle,
                new Rectangle[] {
                    new Rectangle(0, 0, 70, 110),
                    new Rectangle(68, 0, 70, 110),
                    new Rectangle(136, 0, 70, 110),
                    new Rectangle(204, 0, 70, 110),
                    new Rectangle(272, 0, 70, 110),
                }
            },
            {
                (int)Actions.Jump,
                new Rectangle[] {
                    new Rectangle(0, 240, 66, 120),
                    new Rectangle(67, 240, 66, 120),
                    new Rectangle(135, 230, 60, 120),
                    new Rectangle(197, 230, 60, 120),
                    new Rectangle(258, 230, 60, 120),
                    new Rectangle(373, 240, 60, 120),
                }
            },
            {
                (int)Actions.Walk_Forward,
                new Rectangle[] {
                    new Rectangle(0, 117, 70, 110),
                    new Rectangle(68, 117, 70, 110),
                    new Rectangle(140, 119, 77, 110),
                    new Rectangle(220, 119, 77, 110),
                    new Rectangle(293, 118, 77, 110),
                    new Rectangle(362, 116, 67, 110),
                }
            },
            {
                (int)Actions.Walk_Backward,
                new Rectangle[] {
                    new Rectangle(423, 110, 70, 110),
                    new Rectangle(490, 111, 68, 110),
                    new Rectangle(555, 112, 68, 110),
                    new Rectangle(625, 112, 68, 110),
                    new Rectangle(700, 112, 68, 110),
                    new Rectangle(770, 112, 68, 110),
                }
            },
            {
                (int)Actions.Crouch,
                new Rectangle[] {
                    new Rectangle(547, 17, 60, 91),
                    new Rectangle(677, 42, 66, 69),
                }
            },
            {
                (int)Actions.Turn_Around,
                new Rectangle[] {
                    new Rectangle(342, 0, 70, 110),
                    new Rectangle(412, 0, 70, 110),
                    new Rectangle(482, 0, 65, 110),
                }
            },
            {
                (int)Actions.Block,
                new Rectangle[] {
                    new Rectangle(560, 675, 100, 110),
                }
            },
            {
                (int)Actions.Throw,
                new Rectangle[] {
                    new Rectangle(20, 1310, 85, 100),
                    new Rectangle(105, 1310, 90, 100),
                    new Rectangle(196, 1310, 85, 100),
                    new Rectangle(275, 1310, 95, 100),
                    new Rectangle(370, 1310, 80, 100),
                }
            },
            {
                (int)Actions.Hitstun,
                new Rectangle[] {
                    new Rectangle(167, 2020, 70, 110),
                    new Rectangle(237, 2020, 70, 110),
                    new Rectangle(310, 2020, 75, 110),
                }
            },
            {
                (int)Actions.Defeat,
                new Rectangle[] {
                    new Rectangle(630, 2020, 90, 110),
                    new Rectangle(720, 2020, 106, 110),
                    new Rectangle(830, 2020, 80, 100),
                    new Rectangle(910, 2020, 125, 100),
                    new Rectangle(1040, 2020, 130, 100),
                }
            },
            {
                (int)Actions.Victory,
                new Rectangle[] {
                    new Rectangle(350, 1920, 75, 100),
                    new Rectangle(420, 1920, 75, 100),
                    new Rectangle(495, 1915, 75, 110),
                    new Rectangle(575, 1890, 60, 130),
                }
            },
            {
                (int)TestChar_Actions.Jab,
                new Rectangle[] {
                    new Rectangle(0, 354, 75, 110),
                    new Rectangle(89, 354, 110, 110),
                    new Rectangle(0, 354, 75, 110),
                }
            },
            {
                (int)TestChar_Actions.Low_Kick,
                new Rectangle[] {
                    new Rectangle(939,1064,79,80),
                    new Rectangle(1017,1064,107,80),
                    new Rectangle(1131,1064,150,80),
                }
            },
            {
                (int)TestChar_Actions.Jump_Attack,
                new Rectangle[] {
                    new Rectangle(680, 1045, 70, 80),
                    new Rectangle(750, 1045, 70, 80),
                    new Rectangle(820, 1045, 120, 80),
                }
            },
            {
                (int)TestChar_Actions.Fireball,
                new Rectangle[] {
                    new Rectangle(8, 1405, 85, 110),
                    new Rectangle(105, 1405, 95, 110),
                    new Rectangle(205, 1405, 95, 110),
                    new Rectangle(310, 1405, 110, 110),
                    new Rectangle(426, 1407, 135, 120),
                }
            }
        };

        private readonly Dictionary<int, Animation> _testCharAnimations = new Dictionary<int, Animation>() {
            {
                (int)Actions.Idle,
                new Animation(new List<Frame>() {
                    new Frame(_sprites[(int)Actions.Idle][0], new Hurtbox(0, 0, 70, 100)),
                    new Frame(_sprites[(int)Actions.Idle][1], new Hurtbox(0, 0, 70, 100)),
                    new Frame(_sprites[(int)Actions.Idle][2], new Hurtbox(0, 0, 70, 100)),
                    new Frame(_sprites[(int)Actions.Idle][3], new Hurtbox(0, 0, 70, 100)),
                    new Frame(_sprites[(int)Actions.Idle][4], new Hurtbox(0, 0, 70, 100)),
                }, frameRate:3, loop:true)
            },
            {
                (int)Actions.Jump,
                new Animation(new List<Frame>() {
                    new Frame(_sprites[(int)Actions.Jump][0], new Hurtbox(0, 0, 60, 100), Yoffset:-10),
                    new Frame(_sprites[(int)Actions.Jump][1], new Hurtbox(0, 0, 60, 80), Yoffset:-10),
                    new Frame(_sprites[(int)Actions.Jump][2], new Hurtbox(0, 0, 60, 80), Yoffset:-10),
                    new Frame(_sprites[(int)Actions.Jump][3], new Hurtbox(0, 0, 60, 80), Yoffset:-10),
                    new Frame(_sprites[(int)Actions.Jump][4], new Hurtbox(0, 0, 60, 80), Yoffset:-10),
                    new Frame(_sprites[(int)Actions.Jump][5], new Hurtbox(0, 0, 60, 80), Yoffset:-10),
                }, frameRate:5)
            },
            {
                (int)Actions.Walk_Forward,
                new Animation(new List<Frame>() {
                    new Frame(_sprites[(int)Actions.Walk_Forward][0], new Hurtbox(0, 0, 70, 100)),
                    new Frame(_sprites[(int)Actions.Walk_Forward][1], new Hurtbox(0, 0, 74, 100), Xoffset:-10),
                    new Frame(_sprites[(int)Actions.Walk_Forward][2], new Hurtbox(0, 0, 78, 100), Xoffset:-10),
                    new Frame(_sprites[(int)Actions.Walk_Forward][3], new Hurtbox(0, 0, 77, 100)),
                    new Frame(_sprites[(int)Actions.Walk_Forward][4], new Hurtbox(0, 0, 74, 100)),
                    new Frame(_sprites[(int)Actions.Walk_Forward][5], new Hurtbox(0, 0, 70, 100)),
                }, frameRate:5, loop:true)
            },
            {
                (int)Actions.Walk_Backward,
                new Animation(new List<Frame>() {
                    new Frame(_sprites[(int)Actions.Walk_Backward][0], new Hurtbox(0, 0, 70, 100)),
                    new Frame(_sprites[(int)Actions.Walk_Backward][1], new Hurtbox(0, 0, 70, 100)),
                    new Frame(_sprites[(int)Actions.Walk_Backward][2], new Hurtbox(0, 0, 70, 100)),
                    new Frame(_sprites[(int)Actions.Walk_Backward][3], new Hurtbox(0, 0, 70, 100)),
                    new Frame(_sprites[(int)Actions.Walk_Backward][4], new Hurtbox(0, 0, 70, 100)),
                    new Frame(_sprites[(int)Actions.Walk_Backward][5], new Hurtbox(0, 0, 70, 100)),
                }, frameRate:5, loop:true)
            },
            {
                (int)Actions.Crouch,
                new Animation(new List<Frame>() {
                    new Frame(_sprites[(int)Actions.Crouch][0], new Hurtbox(0, 0, 70, 100), Yoffset:15),
                    new Frame(_sprites[(int)Actions.Crouch][1], new Hurtbox(0, 40, 76, 60), Yoffset:40),
                }, frameRate:2, stopOnFinish:false)
            },
            {
                (int)Actions.Turn_Around,
                new Animation(new List<Frame>() {
                    new Frame(_sprites[(int)Actions.Turn_Around][0], new Hurtbox(0, 0, 65, 100)),
                    new Frame(_sprites[(int)Actions.Turn_Around][1], new Hurtbox(0, 0, 65, 100)),
                    new Frame(_sprites[(int)Actions.Turn_Around][2], new Hurtbox(0, 0, 65, 100)),
                }, frameRate:3)
            },
            {
                (int)Actions.Block,
                new Animation(new List<Frame>(){
                    new Frame(_sprites[(int)Actions.Block][0], new Hurtbox(0, 0, 60, 100, DefenseState.Blocking), Xoffset:-10),
                }, frameRate:10)
            },
            {
                (int)Actions.Throw,
                new Animation(new List<Frame>() {
                    new Frame(_sprites[(int)Actions.Throw][0], new Hurtbox(0, 0, 70, 100), Xoffset:-12, Yoffset:10, setVelocity:true),
                    new Frame(_sprites[(int)Actions.Throw][0], new Hurtbox(0, 0, 70, 100), new Hitbox(40, 17, 25, 25, damage:0, hitstun:0), Xoffset:-12, Yoffset:10),
                    new Frame(_sprites[(int)Actions.Throw][2], new Hurtbox(0, 0, 70, 100), Xoffset:-12, Yoffset:10),
                    new Frame(_sprites[(int)Actions.Throw][2], new Hurtbox(0, 0, 70, 100), Xoffset:-12, Yoffset:10),
                    new Frame(_sprites[(int)Actions.Throw][2], new Hurtbox(0, 0, 70, 100), Xoffset:-12, Yoffset:10),

                },
                secondaryAnimation: new Animation(new List<Frame>() {
                        new Frame(_sprites[(int)Actions.Throw][0], Xoffset:-12, Yoffset:10, setVelocity:true),
                        new Frame(_sprites[(int)Actions.Throw][0], Xoffset:-12, Yoffset:10),
                        new Frame(_sprites[(int)Actions.Throw][1], Xoffset:-10, Yoffset:10),
                        new Frame(_sprites[(int)Actions.Throw][2], Xoffset:5, Yoffset:10),
                        new Frame(_sprites[(int)Actions.Throw][3], Xoffset:20, Yoffset:10),
                        new Frame(_sprites[(int)Actions.Throw][4], Xoffset:30, Yoffset:10, Xvelocity:-2.5f, setVelocity:true),
                        new Frame(_sprites[(int)Actions.Throw][4], Xoffset:30, Yoffset:10),
                    },
                    opponentAnimation:
                    new Animation(
                        new List<Frame>() {
                        new Frame(_sprites[(int)Actions.Hitstun][0], Yoffset:10, Xmovement:50, Ymovement:5, setVelocity:true),
                        new Frame(_sprites[(int)Actions.Hitstun][0], Yoffset:10, setVelocity:true),
                        new Frame(_sprites[(int)Actions.Hitstun][0], Yoffset:10, Ymovement:70, setVelocity:true),
                        new Frame(_sprites[(int)Actions.Hitstun][0], Yoffset:10, setVelocity:true),
                        new Frame(_sprites[(int)Actions.Hitstun][0], Yoffset:10, Xmovement:-50, setVelocity:true),
                        new Frame(_sprites[(int)Actions.Hitstun][0], Yoffset:10, Xvelocity:-2f, Yvelocity:1, setVelocity:true, damageTaken:15),
                        new Frame(_sprites[(int)Actions.Defeat][0]),
                    }, frameRate:5, stopOnFinish:false, stopOnLanding:true), frameRate:5),
                frameRate:4)
            },
            {
                (int)Actions.Hitstun,
                new Animation(new List<Frame>() {
                    new Frame(_sprites[(int)Actions.Hitstun][0], new Hurtbox(0, 10, 70, 90), Yoffset:10),
                    new Frame(_sprites[(int)Actions.Hitstun][1], new Hurtbox(0, 10, 70, 90), Xoffset:-5, Yoffset:10),
                    new Frame(_sprites[(int)Actions.Hitstun][2], new Hurtbox(0, 10, 70, 90), Xoffset:-15, Yoffset:10),
                    new Frame(_sprites[(int)Actions.Hitstun][2], new Hurtbox(0, 10, 70, 90), Xoffset:-15, Yoffset:10),
                }, frameRate:4, stopOnHistun:true)
            },
            {
                (int)Actions.Defeat,
                new Animation(new List<Frame>() {
                    new Frame(_sprites[(int)Actions.Defeat][0]),
                    new Frame(_sprites[(int)Actions.Defeat][1], Yoffset:10),
                    new Frame(_sprites[(int)Actions.Defeat][2], Yoffset:10),
                    new Frame(_sprites[(int)Actions.Defeat][3], Yoffset:15),
                    new Frame(_sprites[(int)Actions.Defeat][4], Yoffset:5),
                }, frameRate:4, stopOnFinish:false)
            },
            {
                (int)Actions.Victory,
                new Animation(new List<Frame>() {
                    new Frame(_sprites[(int)Actions.Victory][0], Yoffset:5),
                    new Frame(_sprites[(int)Actions.Victory][1], Xoffset:5, Yoffset:5),
                    new Frame(_sprites[(int)Actions.Victory][2], Xoffset:10),
                    new Frame(_sprites[(int)Actions.Victory][3], Xoffset:10, Yoffset:-25),
                }, frameRate:3, stopOnFinish:false)
            },
            {
                (int)TestChar_Actions.Jab,
                new Animation(new List<Frame>() {
                    new Frame(_sprites[(int)TestChar_Actions.Jab][0], new Hurtbox(0, 0, 75, 100)),
                    new Frame(_sprites[(int)TestChar_Actions.Jab][1], new Hurtbox(0, 0, 100, 100), new Hitbox(90, 17, 25, 25, damage:10, hitstun:9)),
                    new Frame(_sprites[(int)TestChar_Actions.Jab][2], new Hurtbox(0, 0, 75, 100)),
                }, frameRate:2)
            },
            {
                (int)TestChar_Actions.Low_Kick,
                new Animation(new List<Frame>() {
                    new Frame(_sprites[(int)TestChar_Actions.Low_Kick][0], new Hurtbox(0, 35, 100, 65), Yoffset:30),
                    new Frame(_sprites[(int)TestChar_Actions.Low_Kick][1], new Hurtbox(0, 35, 100, 65), Yoffset:30),
                    new Frame(_sprites[(int)TestChar_Actions.Low_Kick][2], new Hurtbox(0, 35, 100, 65), new Hitbox(75, 80, 75, 20, damage:10, hitstun:11), Yoffset:30),
                    new Frame(_sprites[(int)TestChar_Actions.Low_Kick][2], new Hurtbox(0, 35, 100, 65), new Hitbox(75, 80, 75, 20, damage:10, hitstun:11), Yoffset:30),
                    new Frame(_sprites[(int)TestChar_Actions.Low_Kick][2], new Hurtbox(0, 35, 100, 65), new Hitbox(75, 80, 75, 20, damage:10, hitstun:11), Yoffset:30),
                    new Frame(_sprites[(int)TestChar_Actions.Low_Kick][1], new Hurtbox(0, 35, 100, 65), Yoffset:30),
                    new Frame(_sprites[(int)TestChar_Actions.Low_Kick][0], new Hurtbox(0, 35, 100, 65), Yoffset:30),
                }, frameRate:2)
            },
            {
                (int)TestChar_Actions.Jump_Attack,
                new Animation(new List<Frame>() {
                    new Frame(_sprites[(int)TestChar_Actions.Jump_Attack][0], new Hurtbox(0, 0, 60, 70), Xoffset:-5, Yoffset:-8),
                    new Frame(_sprites[(int)TestChar_Actions.Jump_Attack][1], new Hurtbox(0, 0, 60, 70), Yoffset:-8),
                    new Frame(_sprites[(int)TestChar_Actions.Jump_Attack][2], new Hurtbox(0, 0, 60, 60), new Hitbox(25, 35, 90, 35, damage:15, hitstun:8), Xoffset:-5, Yoffset:-16),
                    new Frame(_sprites[(int)TestChar_Actions.Jump_Attack][2], new Hurtbox(0, 0, 60, 60), new Hitbox(25, 35, 90, 35, damage:15, hitstun:8), Xoffset:-5, Yoffset:-16),
                    new Frame(_sprites[(int)TestChar_Actions.Jump_Attack][1], new Hurtbox(0, 0, 60, 70), Yoffset:-8),
                    new Frame(_sprites[(int)TestChar_Actions.Jump_Attack][0], new Hurtbox(0, 0, 60, 70), Xoffset:-5, Yoffset:-8),
                }, frameRate:5, stopOnLanding:true, checkForLandingBeforeFinish:true)
            },
            {
                (int)TestChar_Actions.Fireball,
                new Animation(new List<Frame>() {
                    new Frame(_sprites[(int)TestChar_Actions.Fireball][0], new Hurtbox(-15, 0, 70, 100), Xoffset:-15),
                    new Frame(_sprites[(int)TestChar_Actions.Fireball][1], new Hurtbox(-15, 0, 70, 100), Xoffset:-15),
                    new Frame(_sprites[(int)TestChar_Actions.Fireball][2], new Hurtbox(-10, 0, 70, 100), Xoffset:-15),
                    new Frame(_sprites[(int)TestChar_Actions.Fireball][3], new Hurtbox(0, 0, 100, 100), Xoffset:-15),
                    new Frame(_sprites[(int)TestChar_Actions.Fireball][3], new Hurtbox(0, 0, 100, 100), Xoffset:-15),
                    new Frame(_sprites[(int)TestChar_Actions.Fireball][3], new Hurtbox(0, 0, 100, 100), Xoffset:-15),
                    new Frame(_sprites[(int)TestChar_Actions.Fireball][3], new Hurtbox(0, 0, 100, 100), Xoffset:-15),
                    new Frame(_sprites[(int)TestChar_Actions.Fireball][3], new Hurtbox(0, 0, 100, 100), Xoffset:-15),
                    new Frame(_sprites[(int)TestChar_Actions.Fireball][3], new Hurtbox(0, 0, 100, 100), Xoffset:-15),
                }, frameRate:5)
            }
        };

        private readonly float _testCharMoveSpeed = 9.5f;

        private static readonly float testCharScale = 4.5f;

        /// <summary>
        /// A Test Character.
        /// </summary>
        /// <param name="sprite">The Test Character's spritesheet.</param>
        /// <param name="isFacingRight">True if Character starts facing right.</param>
        public TestChar(Texture2D sprite, bool isFacingRight = true) : base(sprite, testCharScale, isFacingRight) {
            // Add all the animations.
            for (int i = 0; i < _testCharAnimations.Count; i++) {
                Animations.Add(_testCharAnimations[i]);
            }
            CurrentAnimation = _testCharAnimations[(int)Actions.Idle];
            SpritePos = _testCharAnimations[(int)Actions.Idle].CurrentLocation;
            Hurtbox = _testCharAnimations[(int)Actions.Idle].CurrentHurtbox;
            Hitbox = _testCharAnimations[(int)Actions.Idle].CurrentHitbox;

            // Character Attributes
            MoveSpeed = _testCharMoveSpeed;

            // Character special moves
            MotionInputs.Add(new MotionInput(new List<DirectionalInputs>() { DirectionalInputs.D, DirectionalInputs.DF, DirectionalInputs.F, DirectionalInputs.Btn_1 }));

            // Add all states
            States.Add((int)Actions.Idle, new TestChar_IdleState((int)Actions.Idle, this));
            States.Add((int)Actions.Turn_Around, new Character_TurnAroundState((int)Actions.Turn_Around, this));
            States.Add((int)Actions.Hitstun, new Character_ActionState((int)Actions.Hitstun, this));
            States.Add((int)Actions.Block, new Character_ActionState((int)Actions.Block, this));
            States.Add((int)Actions.Throw, new Character_ActionState((int)Actions.Throw, this));
            States.Add((int)Actions.Victory, new Character_ActionState((int)Actions.Victory, this));
            States.Add((int)Actions.Defeat, new Character_ActionState((int)Actions.Defeat, this));
            States.Add((int)TestChar_Actions.Jab, new Character_ActionState((int)TestChar_Actions.Jab, this));
            States.Add((int)TestChar_Actions.Low_Kick, new Character_ActionState((int)TestChar_Actions.Low_Kick, this));
            States.Add((int)TestChar_Actions.Jump_Attack, new Character_ActionState((int)TestChar_Actions.Jump_Attack, this));
            States.Add((int)TestChar_Actions.Fireball, new TestChar_FireballState((int)TestChar_Actions.Fireball, this));

            // Set Current State
            ChangeState((int)Actions.Idle);
        }

    }
}
