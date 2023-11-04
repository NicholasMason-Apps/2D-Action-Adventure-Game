using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Roguelite_Game.Sprites.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Roguelite_Game.Sprites
{
    class ItemRoom : Room
    {
        private float _itemTier;
        private Item _item;
        private Random rnd = new Random();
        private ContentManager _content;

        private string[] STierItems;
        private string[] ATierItems;
        private string[] BTierItems;
        private string[] CTierItems;
        public ItemRoom(ContentManager content, Vector2 offsetMultiplier) : base(content, offsetMultiplier)
        {
            //_item = content.Load<Texture2D>("MapContent/item_temp"); 
            //STierItems = Directory.GetFiles("Content/ItemContent/STierItems").Select(Path.GetFileName).ToArray();
            //ATierItems = Directory.GetFiles("Content/ItemContent/ATierItems").Select(Path.GetFileName).ToArray();
            //BTierItems = Directory.GetFiles("Sprites/Items/BTierItems/BTierItems").Select(Path.GetFileName).ToArray();
            CTierItems = Directory.GetFiles("Content/ItemContent/CTierItems").Select(Path.GetFileName).ToArray();
            _content = content;

            GenerateItem();
            Sprites.Add(_item);
        }

        public void GenerateItem()
        {
            /*
            _itemTier = (float)rnd.NextDouble();
            if (_itemTier > 0.95f)
            {
                //Add code for S tier Item
            } 
            else if (_itemTier > 0.8f)
            {
                //Add code for A tier Item
            } 
            else if (_itemTier > 0.55f)
            {
                //Add code for B tier Item
            } 
            else
            {
                var itemIndex = rnd.Next(0, CTierItems.Length);
                var pickedItem = CTierItems[itemIndex].Replace(".cs", "");
                var itemTexture = _content.Load<Texture2D>("ItemContent/"+pickedItem);
                _item = GetInstance("Roguelite_Game.Sprites.Items."+pickedItem, itemTexture);
            }
            */
            var itemIndex = rnd.Next(0, CTierItems.Length);
            var pickedItem = CTierItems[itemIndex].Replace(".xnb", "");
            var itemTexture = _content.Load<Texture2D>("ItemContent/CTierItems/" + pickedItem);
            var itemType = GetType("Roguelite_Game.Sprites.Items." + pickedItem);
            _item = (Item)Activator.CreateInstance(itemType, itemTexture);
            //_item = (Item)System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(pickedItem);
            _item.Position = new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2) + Offset;
            _item.Layer = 0.2f;
        }
        public static Type GetType(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null) return type;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }
        /*
        public object GetInstance(string qualifiedName, Texture2D texture)
        {
            Type type = Type.GetType(qualifiedName);
            if (type != null)
                return Activator.CreateInstance(type, texture);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(qualifiedName);
                if (type != null)
                    return Activator.CreateInstance(type, texture);
            }
            return null;
        }
        */
    }
}
