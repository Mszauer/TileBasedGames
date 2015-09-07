using System;
using OpenTK;
using OpenTK.Input;
using System.Drawing;
using GameFramework;
using System.Collections.Generic;

namespace Skeleton {
    class Program {
        public static OpenTK.GameWindow Window = null;

        public static void Initialize(object sender, EventArgs e) {
            GraphicsManager.Instance.Initialize(Window);
            TextureManager.Instance.Initialize(Window);
            InputManager.Instance.Initialize(Window);
            SoundManager.Instance.Initialize(Window);
            Game.Instance.Initialize();
        }

        public static void Update(object sender, FrameEventArgs e) {
            float deltaTime = (float)e.Time;
            InputManager.Instance.Update();
            Game.Instance.Update(deltaTime);
        }

        public static void Render(object sender, FrameEventArgs e) {
            GraphicsManager.Instance.ClearScreen(Color.CadetBlue);

            Game.Instance.Render();

            // Show framerate
            int FPS = System.Convert.ToInt32(1.0 / e.Time);
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
        public static void Main() {
            // Create static (global) window instance
            Window = new OpenTK.GameWindow();

            // Hook up the initialize callback
            Window.Load += new EventHandler<EventArgs>(Initialize);
            // Hook up the update callback
            Window.UpdateFrame += new EventHandler<FrameEventArgs>(Update);
            // Hook up the render callback
            Window.RenderFrame += new EventHandler<FrameEventArgs>(Render);
            // Hook up the shutdown callback
            Window.Unload += new EventHandler<EventArgs>(Shutdown);

            // Set window title and size
            Window.Title = "Game Name";
            Window.ClientSize = new Size(800, 600);
            Window.VSync = VSyncMode.On;
            // Run the game at 60 frames per second. This method will NOT return
            // until the window is closed.
            Window.Run(60.0f);

            // If we made it down here the window was closed. Call the windows
            // Dispose method to free any resources that the window might hold
            Window.Dispose();

#if DEBUG
            Console.WriteLine("\nFinished executing, press any key to exit...");
            Console.ReadKey();
#endif
        }
    }
}