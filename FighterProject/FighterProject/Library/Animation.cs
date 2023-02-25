using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FighterProject.Library {
    /// <summary>
    /// This class represents a definition for an Animation. An animation iterates
    /// over it's defined spriteLocations every time Animate() is called. The 
    /// speed of the animation is determined by the animation's framerate.
    /// </summary>
    class Animation {
        /// <summary>
        /// The index of the current frame of the animation.
        /// </summary>
        public int CurrentIndex { get; private set; }

        /// <summary>
        /// The current sprite location for the current sprite.
        /// </summary>
        public Rectangle CurrentLocation { get; private set; }

        /// <summary>
        /// The curent hurtbox for the current sprite.
        /// </summary>
        public Hurtbox CurrentHurtbox { get; private set; }

        /// <summary>
        /// The current hitbox for the current sprite.
        /// </summary>
        public Hitbox CurrentHitbox { get; private set; }

        /// <summary>
        /// The offset from the sprite location to draw.
        /// </summary>
        public Vector2 CurrentOffset { get; private set; }

        /// <summary>
        /// The amount to change the position by.
        /// </summary>
        public Vector2 CurrentPositionChange { get; private set; }

        /// <summary>
        /// The current velocity.
        /// </summary>
        public Vector2 CurrentVelocity { get; private set; }

        /// <summary>
        /// True if the velocity should be modified/used.
        /// </summary>
        public bool CurrentSetVelocity { get; private set; }

        /// <summary>
        /// The current damage taken on this frame of the animation.
        /// </summary>
        public int CurrentDamageTaken { get; private set; }

        /// <summary>
        /// An animation to perform at the same time as the current
        /// animation.
        /// </summary>
        public Animation OpponentAnimation { get; private set; }

        /// <summary>
        /// The total frames of the animation.
        /// </summary>
        public int TotalFrames { get; private set; }

        /// <summary>
        /// True if the animation is finished and doesn't loop.
        /// </summary>
        public bool IsFinished { get; private set; }

        /// <summary>
        /// True if we should change animations once it is finished.
        /// </summary>
        public bool StopOnFinish { get; private set; }

        /// <summary>
        /// True if we should change animations once we land.
        /// </summary>
        public bool StopOnLanding { get; private set; }

        /// <summary>
        /// True if we check our conditions at the end of the animation.
        /// </summary>
        public bool CheckForLandingBeforeFinish { get; private set; }

        private readonly List<Frame> _frames;
        private readonly int _frameRate;
        private readonly bool _loop;
        private int _frameCounter;

        /// <summary>
        /// An Animation that can iterate through frame
        /// objects containing data to animate the 
        /// character on each frame.
        /// </summary>
        /// <param name="frames">The list of frames.</param>
        /// <param name="opponentAnimation">The opponent's animation.</param>
        /// <param name="frameRate">The frame rate.</param>
        /// <param name="loop">True if animation should loop.</param>
        /// <param name="stopOnFinish">True if we stop when animation finishes.</param>
        /// <param name="stopOnLanding">True if we stop the animation when landing.</param>
        /// <param name="checkForLandingBeforeFinish">True if we want to check if we landed before the animation finishes.</param>
        public Animation(List<Frame> frames, Animation opponentAnimation = null, int frameRate = 1, bool loop = false, bool stopOnFinish = true, bool stopOnLanding = false, bool checkForLandingBeforeFinish = false) {
            CurrentIndex = 0;
            CurrentLocation = frames[0].SpriteLocation;
            CurrentHurtbox = frames[0].Hurtbox;
            CurrentHitbox = frames[0].Hitbox;
            CurrentOffset = frames[0].Offset;
            CurrentPositionChange = frames[0].PositionChange;
            CurrentVelocity = frames[0].Velocity;
            CurrentSetVelocity = frames[0].SetVelocity;
            CurrentDamageTaken = frames[0].DamageTaken;

            _frames = frames;
            TotalFrames = frames.Count - 1;
            _frameRate = frameRate;
            _loop = loop;
            // Make frame counter the frame rate so it will update all the fields on the first run.
            _frameCounter = frameRate;
            IsFinished = false;
            StopOnFinish = stopOnFinish;
            StopOnLanding = stopOnLanding;
            CheckForLandingBeforeFinish = checkForLandingBeforeFinish;

            if(opponentAnimation != null) {
                OpponentAnimation = opponentAnimation;
            }

        }

        /// <summary>
        /// Animates the current Animation at the
        /// speed of the Animation's frame rate.
        /// </summary>
        /// <returns>True if fields should be updated.</returns>
        public bool Animate() {
            if (IsFinished) { return false; }

            bool update = false;
            // Iterate until we reach our frameRate limit.
            if (_frameCounter >= _frameRate) {

                if (CurrentIndex > TotalFrames && _loop) {
                    // Loop animation
                    CurrentIndex = 0;
                }
                else if (CurrentIndex > TotalFrames && !_loop) {
                    // Stay on last frame
                    CurrentIndex = TotalFrames;
                    IsFinished = true;
                }

                // Update fields
                _frameCounter = -1;
                CurrentLocation = _frames[CurrentIndex].SpriteLocation;
                CurrentHurtbox = _frames[CurrentIndex].Hurtbox;
                CurrentHitbox = _frames[CurrentIndex].Hitbox;
                CurrentOffset = _frames[CurrentIndex].Offset;
                CurrentPositionChange = _frames[CurrentIndex].PositionChange;
                CurrentVelocity = _frames[CurrentIndex].Velocity;
                CurrentSetVelocity = _frames[CurrentIndex].SetVelocity;
                CurrentDamageTaken = _frames[CurrentIndex].DamageTaken;
                update = true;

                // Change our current frame. 
                CurrentIndex++;
            }
            _frameCounter++;
            return update;
        }

        /// <summary>
        /// Reset the current animation to the beginning
        /// of it's animation.
        /// </summary>
        public void Reset() {
            CurrentIndex = 0;
            _frameCounter = _frameRate;
            IsFinished = false;
            CurrentLocation = _frames[0].SpriteLocation;
            CurrentHurtbox = _frames[0].Hurtbox;
            CurrentHitbox = _frames[0].Hitbox;
            CurrentOffset = _frames[0].Offset;
            CurrentPositionChange = _frames[0].PositionChange;
            CurrentVelocity = _frames[0].Velocity;
            CurrentSetVelocity = _frames[0].SetVelocity;
            CurrentDamageTaken = _frames[0].DamageTaken;
        }
    }
}
