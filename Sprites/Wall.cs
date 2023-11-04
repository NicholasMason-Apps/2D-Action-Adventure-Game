using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelite_Game.Sprites
{
    public class Wall : Sprite, ICollidable
    {
        public Wall(Texture2D texture) : base(texture)
        {
        }
        public void OnCollide(Sprite sprite)
        {
            return;
        }
    }
}
