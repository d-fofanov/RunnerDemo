using System;
using System.Linq;
using Controllers;
using Reflex.Attributes;
using RunnerModel;
using RX;
using Settings;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Views.Gameplay
{
    /**
     * Envelope around the character controller.
     */
    public class GCharacterView : MonoBehaviour
    {
        private ModifiedThirdPersonController _controller =>
            _controllerValue ??= GetComponent<ModifiedThirdPersonController>();
        private ModifiedThirdPersonController _controllerValue;
        private CompositeDisposable _disposable = new();
        private ROModel _model;

        [Inject]
        public void Init(ROModel model, InputController inputController)
        {
            _model = model;
            inputController.JumpInput.Subscribe(OnJumpInputChanged).AddTo(_disposable);
        }

        public void SetFrozen(bool val)
        {
            _controller.IsFrozen = val;
        }

        private void Start()
        {
            var control = _controller.InputsData;
            control.move = Vector2.right;
            control.sprint = true;
        }

        private void OnJumpInputChanged(bool val)
        {
            _controller.InputsData.jump = val;
        }

        private void Update()
        {
            if (_model == null)
                return;

            var aggregated = CharacterEffectSettings.Aggregate(
                _model.ROGame.CharacterEffects.Select(e => e.Settings)
            );

            _controller.Modifiers = aggregated;
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}