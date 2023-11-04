using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelite_Game.Sprites
{
    public class Door : Sprite, ICollidable
    {
        public Room ConnectedRoom { get; set; }

        public Door(Texture2D texture) : base(texture)
        {

        }

        public void OnCollide(Sprite sprite)
        {
            return;
        }
    }
}
