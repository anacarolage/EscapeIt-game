using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;

namespace Puzzle
{
    class Puzzle
    {
        private Texture2D fullImage;

        private Texture2D empty;

        private Texture2D[] textures = new Texture2D[15];

        private List<Tile> tiles = new List<Tile>();

        private Texture2D[] texturesSuffled = new Texture2D[15];

        private Random random = new Random();

        public Vector2 FullImagePosition;

        private SoundEffect tileSound;

        public Vector2 ScreenDimension;






       




    }
}
