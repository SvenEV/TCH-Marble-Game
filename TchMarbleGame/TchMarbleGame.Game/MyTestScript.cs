using SiliconStudio.Xenko.Engine;

namespace TchMarbleGame
{
    public class MyTestScript : SyncScript
    {
        // Declared public member fields and properties will show in the game studio
        public ButtonComponent Button { get; set; }

        public override void Start()
        {
            // Initialization of the script.
            Button.Pressed += OnButtonPressed;
            Button.Released += OnButtonReleased;
        }

        private void OnButtonReleased(ButtonComponent obj)
        {
            Entity.Get<BalloonBehavior>().IsInflated = false;
        }

        private void OnButtonPressed(ButtonComponent obj)
        {
            Entity.Get<BalloonBehavior>().IsInflated = true;
        }

        public override void Update()
        {
            // Do stuff every new frame
        }
    }
}
