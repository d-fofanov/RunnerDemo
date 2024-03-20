using System;
using System.Linq;
using RX;
using UnityEngine.InputSystem;
using Utils;

namespace Controllers
{
    /**
     * Handles input from the player.
     * For a more sophisticated input scheme should be replaced with a proper InputSystem usage.
     */
    public class InputController : IDisposable
    {
        public ReadOnlyReactiveProperty<bool> JumpInput => _jumpInput;

        private readonly ReactiveProperty<bool> _jumpInput = new();
        private CompositeDisposable _disposable = new();
        
        public InputController(TimeUtil timeUtil)
        {
            timeUtil.AddUpdateAction(OnUpdate).AddTo(_disposable);
        }
        
        public void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
        
        private void OnUpdate(float _)
        {
            _jumpInput.Value = (Keyboard.current?.spaceKey.wasPressedThisFrame ?? false) ||
                (Touchscreen.current?.touches.Any(t => t.phase.value == TouchPhase.Began) ?? false);
        }
    }
}