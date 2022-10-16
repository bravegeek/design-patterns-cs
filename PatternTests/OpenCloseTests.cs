using OpenClose;
namespace PatternTests;

public class OpenCloseTests
{
    public class FilterLogic
    {
        private readonly Product[] _products;
        private readonly SizeSpecification _smallSpec;
        private readonly ColorSpecification _blueSpec;
        public FilterLogic()
        {
            _products = new Product[] {
                new Product("Bauble", Color.Blue, Size.Small),
                new Product("Tree", Color.Green, Size.Large),
                new Product("House", Color.Blue, Size.Large)
            };
            _smallSpec = new SizeSpecification(Size.Small);
            _blueSpec = new ColorSpecification(Color.Blue);
        }

        [Fact]
        public void FilterReturnsCorrectProducts()
        {
            var sut = new FilterProducts();
            var filteredProducts = sut.Filter(_products, _blueSpec);

            Assert.Equal(2, filteredProducts.Count());
        }
    }

    public class SpecLogic
    {
        private readonly Product _product;
        public SpecLogic()
        {
            _product = new Product("test", Color.Blue, Size.Small);
        }

        [Fact]
        public void ColorSpec_IsBlue()
        {
            var sut = new ColorSpecification(Color.Blue);
            Assert.True(sut.IsSatisfied(_product));
        }

        [Fact]
        public void ColorSpec_IsNotSatified()
        {
            var sut = new ColorSpecification(Color.Red);
            Assert.False(sut.IsSatisfied(_product));
        }

        [Theory]
        [InlineData(Color.Red)]
        [InlineData(Color.Green)]
        [InlineData(Color.Blue)]
        public void ColorSpec_All(Color color)
        {
            _product.Color = color;
            var sut = new ColorSpecification(color);
            Assert.True(sut.IsSatisfied(_product));
        }

        [Theory]
        [InlineData(Size.Small)]
        [InlineData(Size.Medium)]
        [InlineData(Size.Large)]
        public void SizeSpec_All(Size size)
        {
            _product.Size = size;
            var sut = new SizeSpecification(size);
            Assert.True(sut.IsSatisfied(_product));
        }

        [Fact]
        public void AndSpecification_SizeColor_BothSatisfied()
        {
            var sizeSpec = new SizeSpecification(Size.Small);
            var colorSpec = new ColorSpecification(Color.Blue);

            var sut = new AndSpecification(
                new ISpecification<Product>[] {sizeSpec, colorSpec}
            );

            Assert.True(sut.IsSatisfied(_product));
        }

        [Theory]
        [InlineData(Size.Small, Color.Green, false)] //TF
        [InlineData(Size.Large, Color.Blue, false)] //FT
        [InlineData(Size.Medium, Color.Red, false)] //FF
        [InlineData(Size.Small, Color.Blue, true)] //TT
        public void AndSpecification_All(Size size, Color color, bool pass)
        {
            var sizeSpec = new SizeSpecification(size);
            var colorSpec = new ColorSpecification(color);

            var sut = new AndSpecification(
                new ISpecification<Product>[] {sizeSpec, colorSpec}
            );

            Assert.Equal(pass, sut.IsSatisfied(_product));
        }

        [Theory]
        [InlineData(Size.Small, Color.Green, true)] //TF
        [InlineData(Size.Large, Color.Blue, true)] //FT
        [InlineData(Size.Medium, Color.Red, false)] //FF
        [InlineData(Size.Small, Color.Blue, true)] //TT
        public void OrSpecification_All(Size size, Color color, bool pass)
        {
            var sizeSpec = new SizeSpecification(size);
            var colorSpec = new ColorSpecification(color);

            var sut = new OrSpecification(
                new ISpecification<Product>[] {sizeSpec, colorSpec}
            );

            Assert.Equal(pass, sut.IsSatisfied(_product));
        }
    }
}
