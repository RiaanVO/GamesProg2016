using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRedesign
{
   

    class AiFSM
    {
        private string currentState;
        private Dictionary<string, AiState> states = new Dictionary<string, AiState>();
        private bool stateJustChanged;
        //Used to make sure that there is a state when updating and not accessing a null state
        private bool stateExists;

        public string CurrentState {
            get { return currentState; }
        }

        public AiFSM(string startingState)
        {
            currentState = startingState;
            stateExists = false;
        }

        public bool StateJustChanged {
            get { return stateJustChanged; }
        }

        /// <summary>
        /// Sets the state that will be updated
        /// </summary>
        /// <param name="nextState"></param>
        public void setState(string nextState)
        {
            stateExists = false;
            if (states.ContainsKey(nextState))
            {
                currentState = nextState;
                stateExists = true;
                stateJustChanged = true;
            }
        }

        public void addState(AiState state) {
            if (state.Name.Equals(currentState))
                stateExists = true;
            states.Add(state.Name, state);
        }

        /// <summary>
        /// Checks the transitions for the current active state
        /// </summary>
        /// <param name="gameTime"></param>
        public void update(GameTime gameTime)
        {
            if (stateExists)
            {
                stateJustChanged = false;
                AiState state = states[currentState];
                state.update(gameTime);
                state.checkTransitions();
                
            }
        }
    }
}
