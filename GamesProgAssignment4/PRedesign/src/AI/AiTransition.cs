using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRedesign
{
    class AiTransition
    {
        //Its contatined in a state dictionary
        //private TRANSITIONS name;
        public string TargetState { get; private set; }
        private Func<bool> condition;

        public bool CheckCondition {
            get { return condition(); }
        }

        //public STATES TargetState

        public AiTransition(string TargetState, Func<bool> condition) {
            this.TargetState = TargetState;
            this.condition = condition;
        }
    }
}
