using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace GamesProgAssignment4
{
    class AudioEmitterComponent : AudioComponent
    {
        public AudioEmitter emitter;
        public bool hasMoved { get; protected set; }
        Dictionary<string, SoundEffect> soundEffects;
        Dictionary<string, SoundEffectInstance> soundEffectInstances;
        Dictionary<string, bool> instanceHas3D;

        public AudioEmitterComponent(Game game, GameObject gameObject) : base(game, gameObject)
        {
            emitter = new AudioEmitter();
            soundEffects = new Dictionary<string, SoundEffect>();
            soundEffectInstances = new Dictionary<string, SoundEffectInstance>();
            instanceHas3D = new Dictionary<string, bool>();
            game.Services.GetService<AudioManager>().addEmitterComponent(this);
        }

        //Update but not needed
        /*
        public override void Update()
        {
            if (checkHasMoved())
            {
                emitter.Position = gameObject.position;
            }
        }
        */

        /// <summary>
        /// Updates the position of the emitter
        /// </summary>
        public void UpdatePosition()
        {
            emitter.Position = gameObject.position;
        }

        /// <summary>
        /// Checks to see if the game object has moved from the sound emitters position
        /// </summary>
        /// <returns></returns>
        public bool checkHasMoved()
        {
            return hasMoved = emitter.Position != gameObject.position;
        }

        /// <summary>
        /// Updates the 3d properties of all the sound effect instances
        /// </summary>
        /// <param name="listener"></param>
        public void update3DAudio(AudioListener listener) {
            foreach (KeyValuePair<string, SoundEffectInstance> pair in soundEffectInstances) {
                if(instanceHas3D[pair.Key])
                    pair.Value.Apply3D(listener, emitter);
            }
        }

        /// <summary>
        /// Adds a sound effect to the dictionaries
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="soundEffect"></param>
        public void addSoundEffect(string tag, SoundEffect soundEffect) {
            if(!soundEffects.ContainsKey(tag))
                soundEffects.Add(tag, soundEffect);
        }

        /// <summary>
        /// Creates a sound effect and an instance of that effect
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="soundEffect"></param>
        public void createSoundEffectInstance(string tag, SoundEffect soundEffect, bool is3D) {
            if (!soundEffects.ContainsKey(tag))
                soundEffects.Add(tag, soundEffect);
            createSoundEffectInstance(tag, is3D);
        }

        /// <summary>
        /// Creates a sound effect instance from an already created sound effect
        /// </summary>
        /// <param name="tag"></param>
        public void createSoundEffectInstance(string tag, bool is3D) {
            if (soundEffects.ContainsKey(tag)) {
                soundEffectInstances.Add(tag, soundEffects[tag].CreateInstance());
                instanceHas3D.Add(tag, is3D);
            }
        }

        /// <summary>
        /// Create sound effect instance
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="soundEffect"></param>
        /// <param name="is3D"></param>
        /// <param name="isLooping"></param>
        /// <param name="isPlaying"></param>
        public void createSoundEffectInstance(string tag, SoundEffect soundEffect, bool is3D, bool isLooping, bool isPlaying) {
            createSoundEffectInstance(tag, soundEffect, is3D);
            setInstanceLoop(tag, isLooping);
            setInstancePlayback(tag, isPlaying);
        }

        /// <summary>
        /// Creates a sound effect instance with all of the pramaters
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="soundEffect"></param>
        /// <param name="is3D"></param>
        /// <param name="isLooping"></param>
        /// <param name="isPlaying"></param>
        /// <param name="volume"></param>
        public void createSoundEffectInstance(string tag, SoundEffect soundEffect, bool is3D, bool isLooping, bool isPlaying, float volume)
        {
            createSoundEffectInstance(tag, soundEffect, is3D, isLooping, isPlaying);
            setVolume(tag, volume);
        }


        /// <summary>
        /// Sets the volume of the instance tag
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="volume"></param>
        public void setVolume(string tag, float volume) {
            if (soundEffectInstances.ContainsKey(tag)) {
                soundEffectInstances[tag].Volume = volume;
            }
        }

        /// <summary>
        /// Plays the sound effect, single instance
        /// </summary>
        /// <param name="tag"></param>
        public void playSoundEffect(string tag, float volume) {
            if (soundEffects.ContainsKey(tag))
            {
                SoundEffectInstance sfi = soundEffects[tag].CreateInstance();
                sfi.Volume = volume;
                sfi.Play();
            }
        }

        /// <summary>
        /// Sets a sound effects looping property
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="isLooping"></param>
        public void setInstanceLoop(string tag, bool isLooping) {
            if (!soundEffectInstances.ContainsKey(tag))
                return;
            soundEffectInstances[tag].IsLooped = isLooping;
        }

        /// <summary>
        /// Changes the playing state of the sound effect
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="isPlaying"></param>
        public void setInstancePlayback(string tag, bool isPlaying) {
            if (!soundEffectInstances.ContainsKey(tag))
                return;

            if (isPlaying) {
                soundEffectInstances[tag].Play();
            }
            else
            {
                soundEffectInstances[tag].Stop();
            }
        }
    }
}
