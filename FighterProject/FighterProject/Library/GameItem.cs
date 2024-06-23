using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace FighterProject.Library {

    abstract class GameItem {

        /// <summary>
        /// The Game Item's spritesheet.
        /// </summary>
        public Texture2D Sprite { get; private set; }

        /// <summary>
        /// The Game Item's Position on the spritesheet.
        /// </summary>
        public Rectangle SpritePos { get; protected set; }

        /// <summary>
        /// The Game Item's draw position.
        /// </summary>
        public Vector2 DrawPosition { get; protected set; }

        /// <summary>
        /// The Game Item's Hurtbox.
        /// </summary>
        public Hurtbox Hurtbox { get; protected set; }

        /// <summary>
        /// The Game Item's Hitbox.
        /// </summary>
        public Hitbox Hitbox { get; protected set; }

        /// <summary>
        /// The Game Item's current position on the screen
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The Game Item's current velocity.
        /// </summary>
        public Vector2 Velocity;

        /// <summary>
        /// The Game Item's offset.
        /// </summary>
        public Vector2 Offset { get; protected set; }

        /// <summary>
        /// The Game Item's Sprite Effects.
        /// </summary>
        public SpriteEffects CurrentEffects { get; private set; }

        /// <summary>
        /// The Game Item's Animation.
        /// </summary>
        public Animation CurrentAnimation { get; set; }

        /// <summary>
        /// The Game Item's draw scale.
        /// </summary>
        public float Scale { get; private set; }

        /// <summary>
        /// True if the Game Item is being drawn facing right.
        /// </summary>
        public bool IsFacingRight { get; private set; }

        /// <summary>
        /// The starting direction the Game Item is facing.
        /// </summary>
        protected bool StartingFacingRight { get; set; }

        /// <summary>
        /// The scaled Game Item width.
        /// </summary>
        public int ScaledSpriteWidth {
            get {
                return SpritePos.Width * (int)Scale;
            }
        }

        /// <summary>
        /// The scaled Game Item height.
        /// </summary>
        public int ScaledSpriteHeight {
            get {
                return SpritePos.Height * (int)Scale;
            }
        }

        /// <summary>
        /// The Game Item's list of animations.
        /// </summary>
        protected List<Animation> Animations = new List<Animation>();

        protected int Width;
        protected int Height;

        /// <summary>
        /// A Game Item.
        /// </summary>
        /// <param name="sprite">The Spritesheet.</param>
        /// <param name="scale">The draw scale.</param>
        /// <param name="isFacingRight">True if we begin drawing facing right.</param>
        public GameItem(Texture2D sprite, float scale = 1f, bool isFacingRight = true) {
            Sprite = sprite;
            DrawPosition = new Vector2(0, 0);
            Hurtbox = new Hurtbox(0, 0, 0, 0);
            Hitbox = new Hitbox(0, 0, 0, 0, 0, 0);
            Velocity = new Vector2(0, 0);
            Offset = new Vector2(0, 0);
            Scale = scale;
            IsFacingRight = isFacingRight;
            StartingFacingRight = isFacingRight;
            if (isFacingRight) { CurrentEffects = SpriteEffects.None; }
            else { CurrentEffects = SpriteEffects.FlipHorizontally; }
        }

        /// <summary>
        /// Update the Game Item's parameters when called.
        /// </summary>
        public virtual void Update() {

            // -- Animation -- //
            if (CurrentAnimation.Animate())
                UpdateFields();

            // Draw
            DrawPosition = new Vector2(Position.X - ScaledSpriteWidth / 2, Position.Y - ScaledSpriteHeight);

            // Make sure velocity is correct
            if (CurrentAnimation.CurrentSetVelocity) {
                if (IsFacingRight)
                    Velocity = new Vector2(CurrentAnimation.CurrentVelocity.X, -CurrentAnimation.CurrentVelocity.Y);
                else
                    Velocity = new Vector2(-CurrentAnimation.CurrentVelocity.X, -CurrentAnimation.CurrentVelocity.Y);
            }
        }

        /// <summary>
        /// Draw the Game Item on the screen.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(
                Sprite,
                DrawPosition + Offset,
                SpritePos,
                Color.White,
                0f,
                Vector2.Zero,
                Scale,
                CurrentEffects,
                1f);
        }

        /// <summary>
        /// Draw the Game Item's debug items.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        /// <param name="testSprite">The test sprite.</param>
        public virtual void DrawDebug(SpriteBatch spriteBatch, Texture2D testSprite) {
            // Draw a red square at the position value of the Game Item.
            spriteBatch.Draw(
                testSprite,
                Position - new Vector2(2.5f * 4f, 2.5f * 4f),
                new Rectangle(0, 0, 5, 5),
                Color.Red,
                0f,
                Vector2.Zero,
                4f,
                SpriteEffects.None,
                0.7f);
            // Draw a rectangle representing the full sprite's size
            /*spriteBatch.Draw(
                testSprite,
                DrawPosition + Offset,
                SpritePos,
                Color.Orange,
                0f,
                Vector2.Zero,
                Scale,
                SpriteEffects.None,
                0.3f);*/
        }

        /// <summary>
        /// Turn the Game Item around.
        /// </summary>
        public void TurnAround() {
            if (IsFacingRight) { IsFacingRight = false; }
            else { IsFacingRight = true; }
        }

        /// <summary>
        /// Flip the Game Item's sprite.
        /// </summary>
        public void FlipSprite() {
            if (IsFacingRight) { CurrentEffects = SpriteEffects.None; }
            else { CurrentEffects = SpriteEffects.FlipHorizontally; }
        }

        /// <summary>
        /// Change the Game Item's animation.
        /// </summary>
        /// <param name="id">The id on the animation.</param>
        /// <param name="reset">True if we reset the animation.</param>
        public void ChangeAnimation(int id, bool reset = true) {
            if (CurrentAnimation != Animations[id]) {
                CurrentAnimation.Reset();
                CurrentAnimation = Animations[id];
            }
            else if (reset)
                CurrentAnimation.Reset();

            UpdateFields();
        }

        /// <summary>
        /// Change the Game Item's animation.
        /// </summary>
        /// <param name="animation">The animation.</param>
        /// <param name="reset">True if we reset the animation.</param>
        public void ChangeAnimation(Animation animation, bool reset = true) {
            if (CurrentAnimation != animation) {
                CurrentAnimation.Reset();
                CurrentAnimation = animation;
            }
            else if (reset)
                CurrentAnimation.Reset();

            UpdateFields();
        }

        /// <summary>
        /// Update the Game Item's fields.
        /// </summary>
        protected virtual void UpdateFields() {
            // Get which frame to draw on our spritesheet.
            SpritePos = CurrentAnimation.CurrentLocation;
            Offset = CurrentAnimation.CurrentOffset;
            Hurtbox = CurrentAnimation.CurrentHurtbox;
            Hitbox = CurrentAnimation.CurrentHitbox;

            // Turn Around Logic
            Vector2 posChange = new Vector2(CurrentAnimation.CurrentPositionChange.X, -CurrentAnimation.CurrentPositionChange.Y);
            if (!IsFacingRight) {
                // Flip Offset
                Offset = new Vector2(-Offset.X - SpritePos.Width + Width, Offset.Y);
                // Flip Position Change
                posChange = new Vector2(-posChange.X, posChange.Y);
                // Flip hurtboxes
                int x = (int)Hurtbox.Offset.X - Hurtbox.Width + Width;
                int y = (int)Hurtbox.Offset.Y;
                Hurtbox = new Hurtbox(x, y, Hurtbox.Width, Hurtbox.Height, Hurtbox.BlockState);
                // Flip hitboxes
                int x2 = (int)-Hitbox.Offset.X - Hitbox.Width + Width;
                int y2 = (int)Hitbox.Offset.Y;
                Hitbox = new Hitbox(x2, y2, Hitbox.Width, Hitbox.Height, Hitbox.Damage, Hitbox.Hitstun);
            }

            // Update fields
            Position += posChange;
        }

        /// <summary>
        /// Reset the Game Item to it's originally defined start.
        /// </summary>
        public virtual void Reset() {
            DrawPosition = new Vector2(0, 0);
            Hurtbox = new Hurtbox(0, 0, 0, 0);
            Hitbox = new Hitbox(0, 0, 0, 0, 0, 0);
            Velocity = new Vector2(0, 0);
            Offset = new Vector2(0, 0);
            IsFacingRight = StartingFacingRight;
            if (StartingFacingRight) { CurrentEffects = SpriteEffects.None; }
            else { CurrentEffects = SpriteEffects.FlipHorizontally; }
            CurrentAnimation.Reset();
        }
    }
}
