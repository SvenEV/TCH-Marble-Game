using System;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.UI.Controls;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.UI;
using SiliconStudio.Xenko.Graphics;

namespace TchMarbleGame
{
    public class FpsCounter : SyncScript
    {
        private TextBlock _uiText;

        public SpriteFont Font { get; set; }

        public override void Start()
        {
            var ui = Entity.GetOrCreate<UIComponent>();
            _uiText = ui.Page.RootElement.FindName("textBlock") as TextBlock;
        }

        public override void Update()
        {
            _uiText.Text = Game.UpdateTime.FramePerSecond.ToString("0");
        }
    }
}
