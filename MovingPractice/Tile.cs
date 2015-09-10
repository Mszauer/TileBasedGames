using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GameFramework;

namespace MovingPractice {
    class Tile {
        public int Sprite { get; private set; }
        public Rectangle Source { get; private set; }
        public bool Walkable { get; set; }
        public Point WorldPosition { get; set; }
        public float Scale { get; set; }
        public Tile(string filePath, Rectangle sourceRect) {
            Sprite = TextureManager.Instance.LoadTexture(filePath);
            Source = sourceRect;
            Scale = 1.0f;
            WorldPosition = new Point(0, 0);
            Walkable = false;
        }
        public void Render() {
            Point drawPos = new Point(WorldPosition.X, WorldPosition.Y);
            drawPos.X = (int)(Scale * drawPos.X);
            drawPos.Y = (int)(Scale * drawPos.Y);
            TextureManager.Instance.Draw(Sprite, drawPos, Scale, Source);
        }
        public void Destroy() {
            TextureManager.Instance.UnloadTexture(Sprite);
        }
    }
}
