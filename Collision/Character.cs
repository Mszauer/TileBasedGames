using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;
using GameFramework;

namespace Collision {
    class Character {
        public PointF Position = new PointF(0.0f, 0.0f);
        public int Sprite { get; set; }
        public Dictionary<string, Rectangle[]> SpriteSource { get; private set; }
        public string currentSprite { get; private set; }
        public int currentFrame = 0;
        public PointF Center {
            get {
                return new PointF(Rect.X+(Rect.Width/2), Rect.Y+(Rect.Height/2));
            }
        }
        public Rectangle Rect {
            get {
                return new Rectangle(new Point((int)Position.X,(int)Position.Y), new Size(SpriteSource[currentSprite][currentFrame].Width, SpriteSource[currentSprite][currentFrame].Height));
            }
        }
        public PointF[] Corners {
            get {
                float w = SpriteSource[currentSprite][currentFrame].Width;
                float h = SpriteSource[currentSprite][currentFrame].Height;
                return new PointF[] {
                    new PointF(Position.X,Position.Y),
                    new PointF(Position.X+w,Position.Y),
                    new PointF(Position.X,Position.Y+h),
                    new PointF(Position.X+w,Position.Y+h)
                };
            }
        }
        public static readonly int CORNER_TOP_LEFT = 0;
        public static readonly int CORNER_TOP_RIGHT = 1;
        public static readonly int CORNER_BOTTOM_LEFT = 2;
        public static readonly int CORNER_BOTTOM_RIGHT = 3;
        public Character(string spritePath,Point pos) {
            Sprite = TextureManager.Instance.LoadTexture(spritePath);
            Position = pos;
        }
        public void Destroy() {
            TextureManager.Instance.UnloadTexture(Sprite);
        }
        public void Render() {
            GraphicsManager.Instance.DrawRect(Rect, Color.Red);
            TextureManager.Instance.Draw(Sprite,new Point((int)Position.X,(int)Position.Y), 1.0f, SpriteSource[currentSprite][currentFrame]);
            Rectangle center = new Rectangle((int)Center.X - 5, (int)Center.Y - 5, 10, 10);
            GraphicsManager.Instance.DrawRect(center, Color.Yellow);
            foreach(PointF corner in Corners) {
                Rectangle rect = new Rectangle((int)corner.X - 5, (int)corner.Y - 5, 10, 10);
                GraphicsManager.Instance.DrawRect(rect, Color.Green);
            }
        }
        public void AddSprite(string name, params Rectangle[] source) {
            name = name.ToLower();
            if(SpriteSource == null) {
                SpriteSource = new Dictionary<string, Rectangle[]>();
            }
            if (currentSprite == null) {
                currentSprite = name;
            }
            SpriteSource.Add(name, source);
        }
        public void SetSprite(string name) {
            name = name.ToLower();
            if (SpriteSource.ContainsKey(name)) {
                currentSprite = name;
            }
            else {
                Console.WriteLine("Texture not found: " + name);
            }
        }
    }
}
