using FighterProject.Library;
using FighterProject.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static FighterProject.Entities.Player;

namespace FighterProject.Entities.Characters {
    /// <summary>
    /// A class for a Character. A character has a sprite
    /// that can be changed through animations and 
    /// movement. The user can interact with a character
    /// using controllers.
    /// </summary>
    abstract class Character : GameItem{

        /// <summary>
        /// The other Character we are interacting with.
        /// </summary>
        public Character Opponent { get; set; }

        /// <summary>
        /// The Character's current Character state.
        /// </summary>
        public ICharacterState CurrentState { get; protected set; }

        /// <summary>
        /// The Character's health.
        /// </summary>
        public int Health;

        /// <summary>
        /// The Character's starting health.
        /// </summary>
        public static int MaxHealth = 200;

        /// <summary>
        /// The Character's dizzy amount.
        /// </summary>
        public float Dizzy;

        /// <summary>
        /// The Character's maximum dizzy limit.
        /// </summary>
        public static float MaxDizzy = 250;

        /// <summary>
        /// True if the Character is in the air.
        /// </summary>
        public bool IsAirborne { get; private set; }

        /// <summary>
        /// A list of the Character's projectiles.
        /// </summary>
        public List<Projectile> Projectiles = new List<Projectile>();

        /// <summary>
        /// A Character's possible default actions.
        /// </summary>
        public enum Actions {
            Idle = 0,
            Walk_Forward = 1,
            Walk_Backward = 2,
            Crouch = 3,
            Jump = 4,
            Turn_Around = 5,
            Block = 6,
            Throw = 7,
            Hitstun = 8,
            Defeat = 9,
            Victory = 10,
        }

        /// <summary>
        /// The Character's Gravity.
        /// </summary>
        public readonly float Gravity = 0.1f;

        /// <summary>
        /// The Character's Jump Height.
        /// </summary>
        public readonly float JumpHeight = 2.5f;

        /// <summary>
        /// The Character's Aerial Drift amount;
        /// </summary>
        public readonly float AerialDrift = 0.02f;

        /// <summary>
        /// The current inputs read by the Character.
        /// </summary>
        public List<Input> CurrentInputs { get; private set; }

        /// <summary>
        /// A queue of the Character's inputs.
        /// </summary>
        public Queue<List<Input>> InputHistory { get; private set; }

        /// <summary>
        /// The limit to the Input History Queue.
        /// </summary>
        protected readonly int inputHistoryLimit = 10;

        /// <summary>
        /// The collision box for colliding with walls or other characters.
        /// </summary>
        public CollisionBox BodyBox { get; private set; }

        /// <summary>
        /// The Character's hitstun in number of frames of duration.
        /// </summary>
        public int Hitstun { get; set; }

        /// <summary>
        /// The Character's Move Speed.
        /// </summary>
        public float MoveSpeed { get; set; }

        /// <summary>
        /// The Character's total movement on the x axis.
        /// </summary>
        public float CurrentHorizontalMovement { get; private set; }

        /// <summary>
        /// The Character's motion input moves.
        /// </summary>
        public List<MotionInput> MotionInputs { get; private set; }

        /// <summary>
        /// True if the Character is blocking High attacks.
        /// </summary>
        public bool isBlockingHigh = false;

        /// <summary>
        /// True if the Character is blocking Low attacks.
        /// </summary>
        public bool isBlockingLow = false;

        /// <summary>
        /// The Character's possible states.
        /// </summary>
        protected Dictionary<int, ICharacterState> States = new Dictionary<int, ICharacterState>();

        /// <summary>
        /// The Character's default move speed.
        /// </summary>
        protected readonly float defaultMoveSpeed = 9f;

        /// <summary>
        /// The Character's default draw position width.
        /// </summary>
        protected readonly int defaultCharacterWidth = 70;

        /// <summary>
        /// The Character's default draw position height.
        /// </summary>
        protected readonly int defaultCharacterHeight = 100;

        /// <summary>
        /// The Character's grounded collision box.
        /// </summary>
        protected CollisionBox groundedBodyBox = new CollisionBox(-40/2, -75, 40, 75);

        /// <summary>
        /// The Character's airborne collision box.
        /// </summary>
        protected CollisionBox airborneBodyBox = new CollisionBox(-20, -75, 40, 30);

