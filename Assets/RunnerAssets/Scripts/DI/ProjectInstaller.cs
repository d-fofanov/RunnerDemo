using System;
using Controllers;
using Controllers.Gameplay;
using Reflex.Core;
using Settings;
using UnityEngine;
using Utils;
using CharacterController = Controllers.Gameplay.CharacterController;

namespace DI
{
    /**
     * Installs bindings for the main project.
     */
    public class ProjectInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private StaticSettingsHost staticSettingsHost;
        
        public void InstallBindings(ContainerBuilder builder)
        {
            OneTimeInit();

            var time = TimeUtil.Create();
            builder.AddSingleton(time);
            builder.AddSingleton(staticSettingsHost.Settings);
            
            var persistenceController = new PersistenceController(time, staticSettingsHost.Settings);
            builder.AddSingleton(persistenceController);
            builder.AddSingleton(persistenceController.FullModel);

            builder.AddSingleton(container => new UIViewFactory(container));

            builder.AddSingleton(typeof(AssetFactory));
            builder.AddSingleton(typeof(CoinController));
            builder.AddSingleton(typeof(CharacterController));
            builder.AddSingleton(typeof(GameplayController));
        }

        private void OneTimeInit()
        {
            Application.targetFrameRate = 120;
            
            EncryptionUtils.Initialize(Convert.FromBase64String("b5KmvXdVnHGk9hZ9oRXkoMVKRIzz1nid8IiZuejqHAU="));
        }
    }
}
