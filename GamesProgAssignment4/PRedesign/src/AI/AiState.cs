using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PRedesign
{
    class AiState
    {
        private AiFSM fsm;
        private string name;
        //In FSM dict name, not needed maybe
        //private STATES name;
        //private Dictionary<TRANSITIONS, AiTransition> transitions = new Dictionary<TRANSITIONS, AiTransition>();
        private List<AiTransition> transitions = new List<AiTransition>();
        private Action<GameTime> updateMethod;

        public string Name {
            get { return name; }
        }

        public AiState(AiFSM fsm, string name, Action<GameTime> updateMethod) {
            this.fsm = fsm;
            this.name = name;
            this.updateMethod = updateMethod;
        }

        public void update(GameTime gameTime)
        {
            updateMethod(gameTime);
        }

        public void addTransition(string targetState, Func<bool> condition) {
            transitions.Add(new AiTransition(targetState, condition));
        }

        public void checkTransitions() {
            foreach (AiTransition transition in transitions) {
                if (transition.CheckCondition)
                    fsm.setState(transition.TargetState);
            }
        }
        /* Dictionary code
        public void addTransition(TRANSITIONS transition, STATES state, Func<bool> condition) {
            transitions.Add(transition, new AiTransition(state, condition));
        }

        /// <summary>
        /// Loop through all the transitions, and if one fires set the state to that.
        /// </summary>
        public void checkTransitions() {
            foreach (KeyValuePair<TRANSITIONS, AiTransition> pair in transitions) {
                if (pair.Value.CheckCondition) {
                    fsm.setState(pair.Value.TargetState);
                    return;
                }
            }
        }
        */
    }
}
