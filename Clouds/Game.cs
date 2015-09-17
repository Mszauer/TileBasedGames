using System;
using System.Collections.Generic;
using GameFramework;
using System.Drawing;
using OpenTK;

namespace Clouds {
    class Game {
        protected Point spawnTile = new Point(2, 1);
        protected PlayerCharacter hero = null;
        protected string heroSheet = "Assets/Link.png";
        public OpenTK.GameWindow Window = null;
        protected Map room1 = null;
        protected Map room2 = null;
        protected int[][] room1Layout = new int[][] {
            new int[] { 3, 3, 3, 3, 3, 3, 3, 3 },
            new int[] { 3, 4, 4, 4, 4, 4, 4, 3 },
            new int[] { 3, 4, 4, 4, 4, 4, 4, 3 },
            new int[] { 3, 4,11,12, 4, 4, 4, 3 },
            new int[] { 3, 4, 4, 4, 4, 4, 4, 2 },
            new int[] { 3, 5, 5, 5, 5, 5, 5, 5 }
        };
        protected int[][] room2Layout = new int[][] {
            new int[] { 3, 3, 3, 3, 3, 3, 3, 3 },
            new int[] { 3, 4, 4, 4, 4, 4, 8, 3 },
            new int[] { 3, 4, 4, 4, 4, 4, 9, 3 },
            new int[] { 3, 4, 4, 4, 4, 4,10, 3 },
            new int[] { 2, 4, 4, 4, 4, 4, 4, 3 },
            new int[] { 5, 5, 5, 5, 5, 5, 5, 3 }
        };
        protected Map currentMap = null;
        protected string spriteSheets = "Assets/HouseTiles.png";
        protected Rectangle[] spriteSources = new Rectangle[] {
            /* 0  */new Rectangle(466,32,30,30),
            /* 1  */new Rectangle(466,1,30,30),
            /* 2  */new Rectangle(32,187,30,30),
            /* 3  */new Rectangle(466, 125,30,30), // blue border
            /* 4  */new Rectangle(311, 249,30,30), // blackness
            /* 5  */new Rectangle(466, 63,30,30), // ground layer
            /* 6  */new Rectangle(63, 218,30,30), // blank ladder
            /* 7  */new Rectangle(156, 218,30,30), // ground ladder
            /* 8  */new Rectangle(63, 249,30,30),  // skele 1
            /* 9  */new Rectangle(94, 249,30,30), // slele 2
            /* 10 */new Rectangle(125, 249,30,30), // skele 3
            /* 11 */new Rectangle(156, 249,30,30), // cloud 1
            /* 12 */new Rectangle(187, 249,30,30), // cloud 2
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
            room1 = new Map(room1Layout, spriteSheets, spriteSources, 2, 0, 4, 8, 9, 10);
            room2 = new Map(room2Layout, spriteSheets, spriteSources, 0, 2, 4, 8, 9, 10);
            room1[4][7].MakeDoor(room2, new Point(1, 4));
            room2[4][0].MakeDoor(room1, new Point(6, 4));
            room1[3][2].IsCloud = true;
            room1[3][3].IsCloud = true;
            currentMap = room1;
        }
        public void Update(float dt) {
            currentMap = currentMap.ResolveDoors(hero);
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
