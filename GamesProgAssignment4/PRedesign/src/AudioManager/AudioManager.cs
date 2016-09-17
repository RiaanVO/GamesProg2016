using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace PRedesign
{
    static class AudioManager
    {
        #region Fields
        private static AudioListenerComponent listenerComponent;
        private static List<AudioEmitterComponent> emitterComponents = new List<AudioEmitterComponent>();
        #endregion

        #region Properties
        public static AudioListener AudioListener {
            get { return listenerComponent.AudioListener; }
        }
        public static bool HasListener {
            get { return listenerComponent != null; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sets the listener of the audio system, there can only be one
        /// </summary>
        /// <param name="audioListenerComponent"></param>
        public static void setListenerCompoent(AudioListenerComponent audioListenerComponent) {
            listenerComponent = audioListenerComponent;
        }

        /// <summary>
        /// Adds an emitter to the list of emitter components
        /// </summary>
        /// <param name="audioEmitterComponent"></param>
        public static void addEmitterComponent(AudioEmitterComponent audioEmitterComponent) {
            emitterComponents.Add(audioEmitterComponent);
        }

        /// <summary>
        /// Removes an emitter from the list of emitter componets
        /// </summary>
        /// <param name="audioEmitterComponent"></param>
        public static void removeEmitterComponent(AudioEmitterComponent audioEmitterComponent) {
            emitterComponents.Remove(audioEmitterComponent);
        }

        /// <summary>
        /// Update the positions of the listener and all audio emitter compoents including the 3d audio
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Update(GameTime gameTime) {
            if (listenerComponent == null)
                return;
            bool listenerMoved = listenerComponent.HasMoved;
            if (listenerMoved)
                listenerComponent.UpdatePosition();
            foreach (AudioEmitterComponent emitter in emitterComponents) {

                bool emitterMoved = emitter.HasMoved;
                if (emitterMoved)
                    emitter.updatePosition();

                if (emitterMoved || listenerMoved)
                    emitter.update3DAudio(AudioListener);
            }
        }

        /// <summary>
        /// Stops all currently playing audio emitters
        /// </summary>
        public static void stopAll() {
            foreach (AudioEmitterComponent aec in emitterComponents)
                aec.stopAll();
        }

        /// <summary>
        /// Removes all emitter components and the listener componet is set to null
        /// </summary>
        public static void clearAll() {
            listenerComponent = null;
            emitterComponents.Clear();
        }
        #endregion

    }
}
