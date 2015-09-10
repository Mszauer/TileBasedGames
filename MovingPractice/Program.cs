using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
using System.Drawing;
using OpenTK;

namespace MovingPractice {
    class Program {
        public static OpenTK.GameWindow Window = null;
        public static void Initialize(object sender, EventArgs e) {
            GraphicsManager.Instance.Initialize(Window);
            TextureManager.Instance.Initialize(Window);
            InputManager.Instance.Initialize(Window);
            SoundManager.Instance.Initialize(Window);
            Game.Instance.Initialize(Window);
        }
        public static void Update(object sender, FrameEventArgs e) {
            float dTime = (float)e.Time;
            InputManager.Instance.Update();
            Game.Instance.Update(dTime);
        }
        public static void Render(object sender, FrameEventArgs e) {
            GraphicsManager.Instance.ClearScreen(Color.CadetBlue);
            Game.Instance.Render();
            int FPS = (int)(1 / e.Time);
            GraphicsManager.Instance.DrawString("FPS: " + FPS, new PointF(5, 5), Color.Black);
            GraphicsManager.Instance.DrawString("FPS: " + FPS, new PointF(4, 4), Color.White);
            GraphicsManager.Instance.SwapBuffers();
        }
        public static void Shutdown(object sender, EventArgs e) {
            Game.Instance.Shutdown();
            SoundManager.Instance.Shutdown();
            InputManager.Instance.Shutdown();
            TextureManager.Instance.Shutdown();
            GraphicsManager.Instance.Shutdown();
        }
        static void Main(string[] args) {
            Window = new OpenTK.GameWindow();
            Window.Load += new EventHandler<EventArgs>(Initialize);
            Window.UpdateFrame += new EventHandler<FrameEventArgs>(Update);
            Window.RenderFrame += new EventHandler<FrameEventArgs>(Render);
            Window.Unload += new EventHandler<EventArgs>(Shutdown);
            Window.VSync = VSyncMode.On;
            Window.Run(60.0f);
            Window.Dispose();
#if DEBUG
            Console.WriteLine("\nFinished Executing,press any key to exit...");
            Console.ReadKey();
#endif
        }
    }
}
