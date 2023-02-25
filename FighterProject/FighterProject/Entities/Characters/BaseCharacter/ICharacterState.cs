using System.Collections.Generic;
using static FighterProject.Entities.Player;

namespace FighterProject.Library {

    /// <summary>
    /// An interface representing a character state.
    /// </summary>
    interface ICharacterState {

        /// <summary>
        /// The character state's ID.
        /// </summary>
        abstract int ID { get; set; }

        /// <summary>
        /// Called when entering the Character State.
        /// </summary>
        public void Enter();

        /// <summary>
        /// Updates the Character when called.
        /// </summary>
        /// <param name="inputs">The inputs from the player.</param>
        public void Update(List<Input> inputs);

        /// <summary>
        /// Called when exiting the Character State.
        /// </summary>
        public void Exit();
    }
}
