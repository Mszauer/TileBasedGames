﻿using System;
using System.Collections.Generic;
using GameFramework;
using System.Drawing;
using OpenTK;

namespace MouseToMove {
    class Game {
        protected Point spawnTile = new Point(2, 1);
        protected PlayerCharacter hero = null;
        protected string heroSheet = "Assets/Link.png";
        protected Point cursorTile = new Point(0, 0);
        public OpenTK.GameWindow Window = null;
        protected Map room1 = null;
        public static readonly int TILE_SIZE = 30;
        protected int[][] room1Layout = new int[][] {
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 1, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 1, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 2 },
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1 }
        };
        protected Map room2 = null;
        protected int[][] room2Layout = new int[][] {
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1 },
            new int[] { 2, 0, 0, 0, 1, 0, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 0, 0, 1, 0, 0, 1 },
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1 }
        };
        protected Map currentMap = null;
        protected string spriteSheets = "Assets/HouseTiles.png";
        protected Rectangle[] spriteSources = new Rectangle[] {
            new Rectangle(466,32,TILE_SIZE,TILE_SIZE),
            new Rectangle(466,1,TILE_SIZE,TILE_SIZE),
            new Rectangle(32,187,TILE_SIZE,TILE_SIZE)
        };
        public Tile GetTile(PointF pixelPoint) {
            return currentMap[(int)pixelPoint.Y / TILE_SIZE][(int)pixelPoint.X / TILE_SIZE];
        }
        public Rectangle GetTileRect(PointF pixelPoint) {
            int xTile = (int)pixelPoint.X / TILE_SIZE;//integer math
            int yTile = (int)pixelPoint.Y / TILE_SIZE;
            Rectangle result = new Rectangle(xTile * TILE_SIZE, yTile * TILE_SIZE, TILE_SIZE, TILE_SIZE);
            return result;
        }
        //Singleton
        private static Game instance = null;
        public static Game Instance {
            get {
                if (instance == null) {
                    instance = new Game();
                }
                return instance;
            }
        }

        protected Game() {

        }
        public void Initialize(OpenTK.GameWindow window) {
            Window = window;
            window.ClientSize = new Size(room1Layout[0].Length * TILE_SIZE, room1Layout.Length * TILE_SIZE);
            TextureManager.Instance.UseNearestFiltering = true;

            hero = new PlayerCharacter(heroSheet, new Point(spawnTile.X * TILE_SIZE, spawnTile.Y * TILE_SIZE));
            room1 = new Map(room1Layout, spriteSheets, spriteSources, 2, 0);
            room2 = new Map(room2Layout, spriteSheets, spriteSources, 0, 2);
            room1[4][7].MakeDoor(room2, new Point(1, 1));
            room2[1][0].MakeDoor(room1, new Point(6, 4));
            currentMap = room1;
        }
        public void Update(float dt) {
            currentMap = currentMap.ResolveDoors(hero);
            hero.Update(dt);
            cursorTile = new Point(InputManager.Instance.MousePosition.X/TILE_SIZE, InputManager.Instance.MousePosition.Y / TILE_SIZE);
            if (InputManager.Instance.MousePressed(OpenTK.Input.MouseButton.Left) || InputManager.Instance.KeyPressed(OpenTK.Input.Key.M)) {
                if (currentMap[cursorTile.Y][cursorTile.X].Walkable) {
                    if (currentMap[cursorTile.Y][cursorTile.X].IsDoor) {
                      
                    }
                    hero.SetTargetTile(cursorTile);
                }
            }
        }
        public void Render() {
            currentMap.Render();
            hero.Render();

            //Draw mouse indicator
            Rectangle currentTile = GetTileRect(InputManager.Instance.MousePosition);
                //Top
            GraphicsManager.Instance.DrawLine(new Point(currentTile.X, currentTile.Y), new Point(currentTile.X + currentTile.Width, currentTile.Y), Color.Red);
                //Bottom
            GraphicsManager.Instance.DrawLine(new Point(currentTile.X, currentTile.Y + currentTile.Height), new Point(currentTile.X + currentTile.Width, currentTile.Y + currentTile.Height), Color.Red);
                //Left
            GraphicsManager.Instance.DrawLine(new Point(currentTile.X, currentTile.Y), new Point(currentTile.X, currentTile.Y + currentTile.Height), Color.Red);
                //Right
            GraphicsManager.Instance.DrawLine(new Point(currentTile.X + currentTile.Width, currentTile.Y), new Point(currentTile.X+currentTile.Width, currentTile.Y + currentTile.Height), Color.Red);
        }
        public void Shutdown() {
            room1.Destroy();
            room2.Destroy();
            hero.Destroy();
        }
    }
}
