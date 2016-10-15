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
    }
}