        /// <summary>
        /// A Character entity.
        /// </summary>
        /// <param name="sprite">The Character's spritesheet.</param>
        /// <param name="scale">The scale to draw the Character.</param>
        /// <param name="isFacingRight">Is the character facing right.</param>
        public Character(Texture2D sprite, float scale = 1f, bool isFacingRight = true) : base(sprite, scale, isFacingRight){
            Width = defaultCharacterWidth;
            Height = defaultCharacterHeight;
            Health = MaxHealth;
            Dizzy = 0;
            BodyBox = groundedBodyBox;
            Hitstun = 0;
            MoveSpeed = defaultMoveSpeed;

            MotionInputs = new List<MotionInput>();
            InputHistory = new Queue<List<Input>>();
            InputHistory.Enqueue(new List<Input>());
        }

        /// <summary>
        /// Updates the Character's movement and animations.
        /// </summary>
        /// <param name="timepassed">The time passed.</param>
        /// <param name="inputs">The player's inputs.</param>
        public void Update(float timepassed, List<Input> inputs) {

            // Update the current state with inputs.
            CurrentInputs = inputs;
            // If the current inputs match the inputs at the end of the array, don't add them
            if (!CurrentInputs.SequenceEqual(InputHistory.ElementAt(InputHistory.Count-1))) {
                InputHistory.Enqueue(CurrentInputs);
            }
            // If we have added too many inputs, discard them
            if (InputHistory.Count > inputHistoryLimit) {
                InputHistory.Dequeue();
            }
            CurrentState.Update(inputs);

            // Update Histun
            if (CurrentState.ID == (int)Actions.Hitstun && Hitstun > 0) {
                Hitstun--;
            }
            else if(CurrentState.ID != (int)Actions.Defeat) {
                Dizzy -= 0.2f;
                if (Dizzy < 0) { Dizzy = 0; }
                if (Dizzy > MaxDizzy) { Dizzy = MaxDizzy; }
            }
               

            // Animate our character and update fields if animate returns true.
            if (CurrentAnimation.Animate())
                UpdateFields();

            // Draw
            DrawPosition = new Vector2(Position.X - ScaledSpriteWidth / 2, Position.Y - ScaledSpriteHeight);

            if (CurrentAnimation.CurrentSetVelocity) {
                if (IsFacingRight)
                    Velocity = new Vector2(CurrentAnimation.CurrentVelocity.X, -CurrentAnimation.CurrentVelocity.Y);
                else
                    Velocity = new Vector2(-CurrentAnimation.CurrentVelocity.X, -CurrentAnimation.CurrentVelocity.Y);
            }

            // Check for blocking
            if(inputs.Contains(Input.L) && !Opponent.IsFacingRight && !IsAirborne) { isBlockingHigh = true; }
            else if(inputs.Contains(Input.DL) && !Opponent.IsFacingRight && !IsAirborne) { isBlockingHigh = true; isBlockingLow = true; }
            else if (inputs.Contains(Input.R) && Opponent.IsFacingRight && !IsAirborne) { isBlockingHigh = true; }
            else if (inputs.Contains(Input.DR) && Opponent.IsFacingRight && !IsAirborne) { isBlockingHigh = true; isBlockingLow = true; }
            else { isBlockingHigh = false; isBlockingLow = false; }

            // Movement Logic
            CurrentHorizontalMovement = Velocity.X * MoveSpeed;
            Position.X += CurrentHorizontalMovement;

            // Jump Logic
            Position.Y += Velocity.Y * MoveSpeed;
            if (Position.Y < Game1.Floor) {
                IsAirborne = true;
                Velocity.Y += Gravity;
            }
            else {
                IsAirborne = false;
                Velocity.Y = 0;
            }

            // Get body box
            BodyBox = groundedBodyBox;
            if (IsAirborne) { BodyBox = airborneBodyBox; }

            // Boundary Checks
            if (Position.Y > Game1.Floor) { Position.Y = Game1.Floor; IsAirborne = false; }
            if (Position.X - BodyBox.Width / 2 < Game1.Left_Bound) { Position.X = BodyBox.Width / 2; }
            if (Position.X + BodyBox.Width / 2 > Game1.Right_Bound) { Position.X = Game1.Right_Bound - BodyBox.Width / 2; }

            // Check Character Boundaries with Opponent while moving towards the opponent
            if (CollisionBox.DidCollide(DrawPosition + Velocity, Opponent.DrawPosition + Opponent.Velocity, BodyBox, Opponent.BodyBox, Scale, Opponent.Scale)) {
                if (Position.X == Opponent.Position.X) {
                    // If we are colliding on top of the opponent, move backwards based on the direction we are facing
                    if(IsFacingRight)
                        Position.X = Opponent.Position.X - BodyBox.Width;
                    else
                        Position.X = Opponent.Position.X - BodyBox.Width;
                }
                else if (Position.X < Opponent.Position.X) {
                    // If we are on the left side of the opponent, move left
                    Position.X = Opponent.Position.X - BodyBox.Width;
                }
                else if(Position.X > Opponent.Position.X) {
                    // If we are on the right side of the opponent, move right
                    Position.X = Opponent.Position.X + BodyBox.Width;
                }
            }

            // Update projectiles.
            for (int i = 0; i < Projectiles.Count; i++) {
                Projectiles[i].Update(timepassed);

                // -- Boundary Check -- //
                if(Projectiles[i].Position.X > Game1.Right_Bound || Projectiles[i].Position.X < Game1.Left_Bound) { Projectiles[i].Active = false; }

                // Remove if no longer active.
                if (!Projectiles[i].Active) { Projectiles.RemoveAt(i); i--; }
            }
        }

