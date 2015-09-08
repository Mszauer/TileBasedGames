using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GameFramework;

namespace Hero {
    class Character {
        public Point Position { get; private set; }
        public int Sprite { get; private set; }

        public Rectangle facingUpFrame1 { get; set; }
        public Rectangle facingDownFrame1 { get; set; }
        public Rectangle facingLeftFrame1 { get; set; }
        public Rectangle facingRightFrame1 { get; set; }

        public Rectangle displaySourceRect { get; set; }

        public Character(string spriteSheet, Point startPos) {
            Position = startPos;
            Sprite = TextureManager.Instance.LoadTexture(spriteSheet);
            displaySourceRect = facingDownFrame1;
        }
        public void Destroy() {
            TextureManager.Instance.UnloadTexture(Sprite);
        }
        public void Render() {
            TextureManager.Instance.Draw(Sprite, Position, 1.0f, displaySourceRect);
        }
    }
}
