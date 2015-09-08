using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
using System.Drawing;

namespace TilesEC {
    class Game {
        protected OpenTK.GameWindow window = null;
        protected static Tile[][] map = null;
        /*
        0 = tile(#103)
        1 = wall
        2 = fireplace 403,63,30,30
        3 = water
        4 = water upper left border
        5 = water upper border
        6 = water upper right border
        7 = water right border
        8 = water lower right corner
        9 = water bottom border
        10 = water lower left corner
        11 = water left border
        12 = upper left corner
        13 = upper right corner
        14 = lower left corner
        15 = lower right corner
        */
        protected static int[][] mapLayout = new int[][] {
            new int[] {12,1, 1, 1, 1, 1, 13},
            new int[] {1 ,2, 0, 0, 0, 2, 1 },
            new int[] {1 ,0, 4, 5, 6, 0, 1 },
            new int[] {1 ,0,11, 3, 7, 0, 1 },
            new int[] {1 ,0,10, 9, 8, 0, 1 },
            new int[] {1 ,2, 0, 0, 0, 2, 1 },
            new int[] {14,1, 1, 1, 1, 1, 15},
        };
        protected string spriteSheet = "Assets/TileHouse.png";
        protected Rectangle[] spriteSources = new Rectangle[] {
            new Rectangle(435,156,30,30),
            new Rectangle(32,1,30,30),
            new Rectangle(0,0,0,0),
        };
        Tile[][] GenerateMap(int[][] layout,string spriteSheet, Rectangle[] sources) {
            Tile[][] result = new Tile[layout.Length][];
            float scale = 1.0f;
            for (int i = 0; i < layout.Length; i++) {
                result[i] = new Tile[layout[i].Length];
                for (int j = 0; j < layout[i].Length; j++) {
                    result[i][j].Scale = scale;
                    Rectangle source = sources[layout[i][j]];
                    Point worldPosition = new Point(0, 0);
                    worldPosition.X = (int)(j * source.Width);
                    worldPosition.Y = (int)(i * source.Height);
                    result[i][j] = new Tile(spriteSheet, source);
                    result[i][j].Scale = scale;
                    result[i][j].Walkable = layout[i][j] == 0;
                    result[i][j].WorldPosition = worldPosition;
                }
            }
            return result;
        }

        private static Game instance = null;
        public static Game Instance {
            get {
                if (instance == null) {
                    instance = new Game();
                }
                return instance;
            }
        }
        public void Initialize(OpenTK.GameWindow window) {
            this.window = window;
            TextureManager.Instance.UseNearestFiltering = true;
            map = GenerateMap(mapLayout, spriteSheet, spriteSources);
            window.ClientSize = new Size(mapLayout[0].Length * 30, mapLayout.Length * 30);
        }
        public void Update(float dTime) {

        }
        public void Render() {
            for(int i = 0; i < mapLayout.Length; i++) {
                for (int j = 0; j < mapLayout[i].Length; j++) {
                    map[i][j].Render();
                }
            }
        }
        public void Destroy() {
            for (int i = 0; i < mapLayout.Length; i++) {
                for (int j = 0; j < mapLayout[i].Length; j++) {
                    map[i][j].Destroy();
                }
            }
        }
    }
}
