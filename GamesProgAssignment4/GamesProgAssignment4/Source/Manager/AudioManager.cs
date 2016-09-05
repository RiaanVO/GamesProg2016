using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace GamesProgAssignment4
{
    class AudioManager : GameComponent
    {
        AudioListenerComponet listenerComponent;
        List<AudioEmitterComponent> emitterComponents;
        public AudioManager(Game game) : base(game)
        {
            emitterComponents = new List<AudioEmitterComponent>();
        }

        /// <summary>
        /// Sets the audio listener of the game
        /// </summary>
        /// <param name="audioListenerComponent"></param>
        public void setListenerComponent(AudioListenerComponet audioListenerComponent) {
            listenerComponent = audioListenerComponent;
        }

        /// <summary>
        /// Adds a audio emitter to the list of emitters
        /// </summary>
        /// <param name="audioEmitterComponent"></param>
        public void addEmitterComponent(AudioEmitterComponent audioEmitterComponent) {
            emitterComponents.Add(audioEmitterComponent);
        }

        /// <summary>
        /// Updates all the audio emitters and the audio listener
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (listenerComponent == null)
                return;
            bool listenerMoved = listenerComponent.checkHasMoved();
            if (listenerMoved)
                listenerComponent.UpdatePosition();
            foreach (AudioEmitterComponent emitter in emitterComponents) {
                if (emitter.checkHasMoved() || listenerMoved) {
                    emitter.UpdatePosition();
                    emitter.update3DAudio(listenerComponent.listener);
                }
            }

            base.Update(gameTime);
        }

        public void reset() {
            foreach (AudioEmitterComponent aec in emitterComponents) {
                aec.stopAll();
            }
        }
    }
}
