using GameFramework;
using System.Drawing;

namespace Isometric {
    class Bullet {
        private int spriteSheetHandle;
        private Rectangle sourceRect = new Rectangle(43,161,22,20); //from spritesheet
        public PointF Position = new PointF(0f, 0f);
        public PointF Velocity = new PointF(0f, 0f);
        public Rectangle Rect {
            get {
                return new Rectangle(new Point((int)Position.X,(int)Position.Y), new Size(sourceRect.Width / 2, sourceRect.Width / 2));
            }
        }
        public Bullet(PointF pos, PointF vel, int sheetReferance) {
            Position = pos;
            Velocity = vel;
            spriteSheetHandle = sheetReferance;
        }
        public void Update(float dTime) {
            Position.X += Velocity.X * dTime;
            Position.Y += Velocity.Y * dTime;
        }
        public void Render(PointF offsetPosition) {
            PointF renderPoint = new PointF(Position.X,Position.Y);
            renderPoint.X -= (int)offsetPosition.X;
            renderPoint.Y -= (int)offsetPosition.Y;
            renderPoint = Map.CartToIso(renderPoint);
            renderPoint.X += 57;//allign with registration point
            GraphicsManager.Instance.SetDepth(19 * 19);
            if (Game.ViewWorldSpace) {
                GraphicsManager.Instance.DrawRect(Rect, Color.Red);
            }
            else {
                TextureManager.Instance.Draw(spriteSheetHandle, new Point((int)renderPoint.X,(int)renderPoint.Y), 1.0f, sourceRect);
            }
        }
    }
}
