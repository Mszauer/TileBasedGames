using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;
using GameFramework;

namespace KeysToMove {
    class Character {
        public PointF Position { get; set; }
        public int Sprite { get; set; }
        public Dictionary<string, Rectangle> SpriteSource { get; private set; }
        public string currentSprite { get; private set; }

        public Character(string spritePath,Point startPos) {
            Sprite = TextureManager.Instance.LoadTexture(spritePath);
            Position = startPos;
        }
        public void Destroy() {
            TextureManager.Instance.UnloadTexture(Sprite);
        }
        public void Render() {
            TextureManager.Instance.Draw(Sprite, new Point((int)Position.X,(int)Position.Y), 1.0f, SpriteSource[currentSprite]);
        }
        public void AddSprite(string name, Rectangle source) {
            name = name.ToLower();
            if (SpriteSource == null) {
                SpriteSource = new Dictionary<string, Rectangle>();
            }
            if (currentSprite == null || currentSprite == "") {
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
                Console.WriteLine("Texture not found! " + name);
            }
        }
    }
}
