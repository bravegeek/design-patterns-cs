using OpenClose;
namespace PatternTests;

public class OpenCloseTests
{
    private readonly Product[] _products;

    public OpenCloseTests()
    {
        _products = new Product[] {
            new Product("Bauble", Color.Blue, Size.Small),
            new Product("Tree", Color.Green, Size.Large),
            new Product("House", Color.Blue, Size.Large)
        };
    }

    [Fact]
    public void ColorSpec_IsBlue()
    {
        var product = new Product("test", Color.Blue, Size.Small);
        var sut = new ColorSpecification(Color.Blue);
        Assert.True(sut.IsSatisfied(product));
    }

    [Fact]
    public void ColorSpec_IsNotSatified()
    {
        var product = new Product("test", Color.Blue, Size.Small);
        var sut = new ColorSpecification(Color.Red);
        Assert.False(sut.IsSatisfied(product));
    }

    [Theory]
    [InlineData(Color.Red)]
    [InlineData(Color.Green)]
    [InlineData(Color.Blue)]
    public void ColorSpec_All(Color color)
    {
        var product = new Product("test", color, Size.Small);
        var sut = new ColorSpecification(color);
        Assert.True(sut.IsSatisfied(product));
    }

    [Theory]
    [InlineData(Size.Small)]
    [InlineData(Size.Medium)]
    [InlineData(Size.Large)]
    public void SizeSpec_All(Size size)
    {
        var product = new Product("test", Color.Red, size);
        var sut = new SizeSpecification(size);
        Assert.True(sut.IsSatisfied(product));
    }

    [Fact]
    public void AndSpecification_SizeColor_BothSatisfied()
    {
        var product = new Product("test", Color.Blue, Size.Small);
        var sizeSpec = new SizeSpecification(Size.Small);
        var colorSpec = new ColorSpecification(Color.Blue);

        var sut = new AndSpecification(
            new ISpecification<Product>[] {sizeSpec, colorSpec}
        );

        Assert.True(sut.IsSatisfied(product));
    }

    [Theory]
    [InlineData(Size.Small, Color.Green, false)] //TF
    [InlineData(Size.Large, Color.Red, false)] //FT
    [InlineData(Size.Medium, Color.Blue, false)] //FF
    [InlineData(Size.Small, Color.Red, true)] //TT
    public void AndSpecification_All(Size size, Color color, bool pass)
    {
        var product = new Product("test", Color.Red, Size.Small);

        var sizeSpec = new SizeSpecification(size);
        var colorSpec = new ColorSpecification(color);

        var sut = new AndSpecification(
            new ISpecification<Product>[] {sizeSpec, colorSpec}
        );

        Assert.Equal(pass, sut.IsSatisfied(product));
    }

    [Theory]
    [InlineData(Size.Small, Color.Green, true)] //TF
    [InlineData(Size.Large, Color.Red, true)] //FT
    [InlineData(Size.Medium, Color.Blue, false)] //FF
    [InlineData(Size.Small, Color.Red, true)] //TT
    public void OrSpecification_All(Size size, Color color, bool pass)
    {
        var product = new Product("test", Color.Red, Size.Small);

        var sizeSpec = new SizeSpecification(size);
        var colorSpec = new ColorSpecification(color);

        var sut = new OrSpecification(
            new ISpecification<Product>[] {sizeSpec, colorSpec}
        );

        Assert.Equal(pass, sut.IsSatisfied(product));
    }
}
