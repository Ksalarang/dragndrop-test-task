using DragNDrop.UserInput;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DragNDrop
{
    public class MainLifetimeScope : LifetimeScope
    {
        [Header("Scene objects")]
        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private SpriteRenderer _background;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_camera);

            builder.RegisterEntryPoint<SceneScroller>().WithParameter(_background);

#if UNITY_EDITOR
            builder.Register<DesktopInputHandler>(Lifetime.Singleton).AsImplementedInterfaces();
#else
            builder.Register<MobileInputHandler>(Lifetime.Singleton).AsImplementedInterfaces();
#endif
        }
    }
}