using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRedesign
{
    class AiState
    {
        private AiFSM fsm;
        private string name;
        private Dictionary<string, Func<bool>> transitions = new Dictionary<string, Func<bool>>();

        public string Name {
            get { return name; }
        }

        public AiState(AiFSM fsm, string name) {
            this.fsm = fsm;
            this.name = name;
        }

        public void addTransition(string nextState, Func<bool> condition) {
            transitions.Add(nextState, condition);
        }

        /// <summary>
        /// Loop through all the transitions, and if one fires set the state to that.
        /// </summary>
        public void checkTransitions() {
            foreach (KeyValuePair<string, Func<bool>> pair in transitions) {
                if (pair.Value()) {
                    fsm.setState(pair.Key);
                    return;
                }
            }
        }
    }
}
