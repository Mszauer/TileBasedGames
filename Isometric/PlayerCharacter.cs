﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
using System.Drawing;

namespace Isometric {
    class PlayerCharacter : Character {
        public float speed = 90.0f;
        public PlayerCharacter(string spritePath, Point pos, float height) : base(spritePath, pos, height) {
            AddSprite("Down", new Rectangle(52, 19, 85, 84));
            AddSprite("Up", new Rectangle(266, 19, 85, 84));
            AddSprite("Left", new Rectangle(155, 19, 85, 84));
            AddSprite("Right", new Rectangle(375, 19, 85, 84));
            SetSprite("Down");
        }
        public void Update(float deltaTime) {
            InputManager i = InputManager.Instance;

            if (i.KeyDown(OpenTK.Input.Key.A) || i.KeyDown(OpenTK.Input.Key.Left)) {
                SetSprite("Left");
                Animate(deltaTime);
                Position.X -= speed * deltaTime;
                if (!Game.Instance.GetTile(Corners[CORNER_TOP_LEFT]).Walkable) {
                    Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_TOP_LEFT]));
                    if (intersection.Width * intersection.Height > 0) {
                        Position.X = intersection.Right;
                    }
                }
                if (!Game.Instance.GetTile(Corners[CORNER_BOTTOM_LEFT]).Walkable) {
                    Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_BOTTOM_LEFT]));
                    if (intersection.Width * intersection.Height > 0) {
                        Position.X = intersection.Right;
                    }
                }
            }
            else if (i.KeyDown(OpenTK.Input.Key.D) || i.KeyDown(OpenTK.Input.Key.Right)) {
                SetSprite("Right");
                Animate(deltaTime);
                Position.X += speed * deltaTime;
                if (!Game.Instance.GetTile(Corners[CORNER_TOP_RIGHT]).Walkable) {
                    Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_TOP_RIGHT]));
                    if (intersection.Width * intersection.Height > 0) {
                        Position.X = intersection.Left - Rect.Width;
                    }
                }
                if (!Game.Instance.GetTile(Corners[CORNER_BOTTOM_RIGHT]).Walkable) {
                    Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_BOTTOM_RIGHT]));
                    if (intersection.Width * intersection.Height > 0) {
                        Position.X = intersection.Left - Rect.Width;
                    }
                }
            }
            if (i.KeyDown(OpenTK.Input.Key.W) || i.KeyDown(OpenTK.Input.Key.Up)) {
                SetSprite("Up");
                Animate(deltaTime);
                Position.Y -= speed * deltaTime;
                if (!Game.Instance.GetTile(Corners[CORNER_TOP_LEFT]).Walkable) {
                    Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_TOP_LEFT]));
                    if (intersection.Width * intersection.Height > 0) {
                        Position.Y = intersection.Bottom;
                    }
                }
                if (!Game.Instance.GetTile(Corners[CORNER_TOP_RIGHT]).Walkable) {
                    Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_TOP_RIGHT]));
                    if (intersection.Width * intersection.Height > 0) {
                        Position.Y = intersection.Bottom;
                    }
                }
            }
            else if (i.KeyDown(OpenTK.Input.Key.S) || i.KeyDown(OpenTK.Input.Key.Down)) {
                SetSprite("Down");
                Animate(deltaTime);
                Position.Y += speed * deltaTime;
                if (!Game.Instance.GetTile(Corners[CORNER_BOTTOM_LEFT]).Walkable) {
                    Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_BOTTOM_LEFT]));
                    if (intersection.Width * intersection.Height > 0) {
                        Position.Y = intersection.Top - Rect.Height;
                    }
                }
                if (!Game.Instance.GetTile(Corners[CORNER_BOTTOM_RIGHT]).Walkable) {
                    Rectangle intersection = Intersections.Rect(Rect, Game.Instance.GetTileRect(Corners[CORNER_BOTTOM_RIGHT]));
                    if (intersection.Width * intersection.Height > 0) {
                        Position.Y = intersection.Top - Rect.Height;
                    }
                }
            }

        }

    }
}
