using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilesSolo {
    class Game {
        protected Tile[][] map = null;
        /*
        0 = floor (32,32,30,30)
        1 = wall corner upper left (1,1,30,30)
        2 = wall corner lower left (1,63,30,30)
        3 = wall corner lower right (32,125,30,30)
        4 = wall corner, upper right (32,94,30,30)
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
        */
        protected int[][] mapLayout = new int[][] {
            new int[] {1,1,1,1,1,1,1,1 },
            new int[] {1,1,1,1,0,1,1,1 },
            new int[] {1,1,1,1,0,0,1,1 },
            new int[] {1,0,0,0,0,0,1,1 },
            new int[] {1,0,0,0,0,0,0,1 },
            new int[] {1,0,0,0,0,0,1,1 },
            new int[] {1,1,1,0,0,1,1,1}
        };
        
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
    }
}
