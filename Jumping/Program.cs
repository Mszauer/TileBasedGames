using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using GameFramework;
using System.Drawing;

namespace Jumping {
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
            GraphicsManager.Instance.ClearScreen(System.Drawing.Color.CadetBlue);
            int FPS = (int)(1 / e.Time);
            Game.Instance.Render();
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
        [STAThread]
        static void Main(string[] args) {
            Window = new OpenTK.GameWindow();
            Window.Title = "Doors";
            Window.Load += new EventHandler<EventArgs>(Initialize);
            Window.UpdateFrame += new EventHandler<FrameEventArgs>(Update);
            Window.RenderFrame += new EventHandler<FrameEventArgs>(Render);
            Window.Unload += new EventHandler<EventArgs>(Shutdown);
            Window.Run(60.0);
            Window.Dispose();
#if DEBUG
            Console.WriteLine("\nFinished executing, press any key to exit...");
            Console.ReadKey();
#endif
        }
    }
}
