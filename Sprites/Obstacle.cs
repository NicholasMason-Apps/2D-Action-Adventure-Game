using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelite_Game.Sprites
{
    public class Obstacle : Sprite, ICollidable
    {
        public Obstacle(Texture2D texture) : base(texture)
        {

        }

        public void OnCollide(Sprite sprite)
        {
            throw new NotImplementedException();
        }
    }
}
