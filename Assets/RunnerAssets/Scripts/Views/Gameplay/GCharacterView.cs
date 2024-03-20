using System.Linq;
using Reflex.Attributes;
using RunnerModel;
using Settings;
using StarterAssets;
using UnityEngine;

namespace Views.Gameplay
{
    /**
     * Envelope around the character controller.
     */
    public class GCharacterView : MonoBehaviour
    {
        [Inject] private ROModel _model;
        
        private ModifiedThirdPersonController _controller =>
            _controllerValue ??= GetComponent<ModifiedThirdPersonController>();
        private ModifiedThirdPersonController _controllerValue;

        public void SetFrozen(bool val)
        {
            _controller.IsFrozen = val;
        }

        private void Start()
        {
            var control = GetComponent<StarterAssetsInputs>();
            control.move = Vector2.right;
            control.sprint = true;
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
    }
}