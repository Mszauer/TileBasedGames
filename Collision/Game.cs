using System;
using System.Collections.Generic;
using GameFramework;
using System.Drawing;
using OpenTK;

namespace Collision {
    class Game {
        protected Point spawnTile = new Point(2, 1);
        protected PlayerCharacter hero = null;
        protected string heroSheet = "Assets/Link.png";
        public OpenTK.GameWindow Window = null;
        protected Tile[][] map = null;
        protected int[][] mapLayout = new int[][] {
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 1, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 1, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1 }
        };

        protected string spriteSheets = "Assets/HouseTiles.png";
        protected Rectangle[] spriteSources = new Rectangle[] {
            new Rectangle(466,32,30,30),
            new Rectangle(466,1,30,30)
        };
        Tile[][] GenerateMap(int[][] layout, string sheets, Rectangle[] sources) {
            Tile[][] result = new Tile[layout.Length][];
            float scale = 1.0f;
            for (int i = 0; i < layout.Length; i++) {
                result[i] = new Tile[layout[i].Length];

                for (int j = 0; j < layout[i].Length; j++) {
                    Rectangle source = sources[layout[i][j]];

                    Point worldPosition = new Point();
                    worldPosition.X = (int)(j * source.Width);
                    worldPosition.Y = (int)(i * source.Height);
                    result[i][j] = new Tile(sheets, source);
                    result[i][j].Walkable = layout[i][j] == 0;
                    result[i][j].WorldPosition = worldPosition;
                    Console.WriteLine("Tile " + i + " , " + j + " walkable? " + result[i][j].Walkable);
                    result[i][j].Scale = scale;
                }
            }
            return result;
        }
        public Tile GetTile(PointF pixelPoint) {
            return map[(int)pixelPoint.Y / 30][(int)pixelPoint.X / 30];
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
            window.ClientSize = new Size(mapLayout[0].Length * 30, mapLayout.Length * 30);
            TextureManager.Instance.UseNearestFiltering = true;
            map = GenerateMap(mapLayout, spriteSheets, spriteSources);
            hero = new PlayerCharacter(heroSheet, new Point(spawnTile.X * 30, spawnTile.Y * 30));


        }
        public void Update(float dt) {

            hero.Update(dt);
        }
        public void Render() {
            for (int h = 0; h < map.Length; h++) {
                for (int w = 0; w < map[h].Length; w++) {
                    map[h][w].Render();
                }
            }
            foreach(PointF corner in hero.Corners) {
                if (!GetTile(corner).Walkable) {
                    GraphicsManager.Instance.DrawRect(GetTileRect(corner), Color.Blue);
                }
                else {
                    GraphicsManager.Instance.DrawRect(GetTileRect(corner), Color.Blue);
                }
            }
            hero.Render();

        }
        public void Shutdown() {
            for (int h = 0; h < map.Length; h++) {
                for (int w = 0; w < map[h].Length; w++) {
                    map[h][w].Destroy();
                }
            }
            hero.Destroy();
        }
    }
}
