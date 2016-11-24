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
            ui.Page = new UIPage
            {
                RootElement = new Border
                {
                    BackgroundColor = Color.Black,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Padding = new Thickness(4, 4, 4, 4),
                    Content = _uiText = new TextBlock
                    {
                        TextColor = Color.White,
                        Font = Font
                    }
                }
            };
        }

        public override void Update()
        {
            _uiText.Text = Game.UpdateTime.FramePerSecond.ToString("0");
        }
    }
}
