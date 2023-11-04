using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelite_Game.Sprites.Items
{
    public class EnergyDrink : Item
    {
        public EnergyDrink(Texture2D texture) : base(texture)
        {
            Buff = new Buff("Movement", 0.20f);
            Description = "Increase movement speed by 20%";
            SplashText = "Fast af";

        }
    }
}
