using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
using System.Drawing;

namespace TilesSolo {
    class Tile {
        public int Sprite { get; private set; }
        public Rectangle Source { get; private set; }
        public bool Walkable { get; set; }

        public Point WorldPosition { get; set; }
        public float Scale { get; set; }

        public Tile(string spritePath, Rectangle sourceSprite) {
            Sprite = TextureManager.Instance.LoadTexture(spritePath);
            Source = sourceSprite;
            WorldPosition = new Point(0, 0);
            Scale = 1.0f;
            Walkable = false;
        }
        public void Render() {
            Point renderPosition = new Point(WorldPosition.X, WorldPosition.Y);
            renderPosition.X = (int)(Scale * renderPosition.X);
            renderPosition.Y = (int)(Scale * renderPosition.Y);
            TextureManager.Instance.Draw(Sprite, renderPosition, Scale,Source);
        }
        public void Destroy() {
            TextureManager.Instance.UnloadTexture(Sprite);
        }
    }
}
