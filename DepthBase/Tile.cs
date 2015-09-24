using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GameFramework;

namespace DepthBase {
    class Tile {
        public Map DoorTarget = null;
        public Point DoorLocation = new Point();
        public bool IsDoor { get; set; }
        public int Sprite { get; private set; }
        public Rectangle Source { get; private set; }
        public bool Walkable { get; set; }
        public Point WorldPosition { get; set; }
        public float Scale { get; set; }
        public Tile(string spritePath, Rectangle source) {
            Sprite = TextureManager.Instance.LoadTexture(spritePath);
            Source = source;
            Scale = 1.0f;
            Walkable = false;
            WorldPosition = new Point(0, 0);
        }
        public void Render(PointF offsetPosition) {
            //find world position
            Point renderPos = new Point(WorldPosition.X, WorldPosition.Y);
            //apply scaling
            renderPos.X = (int)(Scale * renderPos.X);
            renderPos.Y = (int)(Scale * renderPos.Y);
            //move to camera space
            renderPos.X -= (int)offsetPosition.X;
            renderPos.Y -= (int)offsetPosition.Y;
            //
            Rectangle renderRect = new Rectangle(Source.Location, Source.Size);
            if (renderRect.Height != 30) {
                int difference = renderRect.Height - 30;
                renderPos.Y -= difference;
            }
            //Draw tile
            TextureManager.Instance.Draw(Sprite, renderPos, Scale, renderRect);
        }
        public void Destroy() {
            TextureManager.Instance.UnloadTexture(Sprite);
        }
        public void MakeDoor(Map target, Point location) {
            DoorTarget = target;
            DoorLocation = location;
            IsDoor = true;
        }
    }
}
