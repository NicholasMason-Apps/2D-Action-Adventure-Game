using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roguelite_Game.Models
{
    public class Animation : ICloneable
    {
        //Definition of properties
        public int CurrentFrame { get; set; }
        public int FrameCount { get; private set; }
        public int FrameHeight { get { return Texture.Height; } } //used to grab the height of the sprite sheet
        public float FrameSpeed { get; set; } //how fast you go through the animation frames (the required time needed to elapse to then increment a frame)
        public int FrameWidth { get { return Texture.Width / FrameCount; } } //takes the width of the entire sprite sheet and gets the width of an individual sprite by dividing the width by the total number of frames in the sprite sheet
        public bool LoopAnimation { get; set; } //Used to determine if a sprite sheet wants to be looped through or not
        public Texture2D Texture { get; private set; }

        //Animation Constructor
        public Animation(Texture2D texture, int frameCount)
        {
            Texture = texture;
            FrameCount = frameCount;
            LoopAnimation = true;
            FrameSpeed = 0.1f;
            ///The above code implements some of the properties as variables in the Animation() constructor
            ///texture and frameCount are parameters passed to the constructor, and then assigned to the properties of the class Texture and FrameCount respectively

        }

        public object Clone() //Allows other objects to use this Animation Class without them conflicting with one-another as they can clone it
        {
            return this.MemberwiseClone();
        }
    }
}
