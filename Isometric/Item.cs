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
            int yTile = (Position.Y) / 30;
            GraphicsManager.Instance.SetDepth(yTile * 20 + xTile + 0.2f);
            PointF renderPosition = new PointF(Position.X,Position.Y);
            renderPosition.X -= (int)offsetPosition.X;
            renderPosition.Y -= (int)offsetPosition.Y;
            renderPosition = Map.CartToIso(renderPosition);
            renderPosition.X += 50; //allign with registration point
            if (Game.ViewWorldSpace) {
                Rectangle r = new Rectangle(Position, new Size(Source.Width / 2, Source.Width / 2));
                GraphicsManager.Instance.DrawRect(r, Color.DarkSeaGreen);
            }
            else {
                TextureManager.Instance.Draw(Sprite, new Point((int)renderPosition.X,(int)renderPosition.Y), 1.0f, Source);
            }
        }
        public void Destroy() {
            TextureManager.Instance.UnloadTexture(Sprite);
        }
    }
}
