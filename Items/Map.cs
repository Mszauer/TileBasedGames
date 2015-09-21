using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GameFramework;

namespace Items {
    class Map {
        protected Tile[][] tileMap = null;
        List<EnemyCharacter> enemies = null;
        List<Item> items = null;
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
        public Map(int[][] layout, string sheets, Rectangle[] sources, params int[] walkable) {
            items = new List<Item>();
            enemies = new List<EnemyCharacter>();
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
                        }
                    }
                }
            }
            return result;
        }
        public void AddEnemy(string spritePath, Point pos, bool moveUpDown) {
            enemies.Add(new EnemyCharacter(spritePath, pos, moveUpDown));
        }
        public void AddItem(string spritePath,Rectangle source, int value, Point position) {
            items.Add(new Item(spritePath, source, value, position));
        }
        public void Update(float dTime, PlayerCharacter hero, List<Bullet> projectiles) {
            for (int i = projectiles.Count - 1; i >= 0; i--) {
                int xTile = (int)projectiles[i].Position.X / 30;
                int yTile = (int)projectiles[i].Position.Y / 30;
                if (xTile >= this[0].Length || xTile < 0) {
                    projectiles.RemoveAt(i);
                }
                else if (yTile >= this.Length || yTile < 0) {
                    projectiles.RemoveAt(i);
                }
                else if (!this[yTile][xTile].Walkable) {
                    projectiles.RemoveAt(i);
                }
            }
            for (int i = enemies.Count - 1; i >= 0; i--) {
                enemies[i].Update(dTime);
                Rectangle intersection = Intersections.Rect(hero.Rect, enemies[i].Rect);
                if (intersection.Width * intersection.Height > 0) {
                    Game.Instance.GameOver = true;
                }
                for (int j = projectiles.Count - 1; j >= 0; j--) {
                    intersection = Intersections.Rect(enemies[i].Rect, projectiles[j].Rect);
                    if (intersection.Width * intersection.Height > 0) {
                        enemies.RemoveAt(i);
                        projectiles.RemoveAt(j);
                        break;
                    }
                }
            }
            for (int i = items.Count - 1; i >= 0; i--) {
                Rectangle intersection = Intersections.Rect(items[i].Rect, hero.Rect);
                if (intersection.Height*intersection.Width > 0) {
                    Game.Instance.Score += items[i].Value;
                    items.RemoveAt(i);
                }
            }
        }
        public void Render() {
            for (int h = 0; h < tileMap.Length; h++) {
                for (int w = 0; w < tileMap[h].Length; w++) {
                    tileMap[h][w].Render();
                }
            }
            for (int i = 0; i < items.Count; i++) {
                items[i].Render();
            }
            for (int i = 0; i < enemies.Count; i++) {
                enemies[i].Render();
            }
        }
        public void Destroy() {
            for (int h = 0; h < tileMap.Length; h++) {
                for (int w = 0; w < tileMap[h].Length; w++) {
                    tileMap[h][w].Destroy();
                }
            }
            for (int i = 0; i < items.Count; i++) {
                items[i].Destroy();
            }
            for (int i = 0; i < enemies.Count; i++) {
                enemies[i].Destroy();
            }
        }

    }
}
