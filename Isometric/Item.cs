using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GameFramework;

namespace Isometric {
    class Item {
        protected int Sprite;
        protected Rectangle Source;
        public int Value { get; private set; }
        public Point Position;

        public Rectangle Rect {
            get {
                return new Rectangle(Position.X, Position.Y, Source.Width, Source.Height);
            }
        }
        public Item(string spriteSheet, Rectangle sourceRect, int value, Point position) {
            Sprite = TextureManager.Instance.LoadTexture(spriteSheet);
            Source = sourceRect;
            Value = value;
            Position = position;
        }
        public void Render(PointF offsetPosition) {
            int xTile = Position.X / 30;
            int yTile = (Position.Y - 1) / 30;
            GraphicsManager.Instance.SetDepth(yTile * 20 + xTile + 0.2f);
            Point renderPosition = new Point((int)Position.X, (int)Position.Y);
            renderPosition.X -= (int)offsetPosition.X;
            renderPosition.Y -= (int)offsetPosition.Y;
            TextureManager.Instance.Draw(Sprite, renderPosition, 1.0f, Source);
        }
        public void Destroy() {
            TextureManager.Instance.UnloadTexture(Sprite);
        }
    }
}
