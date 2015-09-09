using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GameFramework;

namespace HeroPractice {
    class Character {
        public Point Position { get; set; }
        public int Sprite { get; set; }

        public Dictionary<string, Rectangle> spriteSource { get; private set; }
        public string currentSprite { get; private set; }

        public Character(string spritePath,Point startPos) {
            Sprite = TextureManager.Instance.LoadTexture(spritePath);
            Position = startPos;
            
        }
        public void Destroy() {
            TextureManager.Instance.UnloadTexture(Sprite);
        }
        public void Render() {
            TextureManager.Instance.Draw(Sprite, Position, 1.0f,spriteSource[currentSprite]);
        }
        public void AddSprite(string name,Rectangle source) {
            if (spriteSource == null) {
                spriteSource = new Dictionary<string, Rectangle>();
            }
            if (currentSprite == null || currentSprite == "") {
                currentSprite = name;
            }
            name = name.ToLower();
            spriteSource.Add(name, source);
        }
        public void SetSprite(string name) {
            if (spriteSource.ContainsKey(name)) {
                name = name.ToLower();
                currentSprite = name;
            }
            else {
                Console.WriteLine("texture not found!");
            }
        }
    }
}
