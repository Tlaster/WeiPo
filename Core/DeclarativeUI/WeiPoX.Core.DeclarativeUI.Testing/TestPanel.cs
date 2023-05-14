namespace WeiPoX.Core.DeclarativeUI.Testing;

public class TestPanel : TestControl
{
    public List<TestControl> Children { get; } = new();
}