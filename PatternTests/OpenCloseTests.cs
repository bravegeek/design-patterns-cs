using OpenClose;
namespace PatternTests;

public class OpenCloseTests
{
    [Fact]
    public void Test1()
    {
        Assert.Equal(1,1);
    }

    [Fact]
    public void ColorSpec()
    {
        var product = new Product("test", Color.Blue, Size.Small);
        var spec = new ColorSpecification(Color.Blue);
        var satisfied = spec.IsSatisfied(product);
        Assert.True(satisfied);
    }
}
