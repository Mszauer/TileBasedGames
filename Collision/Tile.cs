using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
using System.Drawing;

namespace Collision {
    class Tile {
        public int Sprite { get; private set; }
        public Rectangle Source { get; private set; }
        public bool Walkable { get; set; }
        public PointF WorldPosition { get; set; }
        public float Scale { get; set; }

        public Tile(string spritePath, Rectangle source) {
            Sprite = TextureManager.Instance.LoadTexture(spritePath);
            Source = source;
            Walkable = false;
            Scale = 1.0f;
            WorldPosition = new Point(0, 0);
        }
        public void Render() {
            PointF renderPosition = new PointF(WorldPosition.X, WorldPosition.Y);
            renderPosition.X = (float)(Scale * renderPosition.X);
            renderPosition.Y = (float)(Scale * renderPosition.Y);
            TextureManager.Instance.Draw(Sprite, new Point((int)renderPosition.X, (int)renderPosition.Y), Scale, Source);
        }
        public void Destroy() {
            TextureManager.Instance.UnloadTexture(Sprite);
        }
    }
}
