using System;
using Controllers;
using Reflex.Attributes;
using RunnerModel;
using RX;
using TMPro;
using UnityEngine;

namespace Views.UI
{
    /**
     * UI view for the gameplay.
     * Contains the score, victory and defeat texts.
     */
    public class GameplayUIView : BaseUIView
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _victoryText;
        [SerializeField] private TextMeshProUGUI _defeatText;
        
        [Inject] private ROModel _model;
        [Inject] private UserFlowController _userFlowController;

        private CompositeDisposable _disposable = new();
        
        public void OnMainMenuClicked()
        {
            _userFlowController.MainMenuRequested();
        }
        
        private void OnPointsOrLevelChanged()
        {
            _scoreText.text = $"Level: {_model.ROMeta.Level.Value}, Score: {_model.ROMeta.TotalPoints.Value}";
        }
        
        private void OnGameStateChanged(GameplayModel.State state)
        {
            _victoryText.gameObject.SetActive(state == GameplayModel.State.Victory);
            _defeatText.gameObject.SetActive(state == GameplayModel.State.Defeat);
        }

        protected override void OnShown()
        {
            _disposable?.Dispose();
            _disposable = new ();
            
            _model.ROMeta.TotalPoints.Subscribe(OnPointsOrLevelChanged).AddTo(_disposable);
            _model.ROMeta.Level.Subscribe(OnPointsOrLevelChanged).AddTo(_disposable);
            _model.ROGame.GameState.Subscribe(OnGameStateChanged).AddTo(_disposable);
        }

        protected override void OnHidden()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}