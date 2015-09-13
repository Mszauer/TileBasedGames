using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;
using GameFramework;

namespace Collision {
    class Character {
        public PointF Position { get; set; }
        public int Sprite { get; set; }
        public Dictionary<string, Rectangle[]> SpriteSource { get; private set; }
        public string currentSprite { get; private set; }
        public int currentFrame = 0;

        public Character(string spritePath,Point pos) {
            Sprite = TextureManager.Instance.LoadTexture(spritePath);
            Position = pos;
        }
        public void Destroy() {
            TextureManager.Instance.UnloadTexture(Sprite);
        }
        public void Render() {
            TextureManager.Instance.Draw(Sprite,new Point((int)Position.X,(int)Position.Y), 1.0f, SpriteSource[currentSprite][currentFrame]);
        }
        public void AddSprite(string name, params Rectangle[] source) {
            name = name.ToLower();
            if(SpriteSource == null) {
                SpriteSource = new Dictionary<string, Rectangle[]>();
            }
            if (currentSprite == null) {
                currentSprite = name;
            }
            SpriteSource.Add(name, source);
        }
        public void SetSprite(string name) {
            name = name.ToLower();
            if (SpriteSource.ContainsKey(name)) {
                currentSprite = name;
            }
            else {
                Console.WriteLine("Texture not found: " + name);
            }
        }
    }
}
