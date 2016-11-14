using SiliconStudio.Xenko.Engine;
using System;

namespace TchMarbleGame
{
    class TchMarbleGameApp
    {
        private static Game _game;

        static void Main(string[] args)
        {
            using (_game = new Game())
            {
                _game.WindowCreated += OnWindowCreated;
                _game.Run();
            }
        }

        private static void OnWindowCreated(object sender, EventArgs e)
        {
            _game.Window.AllowUserResizing = true;
        }
    }
}
