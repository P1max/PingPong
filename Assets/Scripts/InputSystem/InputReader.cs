using System;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputReader : InputControls.IDefaultActions, IInputReader
    {
        private readonly InputControls _input;

        private Buttons _lastPressedButton;

        public event Action OnUpAction;
        public event Action OnDownAction;
        public event Action OnReleaseAction;

        public InputReader()
        {
            _input = new InputControls();

            _input.Default.SetCallbacks(this);
            _input.Default.Enable();
        }

        public void OnUp(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnUpAction?.Invoke();

                _lastPressedButton = Buttons.Up;
            }

            if (context.canceled && _lastPressedButton == Buttons.Up) OnReleaseAction?.Invoke();
        }

        public void OnDown(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnDownAction?.Invoke();

                _lastPressedButton = Buttons.Down;
            }

            if (context.canceled && _lastPressedButton == Buttons.Down) OnReleaseAction?.Invoke();
        }

        public void Dispose()
        {
            _input.Default.Disable();
            _input.Dispose();
        }

        #region enum

        private enum Buttons
        {
            Up = 0,
            Down = 1,
        }

        #endregion
    }
}