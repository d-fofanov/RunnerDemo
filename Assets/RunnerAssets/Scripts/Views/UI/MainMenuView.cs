using Controllers;
using Reflex.Attributes;

namespace Views.UI
{
    /**
     * Main menu view.
     * Contains only the Play button.
     */
    public class MainMenuView : BaseUIView
    {
        [Inject] private UserFlowController _userFlowController;

        public void OnPlayClicked()
        {
            _userFlowController.GameplayRequested();
        }

        protected override void OnShown()
        {
        }

        protected override void OnHidden()
        {
        }
    }
}