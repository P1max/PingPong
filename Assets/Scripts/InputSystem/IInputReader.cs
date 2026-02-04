using System;

namespace InputSystem
{
    public interface IInputReader
    {
        public event Action OnUpAction;
        public event Action OnDownAction;
        public event Action OnReleaseAction;
    }
}