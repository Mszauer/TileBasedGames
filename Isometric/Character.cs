using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
using System.Drawing;

namespace Isometric {
    class Character {
        public int Sprite { get; set; }
        public PointF Position = new PointF(0f, 0f);
        public Dictionary<string, Rectangle[]> SpriteSources { get; private set; }
        public string currentSprite { get; set; }
        public int currentFrame = 0;
        float animFPS = 1.0f / 9.0f;
        float animTimer = 0f;
        public Rectangle Rect {
            get {
                int width = SpriteSources[currentSprite][currentFrame].Width/2;
                int height = width;
                return new Rectangle((int)Position.X, (int)Position.Y, width, height);
            }
        }
        public PointF Center {
            get {
                return new PointF(Rect.X + (Rect.Width / 2), Rect.Y + (Rect.Height / 2));
            }
        }
        public PointF[] Corners {
            get {
                float w = Rect.Width;
                float h = Rect.Height;
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
        public Character(string spritePath, Point pos, float height) {
            Sprite = TextureManager.Instance.LoadTexture(spritePath);
            Position = pos;
        }
        public void Render(PointF offsetPosition) {
            int tileX = (int)(Corners[CORNER_BOTTOM_RIGHT].X -1) / Game.TILE_W;
            int tileY = (int)(Corners[CORNER_BOTTOM_RIGHT].Y - 1) / Game.TILE_H;
            GraphicsManager.Instance.SetDepth(tileY * 20 + tileX + 0.5f);

            PointF renderPosition = new PointF(Position.X, Position.Y);
            renderPosition.X -= offsetPosition.X;
            renderPosition.Y -= offsetPosition.Y;

            //Apply iso transformation
            renderPosition = Map.CartToIso(renderPosition);
            //Allign registration points
            renderPosition.X += 25;
            if (SpriteSources[currentSprite][currentFrame].Height > Rect.Height) {
                //apply  depth offset
                renderPosition.Y -= Rect.Height;
            }
            if (Game.ViewWorldSpace) {
                GraphicsManager.Instance.DrawRect(Rect, Color.SteelBlue);
            }
            else {
                TextureManager.Instance.Draw(Sprite, new Point((int)renderPosition.X, (int)renderPosition.Y), 1.0f, SpriteSources[currentSprite][currentFrame]);
            }
        }
        public void Destroy() {
            TextureManager.Instance.UnloadTexture(Sprite);
        }
        public void SetSprite(string name) {
            name = name.ToLower();
            if (SpriteSources.ContainsKey(name)) {
                currentSprite = name;
            }
            else {
                Console.WriteLine("Texture not found: " + name);
            }
        }
        public void AddSprite(string name, params Rectangle[] source) {
            name = name.ToLower();
            if (SpriteSources == null) {
                SpriteSources = new Dictionary<string, Rectangle[]>();
            }
            if (currentSprite == null) {
                currentSprite = name;
            }
            SpriteSources.Add(name, source);

        }
        protected void Animate(float dTime) {
            animTimer += dTime;
            if (animTimer > animFPS) {
                animTimer -= animFPS;
                currentFrame += 1;
                if (currentFrame > SpriteSources[currentSprite].Length - 1) {
                    currentFrame = 0;
                }
            }
        }
    }
}
