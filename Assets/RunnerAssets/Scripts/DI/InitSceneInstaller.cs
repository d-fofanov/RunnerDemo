using Controllers;
using Reflex.Core;
using Reflex.Injectors;
using UnityEngine;
using Utils;
using Views.Gameplay;
using Views.UI;

namespace DI
{
    /**
     * Installs bindings for the initial scene.
     */
    public class InitSceneInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private Canvas mainCanvas;
        [SerializeField] private Canvas overlayCanvas;
        [SerializeField] private GCameraView cameraView;
        [SerializeField] private LoadingScreenView loadingScreenInstance;
        
        public void InstallBindings(ContainerBuilder builder)
        {
            builder.OnContainerBuilt += OnContainerBuilt;

            builder.AddSingleton(cameraView);
            builder.AddSingleton(container =>
            {
                var uiController = new UIController(mainCanvas, overlayCanvas, loadingScreenInstance);
                AttributeInjector.Inject(uiController, container);
                return uiController;
            });
            
            builder.AddSingleton(typeof(SceneController));
            builder.AddSingleton(typeof(UserFlowController));
        }
        
        private void OnContainerBuilt(Container container)
        {
            var timeUtil = container.Resolve<TimeUtil>();
            var userFlowController = container.Resolve<UserFlowController>();
            
            timeUtil.RunNextFrame(userFlowController.InitializeFlow);
        }
    }
}