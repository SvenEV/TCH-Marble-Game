using SiliconStudio.Xenko.Engine;

namespace TchMarbleGame
{
    class TchMarbleGameApp
    {
        static void Main(string[] args)
        {
            using (var game = new Game())
            {
                game.Run();
            }
        }
    }
}
