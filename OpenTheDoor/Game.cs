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
        protected Tile[][] room1 = null;
        protected int[][] room1Layout = new int[][] {
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 1, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 1, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 2 },
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1 }
        };
        protected Tile[][] room2 = null;
        protected int[][] room2Layout = new int[][] {
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1 },
            new int[] { 2, 0, 0, 0, 1, 0, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 0, 0, 1, 0, 0, 1 },
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1 }
        };
        protected Tile[][] currentRoom = null;
        protected int[][] currentLayout = null;
        protected string spriteSheets = "Assets/HouseTiles.png";
        protected Rectangle[] spriteSources = new Rectangle[] {
            new Rectangle(466,32,30,30),
            new Rectangle(466,1,30,30),
            new Rectangle(32,187,30,30)
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
                    result[i][j].Walkable = layout[i][j] == 0 || layout[i][j] == 2;
                    result[i][j].WorldPosition = worldPosition;
                    result[i][j].Scale = scale;
                }
            }
            return result;
        }
        public Tile GetTile(PointF pixelPoint) {
            return currentRoom[(int)pixelPoint.Y / 30][(int)pixelPoint.X / 30];
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
            room1 = GenerateMap(room1Layout, spriteSheets, spriteSources);
            room2 = GenerateMap(room2Layout, spriteSheets, spriteSources);
            hero = new PlayerCharacter(heroSheet, new Point(spawnTile.X * 30, spawnTile.Y * 30));
            currentRoom = room2;
            currentLayout = room2Layout;
        }
        public void Update(float dt) {
            hero.Update(dt);

            for (int row = 0; row < currentLayout.Length; row++) {
                for (int col = 0; col < currentLayout[row].Length; col++) {
                    if (currentLayout[row][col] == 2) {
                        //get doors bouding rectangle
                        Rectangle doorRect = GetTileRect(new PointF(col * 30, row * 30));

                        //get a small rectangle in center of the player
                        Rectangle playerCenter = new Rectangle((int)hero.Center.X - 2, (int)hero.Center.Y - 2, 4, 4);

                        //look for an intersection
                        Rectangle intersection = Intersections.Rect(doorRect, playerCenter);
                        if (intersection.Width*intersection.Height > 0) {
                            if (currentRoom == room1) {
                                currentRoom = room2;
                                currentLayout = room2Layout;

                                hero.Position.X = 1 * 30;
                                hero.Position.Y = 1 * 30;
                            }
                            else {
                                currentRoom = room1;
                                currentLayout = room1Layout;
                                hero.Position.X = 6 * 30;
                                hero.Position.Y = 4 * 30;
                            }
                        }
                    }
                }
            }
        }
        public void Render() {
            for (int h = 0; h < currentRoom.Length; h++) {
                for (int w = 0; w < currentRoom[h].Length; w++) {
                    currentRoom[h][w].Render();
                }
            }
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
            for (int h = 0; h < room1.Length; h++) {
                for (int w = 0; w < room1[h].Length; w++) {
                    room1[h][w].Destroy();
                }
            }
            for (int h = 0; h < room2.Length; h++) {
                for (int w = 0; w < room2[h].Length; w++) {
                    room2[h][w].Destroy();
                }
            }
            hero.Destroy();
        }
    }
}
