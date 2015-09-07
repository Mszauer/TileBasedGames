using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GameFramework;

namespace Skeleton {
    class Tile {
        public int Sprite { get; private set; }
        public Rectangle Source { get; private set; }
        public bool Walkable { get; set; }

        public Point WorldPosition { get; set; }
        public float Scale { get; set; }

        public Tile(string spritePath, Rectangle sourceRect) {
            Sprite = TextureManager.Instance.LoadTexture(spritePath);
            Source = sourceRect;
            Walkable = false;
            WorldPosition = new Point(0, 0);
            Scale = 1.0f;
        }
        public void Render() {
            Point renderPosition = new Point(WorldPosition.X, WorldPosition.Y);
            renderPosition.X = (int)(renderPosition.X * Scale);
            renderPosition.Y = (int)(renderPosition.Y * Scale);
            TextureManager.Instance.Draw(Sprite, renderPosition, Scale, Source);
        }
        public void Destroy() {
            TextureManager.Instance.UnloadTexture(Sprite);
        }
    }
}
