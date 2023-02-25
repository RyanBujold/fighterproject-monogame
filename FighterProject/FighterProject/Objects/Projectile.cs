using FighterProject.Entities.Characters;
using FighterProject.Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static FighterProject.Entities.Characters.Character;

namespace FighterProject.Objects {
    abstract class Projectile : GameItem {

        /// <summary>
        /// True if the projecile is active.
        /// </summary>
        public bool Active { get; set; }

        public int ScaledHitboxWidth {
            get {
                return Hitbox.Width * (int)Scale;
            }
        }

        public int ScaledHitboxHeight {
            get {
                return Hitbox.Height * (int)Scale;
            }
        }

        protected float MoveSpeed = 2f;
        protected int _projectileWidth = 50;
        protected int _projectileHeight = 50;
        protected Character _char;

        /// <summary>
        /// A Projectile.
        /// </summary>
        /// <param name="character">A Character.</param>
        /// <param name="Xstartpos">The starting X coordinate.</param>
        /// <param name="Ystartpos">The starting Y coordinate.</param>
        public Projectile(Character character, float Xstartpos, float Ystartpos) : base(character.Sprite, character.Scale, character.IsFacingRight) {
            _char = character;
            Xstartpos *= character.Scale;
            Ystartpos *= character.Scale;
            if (!character.IsFacingRight) { Xstartpos = character.ScaledWidth - Xstartpos; }
            Position = new Vector2(character.DrawPosition.X + Xstartpos, character.DrawPosition.Y + Ystartpos);
            Width = _projectileWidth;
            Height = _projectileHeight;
            Active = true;
            if (character.IsFacingRight) { Velocity.X = 1; }
            else { Velocity.X = -1; }
        }

        public void Update(float timepassed) {
            base.Update();

            // Movement logic.
            float movementSpeed = timepassed / MoveSpeed;
            Position.X += Velocity.X * movementSpeed;
        }

        public override void DrawDebug(SpriteBatch spriteBatch, Texture2D testSprite) {
            base.DrawDebug(spriteBatch, testSprite);

            spriteBatch.Draw(
                testSprite,
                DrawPosition + Hitbox.Offset,
                Hitbox.Box,
                Color.Purple,
                0f,
                Vector2.Zero,
                Scale,
                SpriteEffects.None,
                0.6f);
        }

    }
}
