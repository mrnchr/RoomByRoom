namespace Infrastructure.SceneLoading
{
    public class SceneLoadingModel
    {
        public ChangedProperty<float> LoadingProgress { get; } = new ChangedProperty<float>();
    }
}