using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GameFramework;

namespace TilesSolo {
    class Game {
        protected Tile[][] map = null;
        /*
        0 = floor (32,32,30,30)
        1 = wall corner upper left (1,1,30,30)
        2 = wall corner lower left (1,63,30,30)
        3 = wall corner lower right (63,1,30,30)
        4 = wall corner, upper right (63,63,30,30)
        5 = lower wall (32,63,30,30)
        6 = upper wall (32,1,30,30)
        7 = left wall (1,32,30,30)
        8 = right wall (63,32,30,30)
        9 = clock (218,125,30,30)
        10 = nightstand (218,94,30,30)
        11 = bed top (94,63,30,30)
        12 = bed bottom (94,94,30,30)
        13 = dresser (218,156,30,30)
        14 = left wall end (94,187,30,30)
        15 = right wall end (125,187,30,30)
        16 = bookshelf top (63,156,30,30)
        17 = bookshelf bottom (63,187,30,30)
        */
        public OpenTK.GameWindow window = null;
        protected int[][] mapLayout = new int[][] {
            new int[] {1, 6, 6, 6,9, 6, 6,4 },
            new int[] {7,16,16,16,0,10,11,8 },
            new int[] {7,17,17,17,0,0,12,8 },
            new int[] {7,0,0,0,0,0,0,8 },
            new int[] {7,0,0,0,0,0,13,8 },
            new int[] {2,5,5,14,15,5,5,3}
        };
        protected string spriteSheets = "Assets/HouseTiles.png";
        protected Rectangle[] spriteSources = new Rectangle[] {
            new Rectangle(32,32,30,30),
            new Rectangle(1,1,30,30),
            new Rectangle(1,63,30,30),
            new Rectangle(63,63,30,30),
            new Rectangle(63,1,30,30),
            new Rectangle(32,63,30,30),
            new Rectangle(32,1,30,30),
            new Rectangle(1,32,30,30),
            new Rectangle(63,32,30,30),
            new Rectangle(218,125,30,30),
            new Rectangle(218,94,30,30),
            new Rectangle(94,63,30,30),
            new Rectangle(94,94,30,30),
            new Rectangle(218,156,30,30),
            new Rectangle(94,187,30,30),
            new Rectangle(125,187,30,30),
            new Rectangle(63,156,30,30),
            new Rectangle(63,187,30,30)
        };
        Tile[][] GenerateMap(int[][] layout, string sheet, Rectangle[] sources) {
            Tile[][] result = new Tile[layout.Length][];
            float scale = 1.0f;
            for (int h = 0; h < layout.Length; h++) {
                result[h] = new Tile[layout[h].Length];
                for (int w = 0; w < layout[h].Length; w++) {
                    Rectangle source = sources[layout[h][w]];
                    Point worldPosition = new Point();
                    worldPosition.X = (int)(w * source.Width);
                    worldPosition.Y = (int)(h * source.Height);
                    result[h][w] = new Tile(sheet, source);
                    result[h][w].Walkable = layout[h][w] == 0 || layout[h][w] == 14 || layout[h][w] == 15;
                    result[h][w].WorldPosition = worldPosition;
                    result[h][w].Scale = scale;
                }
            }
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
        public void Initialize(OpenTK.GameWindow window) {
            this.window = window;
            TextureManager.Instance.UseNearestFiltering = true;
            map = GenerateMap(mapLayout, spriteSheets, spriteSources); 
            window.ClientSize = new Size(mapLayout[0].Length*30, mapLayout.Length*30);
        }
        public void Update(float dTime) {

        }
        public void Render() {
            for (int h = 0; h < mapLayout.Length; h++) {
                for (int w = 0; w < mapLayout[h].Length; w++) {
                    map[h][w].Render();
                }
            }
        }
        public void Shutdown() {
            for (int h = 0; h < mapLayout.Length; h++) {
                for (int w = 0; w < mapLayout[h].Length; w++) {
                    map[h][w].Destroy();
                }
            }
        }
    }
}
