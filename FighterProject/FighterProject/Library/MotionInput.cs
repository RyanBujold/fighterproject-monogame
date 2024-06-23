using System.Collections.Generic;
using static FighterProject.Entities.Player;

namespace FighterProject.Library {

    /// <summary>
    /// A sequence of directions that after inputed would 
    /// perform an action. This class checks if the inputs 
    /// needed to perform the full input are performed within 
    /// the buffer time frame.
    /// </summary>
    internal class MotionInput {
        /// <summary>
        /// Directional inputs based on the direction facing.
        /// </summary>
        public enum DirectionalInputs {
            UB,
            U,
            UF,
            B,
            F,
            DB,
            D,
            DF,
            Btn_1,
            Btn_2,
            Btn_3,
        }
        private List<DirectionalInputs> inputList;
        private int checkIndex;
        private readonly int buffer = 15;
        private int bufCounter;

        /// <summary>
        /// A motion input.
        /// </summary>
        /// <param name="inputs">The inputs to perform.</param>
        public MotionInput(List<DirectionalInputs> inputs) {
            inputList = inputs;
            checkIndex = 0;
            bufCounter = 0;
        }

        /// <summary>
        /// Checks if the inputs were all performed correctly.
        /// </summary>
        /// <param name="inputs">The inputs to check.</param>
        /// <param name="isFacingRight">True if the character performing the move is facing to the right.</param>
        /// <returns></returns>
        public bool CheckInputs(List<Input> inputs, bool isFacingRight) {
            bool passed = false;
            List<DirectionalInputs> directions = translateInputs(inputs, isFacingRight);
            // Check if the current inputs have the next input we need
            if (directions.Contains(inputList[checkIndex])) {
                // If we haven't reached the end of the required input list, increment
                if(checkIndex < inputList.Count - 1) { 
                    checkIndex++;
                }
                else{
                    passed = true;
                    checkIndex = 0;
                }
            }
            else if (bufCounter > buffer) {
                checkIndex = 0;
                bufCounter = 0;
            }
            else {
                bufCounter++;
            }

            return passed;
        }

        private List<DirectionalInputs> translateInputs(List<Input> inputs, bool isFacingRight) {
            List<DirectionalInputs> result = new List<DirectionalInputs>();

            foreach (Input i in inputs) {
                // Determine the directions based on which direction we are facing and the inputs givenss
                if(isFacingRight) {
                    switch (i) {
                        case Input.UL:
                            result.Add(DirectionalInputs.UB);
                            break;
                        case Input.U:
                            result.Add(DirectionalInputs.U);
                            break;
                        case Input.UR:
                            result.Add(DirectionalInputs.UF);
                            break;
                        case Input.L:
                            result.Add(DirectionalInputs.B);
                            break;
                        case Input.R:
                            result.Add(DirectionalInputs.F);
                            break;
                        case Input.DL:
                            result.Add(DirectionalInputs.DB);
                            break;
                        case Input.D:
                            result.Add(DirectionalInputs.D);
                            break;
                        case Input.DR:
                            result.Add(DirectionalInputs.DF);
                            break;
                    }
                }
                else {
                    switch (i) {
                        case Input.UL:
                            result.Add(DirectionalInputs.UF);
                            break;
                        case Input.U:
                            result.Add(DirectionalInputs.U);
                            break;
                        case Input.UR:
                            result.Add(DirectionalInputs.UB);
                            break;
                        case Input.L:
                            result.Add(DirectionalInputs.F);
                            break;
                        case Input.R:
                            result.Add(DirectionalInputs.B);
                            break;
                        case Input.DL:
                            result.Add(DirectionalInputs.DF);
                            break;
                        case Input.D:
                            result.Add(DirectionalInputs.D);
                            break;
                        case Input.DR:
                            result.Add(DirectionalInputs.DB);
                            break;
                    }
                }
                // Buttons
                switch (i) {
                    case Input.Button_1:
                        result.Add(DirectionalInputs.Btn_1);
                        break;
                    case Input.Button_2:
                        result.Add(DirectionalInputs.Btn_2);
                        break;
                    case Input.Button_3:
                        result.Add(DirectionalInputs.Btn_3);
                        break;
                }

            }

            return result;
        }

    }
}
