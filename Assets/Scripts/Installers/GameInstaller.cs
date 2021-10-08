using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IGridSearch>()
            .To<GridSearch>()
            .AsSingle();

        Container.Bind<ITilePlacementRuleProcessor>()
            .To<TilePlacementRuleProcessor>()
            .AsSingle();
    }
}
