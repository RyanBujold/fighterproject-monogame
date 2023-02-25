namespace FighterProject.Library {
    class Timer {

        /// <summary>
        /// The Timer's current time.
        /// </summary>
        public int Time { 
            get { return (int)time; } 
        }

        private float time { get; set; }

        /// <summary>
        /// True if the Timer is finished counting down.
        /// </summary>
        public bool IsFinished { get; private set; }

        private bool _isCountdown = false;
        private readonly float startingCoundownTime;

        /// <summary>
        /// A Timer.
        /// </summary>
        /// <param name="countdownTime">The time to countdown from. If nothing is given, timer counts up.</param>
        public Timer(float countdownTime = -1) {
            if(countdownTime >= 0) { _isCountdown = true; }
            IsFinished = false;
            time = countdownTime;
            startingCoundownTime = countdownTime;
        }

        /// <summary>
        /// Update the timer.
        /// </summary>
        /// <param name="timepassed">The time passed.</param>
        public void Update(float timepassed) {
            // If we are done counting down, we are finished and stop counting down.
            if(_isCountdown && time <= 0) {
                IsFinished = true;
                return;
            }

            if (_isCountdown)
                time -= timepassed/1000;
            else
                time += timepassed/1000;

        }

        /// <summary>
        /// Reset our timer.
        /// </summary>
        public void Reset() {
            if (_isCountdown) {
                time = startingCoundownTime;
            }
            else {
                time = 0;
            }
            IsFinished = false;
        }

    }
}