        /// <summary>
        /// Draw the Character's sprite.
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch.</param>
        public void Draw(SpriteBatch spriteBatch, bool isPlayer2 = false) {
            Color color = Color.White;
            if (isPlayer2) { color = Color.LightBlue; }
            float layer = 0.8f;
            //if (CurrentState.ID != (int)Actions.Crouch) { layer = 1f; } //TODO FIX
            spriteBatch.Draw(
                Sprite,
                DrawPosition + Offset,
                SpritePos,
                color,
                0f,
                Vector2.Zero,
                Scale,
                CurrentEffects,
                layer);

            foreach (Projectile projectile in Projectiles) {
                projectile.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Draw the Character's Debug details.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        /// <param name="testSprite">The test sprite.</param>
        public override void DrawDebug(SpriteBatch spriteBatch, Texture2D testSprite) {
            base.DrawDebug(spriteBatch, testSprite);

            // Draw the hitboxes
            spriteBatch.Draw(
                testSprite,
                DrawPosition + Hitbox.Offset,
                Hitbox.Box,
                Color.Green,
                0f,
                Vector2.Zero,
                Game1.Default_Scale,
                SpriteEffects.None,
                0.5f);
            // Draw the hurtboxes
            spriteBatch.Draw(
                testSprite,
                DrawPosition + Hurtbox.Offset,
                Hurtbox.Box,
                Color.White,
                0f,
                Vector2.Zero,
                Game1.Default_Scale,
                SpriteEffects.None,
                0.4f);
            // Draw the collision box
            spriteBatch.Draw(
                testSprite,
                Position + BodyBox.Offset,
                BodyBox.Box,
                Color.Yellow,
                0f,
                Vector2.Zero,
                Game1.Default_Scale,
                SpriteEffects.None,
                0.2f);

            // Draw the projectile hitboxes
            foreach (Projectile projectile in Projectiles) {
                projectile.DrawDebug(spriteBatch, testSprite);
            }
            // Draw an input reader
            DrawInputs(spriteBatch, testSprite, Position + new Vector2(-100,150), CurrentInputs);
            
            // Draw input history
            int yOffset = 100;
            Vector2 historyPos = new Vector2(20, 200);
            if (!IsFacingRight) { historyPos = new Vector2(Game1.Right_Bound - 250, 200); }
            Queue<List<Input>> reverse = new Queue<List<Input>>(InputHistory.Reverse());
            foreach(List<Input> inputs in reverse) {
                DrawInputs(spriteBatch, testSprite, historyPos, inputs);
                historyPos.Y += yOffset;
            }

        }

        protected void DrawInputs(SpriteBatch spriteBatch, Texture2D testSprite, Vector2 position, List<Input> inputs) {
            for (int c = 0; c < 3; c++) {
                for (int r = 0; r < 3; r++) {
                    Color color = Color.Gray;
                    if (c == 0 && r == 0 && inputs.Contains(Input.UL)) { color = Color.Red; }
                    if (c == 1 && r == 0 && inputs.Contains(Input.U)) { color = Color.Red; }
                    if (c == 2 && r == 0 && inputs.Contains(Input.UR)) { color = Color.Red; }
                    if (c == 0 && r == 1 && inputs.Contains(Input.L)) { color = Color.Red; }
                    //if (c == 1 && r == 1 && inputs.Contains(Input.U)) { color = Color.Red; } TODO neutral
                    if (c == 2 && r == 1 && inputs.Contains(Input.R)) { color = Color.Red; }
                    if (c == 0 && r == 2 && inputs.Contains(Input.DL)) { color = Color.Red; }
                    if (c == 1 && r == 2 && inputs.Contains(Input.D)) { color = Color.Red; }
                    if (c == 2 && r == 2 && inputs.Contains(Input.DR)) { color = Color.Red; }
                    spriteBatch.Draw(
                    testSprite,
                    position + new Vector2(c * 25, (r * 25) - 100),
                    new Rectangle(0, 0, 20, 20),
                    color,
                    0f,
                    Vector2.Zero,
                    Game1.Default_Scale,
                    SpriteEffects.None,
                    0.9f);

                }
            }
            for (int i = 0; i < 3; i++) {
                Color color = Color.Gray;
                if (i == 0 && inputs.Contains(Input.Button_1)) {
                    color = Color.Red;
                }
                if (i == 1 && inputs.Contains(Input.Button_2)) {
                    color = Color.Red;
                }
                if (i == 2 && inputs.Contains(Input.Button_3)) {
                    color = Color.Red;
                }
                spriteBatch.Draw(
                    testSprite,
                    position + new Vector2((i * 35) + 100, -90),
                    new Rectangle(0, 0, 28, 28),
                    color,
                    0f,
                    Vector2.Zero,
                    Game1.Default_Scale,
                    SpriteEffects.None,
                    0.9f);
            }
        }

        /// <summary>
        /// Change the Character's state.
        /// </summary>
        /// <param name="id">The state identifier.</param>
        public void ChangeState(int id) {
            if (CurrentState != null)
                CurrentState.Exit();
            if (!States.ContainsKey(id))
                id = 0;
            CurrentState = States[id];
            CurrentState.Enter();
        }

        /// <summary>
        /// Change the Character's state to a new one.
        /// </summary>
        /// <param name="characterState">The new state.</param>
        public void ChangeState(ICharacterState characterState) {
            if (CurrentState != null)
                CurrentState.Exit();
            CurrentState = characterState;
            CurrentState.Enter();
        }

        /// <summary>
        /// Update the Character's fields.
        /// </summary>
        protected override void UpdateFields() {
            base.UpdateFields();

            // This is here so that secondary animations do damage
            Health -= CurrentAnimation.CurrentDamageTaken;
            Dizzy += CurrentAnimation.CurrentDamageTaken*1.5f;
        }

        /// <summary>
        /// Reset the Character's attributes and make sure they are facing the right direction.
        /// </summary>
        public void Reset(bool isPlayer2 = false) {
            ChangeState((int)Actions.Idle);
            if (!isPlayer2) {
                Position = Battlefield.Player1StartingPos;
                if (!IsFacingRight) {
                    TurnAround();
                    FlipSprite();
                }
            }
            else {
                Position = Battlefield.Player2StartingPos;
                if (IsFacingRight) {
                    TurnAround();
                    FlipSprite();
                }
            }
            Health = MaxHealth - (int)Math.Round(Dizzy);
        }

        /// <summary>
        /// Checks if two lists of inputs are matching.
        /// </summary>
        /// <param name="a">An input list.</param>
        /// <param name="b">An input list.</param>
        /// <returns>True if 'a' and 'b' match.</returns>
        public bool AreInputsEqual(List<Input> a, List<Input> b) {
            bool match = true;
            foreach(Input input in a) {
                if(!b.Contains(input)) 
                    match = false;
            }
            foreach (Input input in b) {
                if (!a.Contains(input))
                    match = false;
            }
            return match;
        }
    }
}
