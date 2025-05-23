using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace TestMiniProj_1
{
    internal class myButton : Transformable, Drawable
    {
        public enum ButtonState
        {
            Normal,    // 默认状态
            Hovered,   // 鼠标悬停
            Pressed    // 鼠标按下
        }
        private RectangleShape _background;
        private Text _text;
        private ButtonState _currentState = ButtonState.Normal;
        public bool AllowSpaceKeyTrigger { get; set; } = true;
        private bool _wasSpacePressed = false;

        // 状态颜色配置
        public Color NormalColor { get; set; } = new Color(100, 100, 100);
        public Color HoveredColor { get; set; } = new Color(150, 150, 150);
        public Color PressedColor { get; set; } = new Color(200, 200, 200);
        public Color TextColor { get; set; } = Color.White;

        // 事件委托
        public event Action OnClick;
        public event Action OnMouseEnter;
        public event Action OnMouseLeave;

        public myButton(string label, Font font, uint fontSize = 20)
        {
            // 初始化背景矩形
            _background = new RectangleShape();
            _background.FillColor = NormalColor;

            // 初始化文本
            _text = new Text(label, font, fontSize);
            _text.FillColor = TextColor;
            
            UpdateSize(); // 根据文本调整按钮大小
        }

        public myButton(string label): this(label, Config.DEAFULT_FONT, 20)
        {

        }

        // 更新按钮尺寸（根据文本自动调整）
        private void UpdateSize()
        {
            FloatRect textBounds = _text.GetLocalBounds();
            _background.Size = new Vector2f(textBounds.Width + 40, textBounds.Height + 20);

            // 文本居中
            _text.Origin = new Vector2f(
                textBounds.Left + textBounds.Width / 2,
                textBounds.Top + textBounds.Height / 2
            );
            _text.Position = _background.Size / 2;
        }

        
        public void HandleInput()
        {
            // 处理鼠标交互
            //HandleMouseInput(window);

            // 处理键盘交互（空格键）
            if (AllowSpaceKeyTrigger)
            {
                bool isSpacePressed = Keyboard.IsKeyPressed(Keyboard.Key.Space);

                if (isSpacePressed && !_wasSpacePressed)
                {
                    // 触发点击事件
                    _currentState = ButtonState.Pressed;
                    OnClick?.Invoke();
                }
                _wasSpacePressed = isSpacePressed;
            }
        }

        // 处理鼠标事件
        public void HandleMouseInput(RenderWindow window)
        {
            Vector2f mousePos = window.MapPixelToCoords(
                Mouse.GetPosition(window)
            );

            // 转换鼠标坐标到按钮局部空间
            Vector2f localMousePos = Transform.GetInverse().TransformPoint(mousePos);

            bool contains = _background.GetGlobalBounds().Contains(localMousePos.X, localMousePos.Y);

            ButtonState oldState = _currentState;

            if (contains)
            {
                _currentState = Mouse.IsButtonPressed(Mouse.Button.Left)
                    ? ButtonState.Pressed
                    : ButtonState.Hovered;

                // 触发事件
                if (_currentState == ButtonState.Pressed && oldState != ButtonState.Pressed)
                    OnClick?.Invoke();

                if (oldState != ButtonState.Hovered && _currentState == ButtonState.Hovered)
                    OnMouseEnter?.Invoke();
            }
            else
            {
                _currentState = ButtonState.Normal;
                if (oldState == ButtonState.Hovered)
                    OnMouseLeave?.Invoke();
            }

            // 更新颜色
            switch (_currentState)
            {
                case ButtonState.Hovered:
                    _background.FillColor = HoveredColor;
                    break;
                case ButtonState.Pressed:
                    _background.FillColor = PressedColor;
                    break;
                default:
                    _background.FillColor = NormalColor;
                    break;
            }
        }

        // 实现Drawable接口
        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform; // 应用按钮的变换（旋转/缩放/移动）

            target.Draw(_background, states);
            target.Draw(_text, states);
        }

        // 属性封装
        public string Text
        {
            get => _text.DisplayedString;
            set
            {
                _text.DisplayedString = value;
                UpdateSize();
            }
        }

    }
}
