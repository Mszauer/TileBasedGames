using GameFramework;
using System.Drawing;

namespace Shooting {
    class Bullet {
        public PointF Position = new PointF(0f, 0f);
        public PointF Velocity = new PointF(0f, 0f);
        public Rectangle Rect {
            get {
                return new Rectangle((int)Position.X - 5, (int)Position.Y - 5, 10, 10);
            }
        }
        public Bullet(PointF pos, PointF vel) {
            Position = pos;
            Velocity = vel;
        }
        public void Update(float dTime) {
            Position.X += Velocity.X * dTime;
            Position.Y += Velocity.Y * dTime;
        }
        public void Render() {
            Rectangle renderRect = new Rectangle(0,0,10,10);
            renderRect.X = (int)(Position.X - 5.0f);
            renderRect.Y = (int)(Position.Y - 5.0f);
            GraphicsManager.Instance.DrawRect(renderRect, Color.Red);
        }
    }
}
