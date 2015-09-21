using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Items {
    class EnemyCharacter : Character {
        float speed = 60.0f;
        bool moveUpDown = false;
        float directions = 1.0f;
        public EnemyCharacter(string spritePath, Point pos, bool movingUpDown) : base(spritePath, pos) {
            AddSprite("Down", new Rectangle(68, 112, 29, 30), new Rectangle(101, 112, 29, 30));
            AddSprite("Up", new Rectangle(134, 112, 30, 29), new Rectangle(167, 112, 30, 29));
            AddSprite("Left", new Rectangle(1, 113, 30, 30), new Rectangle(34, 112, 30, 30));
            AddSprite("Right", new Rectangle(201, 112, 30, 29), new Rectangle(234, 113, 30, 29));
            SetSprite("Down");

            moveUpDown = movingUpDown;
            if (!moveUpDown) {
                SetSprite("Right");
            }
        }
        public void Update(float dTime) {
            Animate(dTime);
            if (moveUpDown) {
                if (directions > 0) {
                    SetSprite("down");
                }
                else {
                    SetSprite("up");
                }
                //movement
                Position.Y += directions * speed * dTime;
                //wall collision
                //upper left
                if (!Game.Instance.GetTile(Corners[CORNER_TOP_LEFT]).Walkable) {
                    Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_TOP_LEFT]));
                    if (intersection.Height * intersection.Width > 0) {
                        directions *= -1;
                        Position.Y = intersection.Bottom;
                    }
                }
                //upper right
                if (!Game.Instance.GetTile(Corners[CORNER_TOP_RIGHT]).Walkable) {
                    Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_TOP_RIGHT]));
                    if (intersection.Width * intersection.Height > 0) {
                        directions *= -1;
                        Position.Y = intersection.Bottom;
                    }
                }
                //lower left
                if (!Game.Instance.GetTile(Corners[CORNER_BOTTOM_LEFT]).Walkable) {
                    Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_BOTTOM_LEFT]));
                    if (intersection.Width * intersection.Height > 0) {
                        directions *= -1;
                        Position.Y = intersection.Top - Rect.Height;
                    }
                }
                //lower right
                if (!Game.Instance.GetTile(Corners[CORNER_BOTTOM_RIGHT]).Walkable) {
                    Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_BOTTOM_RIGHT]));
                    if (intersection.Width * intersection.Height > 0) {
                        directions *= -1;
                        Position.Y = intersection.Top - Rect.Height;
                    }
                }
            }
            else {
                if (directions > 0) {
                    SetSprite("Right");
                }
                else {
                    SetSprite("Left");
                }
                //movement
                Position.X += directions * speed * dTime;
                //wall collision
                //upper left
                if (!Game.Instance.GetTile(Corners[CORNER_TOP_LEFT]).Walkable) {
                    Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_TOP_LEFT]));
                    if (intersection.Height * intersection.Width > 0) {
                        directions *= -1;
                        Position.X = intersection.Right;
                    }
                }
                //upper right
                if (!Game.Instance.GetTile(Corners[CORNER_TOP_RIGHT]).Walkable) {
                    Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_TOP_RIGHT]));
                    if (intersection.Width * intersection.Height > 0) {
                        directions *= -1;
                        Position.X = intersection.Left - Rect.Width;
                    }
                }
                //lower left
                if (!Game.Instance.GetTile(Corners[CORNER_BOTTOM_LEFT]).Walkable) {
                    Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_BOTTOM_LEFT]));
                    if (intersection.Width * intersection.Height > 0) {
                        directions *= -1;
                        Position.X = intersection.Right;
                    }
                }
                //lower right
                if (!Game.Instance.GetTile(Corners[CORNER_BOTTOM_RIGHT]).Walkable) {
                    Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_BOTTOM_RIGHT]));
                    if (intersection.Width * intersection.Height > 0) {
                        directions *= -1;
                        Position.X = intersection.Left - Rect.Width;
                    }
                }

            }
        }

    }
}
