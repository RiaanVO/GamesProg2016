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

        public string CurrentState {
            get { return currentState; }
        }

        public AiFSM(string startingState)
        {
            currentState = startingState;
        }

        /// <summary>
        /// Sets the state that will be updated
        /// </summary>
        /// <param name="nextState"></param>
        public void setState(string nextState)
        {
            if (states.ContainsKey(nextState))
            {
                currentState = nextState;
            }
        }

        public void addState(AiState state) {
            states.Add(state.Name, state);
        }

        /// <summary>
        /// Checks the transitions for the current active state
        /// </summary>
        /// <param name="gameTime"></param>
        public void update(GameTime gameTime)
        {
            if (states.ContainsKey(currentState))
            {
                states[currentState].checkTransitions();
            }
        }
    }
}
