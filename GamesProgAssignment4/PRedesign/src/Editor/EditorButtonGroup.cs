using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRedesign {

    class EditorButtonGroup {
        #region Fields
        private IList<EditorOptionButton> buttons = new List<EditorOptionButton>();
        #endregion

        #region Initialisation
        public EditorButtonGroup() {

        }
        #endregion

        #region Helper Methods
        public void AddButton(EditorOptionButton button) {
            buttons.Add(button);
        }

        public void RemoveButton(EditorOptionButton button) {
            buttons.Remove(button);
        }

        public void ToggleButton(EditorOptionButton button) {
            foreach (EditorOptionButton btn in buttons) {
                if (btn == button) {
                    btn.Selected = true;
                } else {
                    btn.Selected = false;
                }
            }
        }
        #endregion
    }
}
