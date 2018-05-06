using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG_Library
{
    public interface IInputManager
    {
        MouseState PreviousMouseState { get; }
        MouseState CurrentMouseState { get; }

        KeyboardState PreviousKeyboardState { get; }
        KeyboardState CurrentKeyboardState { get; }

        bool MouseCursorOver(Rectangle bound);
        bool MouseButtonClicked(MouseButton btn);
        bool MouseButtonHeld(MouseButton btn);
        bool IsMouseButtonReleased(MouseButton btn);
        bool IsMouseButtonPressed(MouseButton btn);
        bool WasMouseButtonReleased(MouseButton btn);
        bool WasMouseButtonPressed(MouseButton btn);
    }

    public enum MouseButton
    {
        Left, Middle, Right
    };

    public class InputManager : GameComponent, IInputManager
    {
        // #region #endregion tags are a nice way of blockifying code in VS.
        #region Fields
        // Store current and previous states for comparison. 
        private MouseState previousMouseState;
        private MouseState currentMouseState;

        // Some keyboard states for later use.
        private KeyboardState previousKeyboardState;
        private KeyboardState currentKeyboardState;
        #endregion

        #region Property
        public MouseState PreviousMouseState { get => previousMouseState; }
        public MouseState CurrentMouseState { get => currentMouseState; }

        public KeyboardState PreviousKeyboardState { get => previousKeyboardState; }
        public KeyboardState CurrentKeyboardState { get => currentKeyboardState; }
        #endregion

        public InputManager(Game game) : base(game)
        {
            previousMouseState = Mouse.GetState();
            previousKeyboardState = Keyboard.GetState();
            Game.Services.AddService<IInputManager>(this);
        }

        #region GameComponent Override

        public override void Initialize()
        {
            base.Initialize(); 
        }

        public override void Update(GameTime gameTime)
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            base.Update(gameTime);
        }

        #endregion

        #region Mouse Methods

        public bool MouseCursorOver(Rectangle bound)
        {
            return bound.Contains(currentMouseState.X, currentMouseState.Y);
        }

        public bool MouseButtonClicked(MouseButton btn)
        {
            return (WasMouseButtonPressed(btn) || IsMouseButtonReleased(btn))
                && (WasMouseButtonReleased(btn) || IsMouseButtonPressed(btn));
        }

        public bool MouseButtonHeld(MouseButton btn)
        {
            return WasMouseButtonPressed(btn) && IsMouseButtonPressed(btn);
        }

        public bool IsMouseButtonReleased(MouseButton btn)
        {
            // Simply returns whether the button state is released or not.
            switch (btn)
            {
                case MouseButton.Left:
                    return currentMouseState.LeftButton == ButtonState.Released;
                case MouseButton.Middle:
                    return currentMouseState.MiddleButton == ButtonState.Released;
                case MouseButton.Right:
                    return currentMouseState.RightButton == ButtonState.Released;
            }
            return false;
        }

        public bool WasMouseButtonReleased(MouseButton btn)
        {
            switch (btn)
            {
                case MouseButton.Left:
                    return previousMouseState.LeftButton == ButtonState.Released;
                case MouseButton.Middle:
                    return previousMouseState.MiddleButton == ButtonState.Released;
                case MouseButton.Right:
                    return previousMouseState.RightButton == ButtonState.Released;
            }
            return false;
        }

        public bool IsMouseButtonPressed(MouseButton btn)
        {
            // This will just call the method above and negate.
            return !IsMouseButtonReleased(btn);
        }

        public bool WasMouseButtonPressed(MouseButton btn)
        {
            return !WasMouseButtonReleased(btn);
        }

        #endregion

        #region Keyboard Methods

        // TODO:

        #endregion
    }
}

