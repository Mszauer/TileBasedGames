using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GameFramework;
using System.IO;

namespace MouseToMove {
    class Map {
        protected Tile[][] tileMap = null;

        public Tile[] this[int i] {
            get {
                return tileMap[i];
            }
        }
        public int Length {
            get {
                return tileMap.Length;
            }
        }
        int[] row = null;
        int[] col = null;
        Tile[][] map = null;

        public Map(string mapPath){
            if (System.IO.File.Exists(mapPath)){
                Console.WriteLine("Loading map...");
                using (TextReader reader = File.OpenText(mapPath)) {
                    string contents = reader.ReadLine();
                    row = new int[contents.Length];

                    while (contents != null) {
                        List<string> content = contents.Split(' ');
                        //do map stuff here
                        if (content[0] >= 0) {
                            //load rows
                            for (int i = 0; i < content.Length; i++) {
                                row[i] = (int)content[i];
                            }
                        }
                        if (content[0] == 'T') {
                            //texture path
                            string path = content;
                            for (int i = 2; i < content.Length; i++) {
                                //add the chars into a string
                                path += content[i];
                            }
                            Game.TileSheet = path;
                        }
                        if (content[0] == 'R') {
                            //source rectangle from texture
                            //split by space
                            Rectangle r = new Rectangle(content[1], content[2], content[3], content[4]); //what about double digits?
                            Game.SpriteSources.Add
                            //what type of tile
                            //rectangle size

                        }
                        if (content[0] == 'W') {
                            //which tiles are walkable
                        }
                        if (content[0] == 'D') {
                            //identifies which tile is a door
                            //door destination
                        }
                        if(content[0] == 'S') {
                            //starting tile
                            Game.SpawnTile = new Point((int)content[1], (int)content[2]);
                        }
                        contents = reader.ReadLine();
                    }
                }
                Console.WriteLine("Map has been laoded");
            }
            else {
                Console.WriteLine("Map not found!");
            }
        }
        
        /*public Map(int[][] layout, string sheets, Rectangle[] sources, params int[] walkable) {
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
                    result[i][j].Walkable = false;
                    result[i][j].IsDoor = false;
                    result[i][j].WorldPosition = worldPosition;
                    result[i][j].Scale = scale;
                    foreach (int w in walkable) {
                        if (layout[i][j] == w) {
                            tileMap[i][j].Walkable = true;
                        }
                    }
                }
                tileMap = result;
            }
        }
        */
        public Map ResolveDoors(PlayerCharacter hero) {
            Map result = this;
            for (int row = 0; row < tileMap.Length; row++) {
                for (int col = 0; col < tileMap[row].Length; col++) {
                    if (tileMap[row][col].IsDoor) {
                        //get doors bouding rectangle
                        Rectangle doorRect = new Rectangle(col * 30, row * 30, 30, 30);
                        //get a small rectangle in center of the player
                        Rectangle playerCenter = new Rectangle((int)hero.Center.X - 2, (int)hero.Center.Y - 2, 4, 4);

                        //look for an intersection
                        Rectangle intersection = Intersections.Rect(doorRect, playerCenter);
                        if (intersection.Width * intersection.Height > 0) {
                            result = tileMap[row][col].DoorTarget;
                            hero.Position.X = tileMap[row][col].DoorLocation.X * 30;
                            hero.Position.Y = tileMap[row][col].DoorLocation.Y * 30;
                            hero.SetTargetTile(tileMap[row][col].DoorLocation);
                        }
                    }
                }
            }
            return result;
        }
        public void Render() {
            for (int h = 0; h < tileMap.Length; h++) {
                for (int w = 0; w < tileMap[h].Length; w++) {
                    tileMap[h][w].Render();
                }
            }
        }
        public void Destroy() {
            for (int h = 0; h < tileMap.Length; h++) {
                for (int w = 0; w < tileMap[h].Length; w++) {
                    tileMap[h][w].Destroy();
                }
            }
        }
    }
}
