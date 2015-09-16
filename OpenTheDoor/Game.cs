using System;
using System.Collections.Generic;
using GameFramework;
using System.Drawing;
using OpenTK;

namespace OpenTheDoor {
    class Game {
        protected Point spawnTile = new Point(2, 1);
        protected PlayerCharacter hero = null;
        protected string heroSheet = "Assets/Link.png";
        public OpenTK.GameWindow Window = null;
        protected Map room1 = null;
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
            new Rectangle(466,32,30,30),
            new Rectangle(466,1,30,30),
            new Rectangle(32,187,30,30)
        };
        public Tile GetTile(PointF pixelPoint) {
            return currentMap[(int)pixelPoint.Y / 30][(int)pixelPoint.X / 30];
        }
        public Rectangle GetTileRect(PointF pixelPoint) {
            int xTile = (int)pixelPoint.X / 30;//integer math
            int yTile = (int)pixelPoint.Y / 30;
            Rectangle result = new Rectangle(xTile * 30, yTile * 30, 30, 30);
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
            window.ClientSize = new Size(room1Layout[0].Length * 30, room1Layout.Length * 30);
            TextureManager.Instance.UseNearestFiltering = true;

            hero = new PlayerCharacter(heroSheet, new Point(spawnTile.X * 30, spawnTile.Y * 30));
            room1 = new Map(room1Layout, spriteSheets, spriteSources,2,0);
            room2 = new Map(room2Layout, spriteSheets, spriteSources,0,2);
            room1[4][7].MakeDoor(room2,new Point(1,1));
            room2[1][0].MakeDoor(room1, new Point(6,4));
            currentMap = room1;
        }
        public void Update(float dt) {
            currentMap.ResolveDoors(hero);
            hero.Update(dt);
        }
        public void Render() {
            currentMap.Render();
            /*
            foreach (PointF corner in hero.Corners) {
                if (!GetTile(corner).Walkable) {
                    GraphicsManager.Instance.DrawRect(GetTileRect(corner), Color.Violet);
                }
                else {
                    GraphicsManager.Instance.DrawRect(GetTileRect(corner), Color.Blue);
                }
            }*/
            hero.Render();

        }
        public void Shutdown() {
            room1.Destroy();
            room2.Destroy();
            hero.Destroy();
        }
    }
}
