
namespace FighterProject.Library {

    /// <summary>
    /// A hitbox that defines an attack's type and damage.
    /// </summary>
    class Hitbox : CollisionBox{

        /* Add attack types. Ex. high attack, low attack, mid attack...*/

        /// <summary>
        /// The id representing the hitbox group.
        /// </summary>
        public int GroupId { get; private set; }

        /// <summary>
        /// The damage done upon collision.
        /// </summary>
        public int Damage { get; private set; }

        /// <summary>
        /// The number of frames out of 60 to apply for hitstun.
        /// </summary>
        public int Hitstun { get; private set; }

        /// <summary>
        /// An empty hitbox.
        /// </summary>
        public static Hitbox None = new Hitbox(0, 0, 0, 0, 0, 0);

        /// <summary>
        /// An attack hitbox.
        /// </summary>
        /// <param name="Xoffset">The X offset.</param>
        /// <param name="Yoffset">The Y offset.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="damage">The attack damage.</param>
        /// <param name="id">The hitboxe's group id.</param>
        public Hitbox(int Xoffset, int Yoffset, int width, int height, int damage, int hitstun, int id = 1) : base(Xoffset, Yoffset, width, height) {
            Damage = damage;
            Hitstun = hitstun;
            GroupId = id;
        }

        /// <summary>
        /// Check if hitboxes are the same.
        /// </summary>
        /// <param name="obj">The hitbox.</param>
        /// <returns>True if hitboxes are the same.</returns>
        public bool Equals(Hitbox col) {        
            // Don't check the offset because it changes when the player is facing left.
            if (Width == col.Width &&
               Height == col.Height &&
               Damage == col.Damage)
                return true;
            else
                return false;

        }

    }
}
