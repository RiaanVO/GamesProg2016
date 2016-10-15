using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace PRedesign
{
    /// <summary>
    /// Helper class which holds references to all loaded content.
    /// This allows the loading to occur once and other classes to make use of that load
    /// without passing lots of variables between classes etc.
    /// Also makes the content system more extensible with configuration files.
    /// </summary>
    static class ContentStore
    {
        
        public static Dictionary<string, Model> loadedModels = new Dictionary<string, Model>();
        public static Dictionary<string, Texture2D> loadedTextures = new Dictionary<string, Texture2D>();
        public static Dictionary<string, SoundEffect> loadedSounds = new Dictionary<string, SoundEffect>();

        public static void Add(string name, Model model) {
            if (loadedModels.ContainsKey(name))
                return;
            loadedModels.Add(name, model);
        }

        public static void Add(string name, Texture2D texture)
        {
            if (loadedTextures.ContainsKey(name))
                return;
            loadedTextures.Add(name, texture);
        }

        public static void Add(string name, SoundEffect soundEffect)
        {
            if (loadedSounds.ContainsKey(name))
                return;
            loadedSounds.Add(name, soundEffect);
        }

        public static void Unload() {
            loadedModels.Clear();
            loadedTextures.Clear();
            loadedSounds.Clear();
        }
    }
}
