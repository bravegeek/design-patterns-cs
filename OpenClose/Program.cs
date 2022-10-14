﻿// See https://aka.ms/new-console-template for more information

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
var manyAndSpecs = new ISpecification<Product>[] { new ColorSpecification(Color.Blue), new SizeSpecification(Size.Small)};
var manyAndFilterProducts = new FilterProducts().Filter(products, new AndSpecification(manyAndSpecs));
Array.ForEach(manyAndFilterProducts.ToArray(), Console.WriteLine);
Console.WriteLine();

Console.WriteLine("Large OR Green Products");
var manyOrSpecs = new ISpecification<Product>[] { new ColorSpecification(Color.Green), new SizeSpecification(Size.Large)};
var manyOrFilterProducts = new FilterProducts().Filter(products, new OrSpecification(manyOrSpecs));
Array.ForEach(manyOrFilterProducts.ToArray(), Console.WriteLine);
Console.WriteLine();

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

    public bool IsSatisfied(Product product)
    {
        return product.Color == color;
    }
}

public class SizeSpecification : ISpecification<Product>
{
    private readonly Size size;
    public SizeSpecification(Size size)
    {
        this.size = size;
    }

    public bool IsSatisfied(Product product)
    {
        return product.Size == size;
    }
}

public class AndSpecification : ISpecification<Product>
{
    private readonly ISpecification<Product>[] specifications;
    public AndSpecification(ISpecification<Product>[] specifications)
    {
        this.specifications = specifications;
    }

    public bool IsSatisfied(Product product)
    {
        // All specifications must be true for AND
        return specifications.All(spec => spec.IsSatisfied(product));
    }
}

public class OrSpecification : ISpecification<Product>
{
    private readonly ISpecification<Product>[] specifications;
    public OrSpecification(ISpecification<Product>[] specifications)
    {
        this.specifications = specifications;
    }

    public bool IsSatisfied(Product product)
    {
        // Any specification can be true for OR
        return specifications.Any(spec => spec.IsSatisfied(product));
    }
}

public class FilterProducts : IFilter<Product>
{
    public IEnumerable<Product> Filter(IEnumerable<Product> products, ISpecification<Product> spec)
    {
        return products.Where(product => spec.IsSatisfied(product));
        
        // foreach (var product in products)
        //     if (spec.IsSatisfied(product))
        //         yield return product;
    }
}