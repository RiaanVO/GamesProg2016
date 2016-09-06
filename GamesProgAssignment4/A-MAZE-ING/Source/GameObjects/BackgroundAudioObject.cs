using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace A_MAZE_ING
{
    class BackgroundAudioObject : GameObject
    {
        AudioEmitterComponent audioEmitter;
        public BackgroundAudioObject(Game game, ObjectManager objectManager, Vector3 startPosition) : base(game, objectManager, startPosition)
        {
            audioEmitter = new AudioEmitterComponent(game, this);
            audioEmitter.createSoundEffectInstance("background1", game.Content.Load<SoundEffect>(@"Sounds/scarybgm"), false, true, true, 0.3f);
        }
    }
}
