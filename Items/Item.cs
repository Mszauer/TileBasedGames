using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GameFramework;

namespace Items {
    class Item {
        protected int Sprite;
        protected Rectangle Source;
        public int Value { get; private set; }
        public Point Position;

        public Rectangle Rect {
            get {
                return new Rectangle(Position.X, Position.Y, Source.Width,Source.Height);
            }
        }
        public Item(string spriteSheet,Rectangle sourceRect,int value,Point position) {
            Sprite = TextureManager.Instance.LoadTexture(spriteSheet);
            Source = sourceRect;
            Value = value;
            Position = position;
        }
        public void Render() {
            TextureManager.Instance.Draw(Sprite, Position, 1.0f, Source);
        }
        public void Destroy() {
            TextureManager.Instance.UnloadTexture(Sprite);
        }
    }
}
