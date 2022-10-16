namespace OpenClose;
// See https://aka.ms/new-console-template for more information

public class Program
{
    static void Main(string[] args)
    {
        var products = new Product[] {
            new Product("Bauble", Color.Blue, Size.Small),
            new Product("Tree", Color.Green, Size.Large),
            new Product("House", Color.Blue, Size.Large)
        };

        Console.WriteLine("All Products");
        Array.ForEach(products, Console.WriteLine);
        Console.WriteLine();

        Console.WriteLine("Blue Products");
        var filteredProducts = new FilterProducts().Filter(products, new ColorSpecification(Color.Blue));
        Array.ForEach(filteredProducts.ToArray(), Console.WriteLine);
        Console.WriteLine();

        Console.WriteLine("Small AND Blue Products");
        // create array of specifications
        var manyAndSpecs = new ISpecification<Product>[] { new ColorSpecification(Color.Blue), new SizeSpecification(Size.Small)};
        // AND specification
        var andSpec = new AndSpecification(manyAndSpecs);
        // Filter products w AND spec
        var manyAndFilterProducts = new FilterProducts().Filter(products, andSpec);
        Array.ForEach(manyAndFilterProducts.ToArray(), Console.WriteLine);
        Console.WriteLine();

        Console.WriteLine("Large OR Green Products");
        var manyOrSpecs = new ISpecification<Product>[] { new ColorSpecification(Color.Green), new SizeSpecification(Size.Large)};
        var manyOrFilterProducts = new FilterProducts().Filter(products, new OrSpecification(manyOrSpecs));
        Array.ForEach(manyOrFilterProducts.ToArray(), Console.WriteLine);
        Console.WriteLine();
    }
}
    // public void ColorSpec()
    // {
    //     var product = new Product("test", Color.Red, Size.Small);
    //     var spec = new ColorSpecification(Color.Blue);
    //     var satisfied = spec.IsSatisfied(product);
    //     Assert.Equal(true, satisfied);
    // }

public enum Color {
    Red, Green, Blue
}

public enum Size {
    Small, Medium, Large
}

public class Product {
    public string Name { get; set; }
    public Color Color { get; set; }
    public Size Size { get; set; }

    public Product(string name, Color color, Size size)
    {
        Name = name;
        Color = color;
        Size = size;
    }

    public override string ToString()
    {
        return $"{Name}:  Size-{Size}, Color-{Color}";
    }
}

public interface ISpecification<T>
{
    bool IsSatisfied(T t);
}

public interface IFilter<T>
{
    IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
}

public class ColorSpecification : ISpecification<Product>
{
    private readonly Color color;
    public ColorSpecification(Color color)
    {
        this.color = color;
    }

    public bool IsSatisfied(Product item)
    {
        return item.Color == color;
    }
}

public class SizeSpecification : ISpecification<Product>
{
    private readonly Size size;
    public SizeSpecification(Size size)
    {
        this.size = size;
    }

    public bool IsSatisfied(Product item)
    {
        return item.Size == size;
    }
}

public class AndSpecification : ISpecification<Product>
{
    private readonly ISpecification<Product>[] specifications;
    public AndSpecification(ISpecification<Product>[] specifications)
    {
        this.specifications = specifications;
    }

    public bool IsSatisfied(Product item)
    {
        // All specifications must be true for AND
        return specifications.All(spec => spec.IsSatisfied(item));
    }
}

public class OrSpecification : ISpecification<Product>
{
    private readonly ISpecification<Product>[] specifications;
    public OrSpecification(ISpecification<Product>[] specifications)
    {
        this.specifications = specifications;
    }

    public bool IsSatisfied(Product item)
    {
        // Any specification can be true for OR
        return specifications.Any(spec => spec.IsSatisfied(item));
    }
}

public class FilterProducts : IFilter<Product>
{
    public IEnumerable<Product> Filter(IEnumerable<Product> products, ISpecification<Product> spec)
    {
        return products.Where(product => spec.IsSatisfied(product));

        // alternate implementation
        // 
        // foreach (var product in products)
        //     if (spec.IsSatisfied(product))
        //         yield return product;
    }
}