namespace WeiPoX.Core.DeclarativeUI.Testing;

internal class TestPanel : TestControl
{
    public List<TestControl> Children { get; } = new();
}