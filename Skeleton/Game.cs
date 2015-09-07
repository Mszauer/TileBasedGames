using System;
using System.Collections.Generic;
using GameFramework;

namespace Skeleton {
    class Game {
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
        public void Initialize() {

        }
        public void Update(float dt) {

        }
        public void Render() {

        }
        public void Shutdown() {

        }
    }
}
