using FighterProject.Entities.Characters;
using Microsoft.Xna.Framework;

namespace FighterProject.Library {
    /// <summary>
    /// A CollisionBox that represents a rectangle used for
    /// various collision detection.
    /// </summary>
    class CollisionBox {

        /// <summary>
        /// The offset to draw the collision box at.
        /// </summary>
        public Vector2 Offset { get; set; }

        /// <summary>
        /// A rectangle representing the collision box.
        /// </summary>
        public Rectangle Box { get; private set; }

        /// <summary>
        /// The width of the collision box.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// The height of the collision box.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// A Collision Box object.
        /// </summary>
        /// <param name="Xoffset">The X offset.</param>
        /// <param name="Yoffset">The Y offset.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public CollisionBox(float Xoffset, float Yoffset, int width, int height) {
            Offset = new Vector2(Xoffset, Yoffset);
            Box = new Rectangle(0, 0, width, height);
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Check if attacker's collision box collided with defender's collision box.
        /// </summary>
        /// <param name="atkDrawPos">The attacker's draw position.</param>
        /// <param name="defDrawPos">The defender's draw position.</param>
        /// <param name="atkBox">The attacker's collision box.</param>
        /// <param name="defBox">The defender's collision box.</param>
        /// <param name="atkScale">The attacker's scale.</param>
        /// <param name="defScale">The defener's scale.</param>
        /// <returns>True if the CollisionBoxes have collided. False otherwise.</returns>
        public static bool DidCollide(Vector2 atkDrawPos, Vector2 defDrawPos, CollisionBox atkBox, CollisionBox defBox, float atkScale, float defScale) {
            // Return if the is no hitbox.
            if(atkBox.Width == 0 && atkBox.Height == 0){ return false; }
            if(defBox.Width == 0 && defBox.Height == 0){ return false; }

            // Setup variables (Don't scale the offset because we have draw positions)
            float atkLeft = atkDrawPos.X + atkBox.Offset.X;
            float atkRight = atkDrawPos.X + atkBox.Width + atkBox.Offset.X;
            float atkBottom = atkDrawPos.Y + atkBox.Height + atkBox.Offset.Y;
            float atkTop = atkDrawPos.Y + atkBox.Offset.Y;

            float defLeft = defDrawPos.X + defBox.Offset.X;
            float defRight = defDrawPos.X + defBox.Width + defBox.Offset.X;
            float defBottom = defDrawPos.Y + defBox.Height + defBox.Offset.Y;
            float defTop = defDrawPos.Y + defBox.Offset.Y;

            // Check collision
            if (atkRight >= defLeft && atkLeft <= defRight && atkBottom >= defTop && atkTop <= defBottom) {
                return true;
            }
            return false;
        }

    }
}
