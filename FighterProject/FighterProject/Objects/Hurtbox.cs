
namespace FighterProject.Library {

    /// <summary>
    /// Defines a hurtbox that indicates wether the opponent is defending or not.
    /// </summary>
    class Hurtbox : CollisionBox {

        /// <summary>
        /// The defensive option from the hurtbox.
        /// </summary>
        public DefenseState BlockState { get; private set; }

        /// <summary>
        /// An empty hitbox.
        /// </summary>
        public static Hurtbox None = new Hurtbox(0, 0, 0, 0);

        public enum DefenseState {
            None,
            Blocking,
        }

        /// <summary>
        /// A hurtbox.
        /// </summary>
        /// <param name="Xoffset">The X offset.</param>
        /// <param name="Yoffset">The Y offset.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="blockState">The blocking state.</param>
        public Hurtbox(int Xoffset, int Yoffset, int width, int height, DefenseState blockState = DefenseState.None) : base(Xoffset, Yoffset, width, height) {
            BlockState = blockState;
        }

    }
}