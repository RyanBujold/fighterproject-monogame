using System.Collections.Generic;
using FighterProject.Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FighterProject.Entities.Characters.ShadowManCharacter {
    class ShadowMan : Character {

        public static readonly string SpriteSheet = "sprites/shadowManSpritesheetSmall1";

        public enum ShadowMan_Actions {
            Punch = 11,
            Kick = 12,
            Crouch_Punch = 13,
            Crouch_Kick = 14,
            Jump_Punch = 15,
            Jump_Kick = 16,
            Dodge = 17,
            Dash = 18,
            BackDash = 19,
        }


        // Sprite sizes: w/h=800
        private static readonly Dictionary<int, Rectangle[]> _sprites = new Dictionary<int, Rectangle[]>() {
            {
                (int)Actions.Idle,
                new Rectangle[] {
                    new Rectangle(270, 116, 234, 667),
                    new Rectangle(800, 0, 800, 800),
                    new Rectangle(1600, 0, 800, 800),
                    new Rectangle(2400, 0, 800, 800),
                }
            },
            {
                (int)Actions.Jump,
                new Rectangle[] {
                    new Rectangle(0, 0, 800, 800),
                }
            },
            {
                (int)Actions.Walk_Forward,
                new Rectangle[] {
                    new Rectangle(0, 0, 800, 800),
                }
            },
            {
                (int)Actions.Walk_Backward,
                new Rectangle[] {
                    new Rectangle(0, 0, 800, 800),
                }
            },
            {
                (int)Actions.Crouch,
                new Rectangle[] {
                    new Rectangle(0, 0, 800, 800),
                }
            },
            {
                (int)Actions.Turn_Around,
                new Rectangle[] {
                    new Rectangle(0, 0, 800, 800),
                }
            },
            {
                (int)Actions.Block,
                new Rectangle[] {
                    new Rectangle(0, 0, 800, 800),
                }
            },
            {
                (int)Actions.Throw,
                new Rectangle[] {
                    new Rectangle(0, 0, 800, 800),
                }
            },
            {
                (int)Actions.Hitstun,
                new Rectangle[] {
                    new Rectangle(0, 0, 800, 800),
                }
            },
            {
                (int)Actions.Defeat,
                new Rectangle[] {
                    new Rectangle(0, 0, 800, 800),
                }
            },
            {
                (int)Actions.Victory,
                new Rectangle[] {
                    new Rectangle(0, 0, 800, 800),
                }
            },
        };

        private readonly Dictionary<int, Animation> _testCharAnimations = new Dictionary<int, Animation>() {
            {
                (int)Actions.Idle,
                new Animation(new List<Frame>() {
                    new Frame(_sprites[(int)Actions.Idle][0], new Hurtbox(0, 0, 70, 100)),

                }, frameRate:1, loop:true)
            },
            /*{
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
            },*/
        };

        private static readonly float shadowManScale = 1f;

        public ShadowMan(Texture2D sprite, bool isFacingRight = true) : base(sprite, shadowManScale, isFacingRight) {
            // Add all the animations.
            for (int i = 0; i < _testCharAnimations.Count; i++) {
                Animations.Add(_testCharAnimations[i]);
            }
            CurrentAnimation = _testCharAnimations[(int)Actions.Idle];
            SpritePos = _testCharAnimations[(int)Actions.Idle].CurrentLocation;
            Hurtbox = _testCharAnimations[(int)Actions.Idle].CurrentHurtbox;
            Hitbox = _testCharAnimations[(int)Actions.Idle].CurrentHitbox;

            // Character Attributes
            //Width = 500;
            //Height = 500;
            //groundedBodyBox = new CollisionBox(-75, -525, 25, 75);
            //airborneBodyBox = new CollisionBox(0, 0, 50, 75);
            //MoveSpeed = _testCharMoveSpeed;

            // Character special moves
            //MotionInputs.Add(new MotionInput(new List<DirectionalInputs>() { DirectionalInputs.D, DirectionalInputs.DF, DirectionalInputs.F, DirectionalInputs.Btn_1 }));

            // Add all states
            States.Add((int)Actions.Idle, new ShadowMan_IdleState((int)Actions.Idle, this));

            // Set Current State
            ChangeState((int)Actions.Idle);

        }
    }
}
