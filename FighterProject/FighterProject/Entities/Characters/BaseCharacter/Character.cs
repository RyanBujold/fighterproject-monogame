using FighterProject.Library;
using FighterProject.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
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
        /// The character's starting health.
        /// </summary>
        public static int MaxHealth = 200;

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
            Jump = 3,
            Turn_Around = 4,
            Block = 5,
            Throw = 6,
            Hitstun = 7,
            Defeat = 8,
            Victory = 9,
        }

        /// <summary>
        /// The Character's Gravity.
        /// </summary>
        public readonly float Gravity = 0.15f;

        /// <summary>
        /// The Character's Jump Height.
        /// </summary>
        public readonly float JumpHeight = 3f;

        /// <summary>
        /// The Character's possible states.
        /// </summary>
        protected Dictionary<int, ICharacterState> States = new Dictionary<int, ICharacterState>();

        /// <summary>
        /// The Character's Move Speed.
        /// </summary>
        protected float MoveSpeed = 2.38f;

        /// <summary>
        /// The Character's draw position width.
        /// </summary>
        protected readonly int characterWidth = 70;

        /// <summary>
        /// The Character's draw position height.
        /// </summary>
        protected readonly int characterHeight = 100;

        /// <summary>
        /// A Character entity.
        /// </summary>
        /// <param name="sprite">The Character's spritesheet.</param>
        /// <param name="scale">The scale to draw the Character.</param>
        /// <param name="isFacingRight">Is the character facing right.</param>
        public Character(Texture2D sprite, float scale = 1f, bool isFacingRight = true) : base(sprite, scale, isFacingRight){
            Width = characterWidth;
            Height = characterHeight;
            Health = MaxHealth;
        }

        /// <summary>
        /// Updates the Character's movement and
        /// animations.
        /// </summary>
        /// <param name="timepassed">The time passed.</param>
        /// <param name="inputs">The player's inputs.</param>
        public void Update(float timepassed, List<Input> inputs) {

            // Update the current state with inputs.
            CurrentState.Update(inputs);

            // Animate our character and update fields if animate returns true.
            if(CurrentAnimation.Animate())
                UpdateFields();

            // Draw
            DrawPosition = new Vector2(Position.X - ScaledWidth / 2, Position.Y - ScaledHeight);

            if (CurrentAnimation.CurrentSetVelocity) {
                if(IsFacingRight)
                    Velocity = new Vector2(CurrentAnimation.CurrentVelocity.X, -CurrentAnimation.CurrentVelocity.Y);
                else
                    Velocity = new Vector2(-CurrentAnimation.CurrentVelocity.X, -CurrentAnimation.CurrentVelocity.Y);
            }

            // Movement Logic
            float movementSpeed = timepassed / MoveSpeed;
            Position.X += Velocity.X * movementSpeed;

            // Jump Logic
            Position.Y += Velocity.Y * movementSpeed;
            if (Position.Y < Game1.Floor) {
                IsAirborne = true;
                Velocity.Y += Gravity;
            }
            else {
                IsAirborne = false;
                Velocity.Y = 0;
            }
               
            // Boundary Checks
            if (Position.Y > Game1.Floor) { Position.Y = Game1.Floor; IsAirborne = false; }
            if (Position.X - Hurtbox.ScaleSize(Scale).Width / 2 < Game1.Left_Bound) { Position.X = Hurtbox.ScaleSize(Scale).Width / 2; }
            if (Position.X + Hurtbox.ScaleSize(Scale).Width / 2 > Game1.Right_Bound) { Position.X = Game1.Right_Bound - Hurtbox.ScaleSize(Scale).Width / 2; }

            // Check Character Boundaries with Opponent
            if (Position.X + ScaledWidth / 2 > Opponent.Position.X && IsFacingRight && !Opponent.IsFacingRight && !IsAirborne && !Opponent.IsAirborne) { Position.X = Opponent.Position.X - ScaledWidth / 2; }
            if (Position.X - ScaledWidth / 2 < Opponent.Position.X && !IsFacingRight && Opponent.IsFacingRight && !IsAirborne && !Opponent.IsAirborne) { Position.X = Opponent.Position.X + ScaledWidth / 2; }

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
            spriteBatch.Draw(
                Sprite,
                DrawPosition + Offset,
                SpritePos,
                color,
                0f,
                Vector2.Zero,
                Scale,
                CurrentEffects,
                0.9f);

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

            spriteBatch.Draw(
                testSprite,
                DrawPosition + Hitbox.Offset,
                Hitbox.Box,
                Color.Green,
                0f,
                Vector2.Zero,
                Scale,
                SpriteEffects.None,
                0.5f);
            spriteBatch.Draw(
                testSprite,
                DrawPosition + Hurtbox.Offset,
                Hurtbox.Box,
                Color.White,
                0f,
                Vector2.Zero,
                Scale,
                SpriteEffects.None,
                0f);

            foreach (Projectile projectile in Projectiles) {
                projectile.DrawDebug(spriteBatch, testSprite);
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

            Health -= CurrentAnimation.CurrentDamageTaken;
        }

        /// <summary>
        /// Reset the Character's attributes.
        /// </summary>
        public void Reset(bool isPlayer2 = false) {
            ChangeState((int)Actions.Idle);
            if (!isPlayer2)
                Position = Battlefield.Player1StartingPos;
            else
                Position = Battlefield.Player2StartingPos;

            Health = MaxHealth;
        }
    }
}
