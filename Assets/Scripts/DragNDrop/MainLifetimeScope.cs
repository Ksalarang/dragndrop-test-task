using DragNDrop.Draggables;
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

        [SerializeField]
        private Transform _surfacesParent;

        [SerializeField]
        private Transform _draggablesParent;

        [Header("Configs")]
        [SerializeField]
        private DraggablesConfig _draggablesConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_camera);
            builder.RegisterInstance(_draggablesConfig);

            builder.RegisterEntryPoint<SceneScroller>().WithParameter(_background);
            builder.RegisterEntryPoint<ObjectDragHandler>();

            builder.Register<ObjectDropHandler>(Lifetime.Singleton).AsImplementedInterfaces()
                .WithParameter(_surfacesParent);

            builder.Register<ObjectOrderHandler>(Lifetime.Singleton).AsImplementedInterfaces()
                .WithParameter(_draggablesParent);

#if UNITY_EDITOR
            builder.Register<DesktopInputHandler>(Lifetime.Singleton).AsImplementedInterfaces();
#else
            builder.Register<MobileInputHandler>(Lifetime.Singleton).AsImplementedInterfaces();
#endif
        }
    }
}