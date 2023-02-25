using Microsoft.Xna.Framework;

namespace FighterProject.Library {
    /// <summary>
    /// A Frame of animation containing sprite, 
    /// hurtbox and hitbox information for animating.
    /// </summary>
    class Frame {
        /// <summary>
        /// The location of a sprite on the spritesheet.
        /// </summary>
        public Rectangle SpriteLocation { get; private set; }

        /// <summary>
        /// The character's current Hurtbox.
        /// </summary>
        public Hurtbox Hurtbox { get; private set; }

        /// <summary>
        /// The animation's current Hitbox.
        /// </summary>
        public Hitbox Hitbox { get; private set; }

        /// <summary>
        /// The character's current draw Offset.
        /// </summary>
        public Vector2 Offset { get; private set; }

        /// <summary>
        /// The character's current Velocity.
        /// </summary>
        public Vector2 Velocity { get; private set; }

        /// <summary>
        /// The amount to modify the character's position by.
        /// </summary>
        public Vector2 PositionChange { get; private set; }

        /// <summary>
        /// True if we want to modify the velocity, otherwise don't.
        /// </summary>
        public bool SetVelocity { get; private set; }

        /// <summary>
        /// The damage to take on this frame.
        /// </summary>
        public int DamageTaken { get; private set; }

        /// <summary>
        /// A Frame of animation.
        /// </summary>
        /// <param name="spriteLocation">The location of a sprite on the spritesheet.</param>
        /// <param name="hurtbox">A hurtbox definition.</param>
        /// <param name="hitbox">A hitbox definition.</param>
        /// <param name="Xoffset">The X offset for the frame.</param>
        /// <param name="Yoffset">The Y offset for the frame.</param>
        /// <param name="Xmovement">The X amount to move horizontally.</param>
        /// <param name="Ymovement">The Y amount to move vertically.</param>
        /// <param name="Xvelocity">The X velocity.</param>
        /// <param name="Yvelocity">The Y velocity.</param>
        /// <param name="setVelocity">True if we want to modify velocity.</param>
        public Frame(Rectangle spriteLocation, Hurtbox hurtbox = null, Hitbox hitbox = null, 
            float Xoffset = 0, float Yoffset = 0, 
            float Xmovement = 0, float Ymovement = 0, 
            float Xvelocity = 0, float Yvelocity = 0, bool setVelocity = false,
            int damageTaken = 0) {

            SpriteLocation = spriteLocation;
            Hurtbox = hurtbox ?? Hurtbox.None;
            Hitbox = hitbox ?? Hitbox.None;
            Offset = new Vector2(Xoffset, Yoffset);
            PositionChange = new Vector2(Xmovement, Ymovement);
            Velocity = new Vector2(Xvelocity, Yvelocity);
            SetVelocity = setVelocity;
            DamageTaken = damageTaken;
        }

    }
}
