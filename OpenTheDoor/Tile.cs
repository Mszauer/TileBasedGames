using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GameFramework;

namespace OpenTheDoor {
    class Tile {
        public int Sprite { get; private set; }
        public Rectangle Source { get; private set; }
        public bool Walkable { get; set; }
        public Point WorldPosition { get; set; }
        public float Scale { get; set; }
        public Tile(string spritePath,Rectangle source) {
            Sprite = TextureManager.Instance.LoadTexture(spritePath);
            Source = source;
            Scale = 1.0f;
            Walkable = false;
            WorldPosition = new Point(0, 0);
        }
        public void Render() {
            Point renderPos = new Point(WorldPosition.X, WorldPosition.Y);
            renderPos.X = (int)(Scale * renderPos.X);
            renderPos.Y = (int)(Scale * renderPos.Y);
            TextureManager.Instance.Draw(Sprite, renderPos, Scale, Source);
        }
        public void Destroy() {
            TextureManager.Instance.UnloadTexture(Sprite);
        }
    }
}
