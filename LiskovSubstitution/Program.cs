namespace LiskovSubstitution;

public class Program
{
    static void Main(string[] args)
    {
        static int Area(Rectangle rect) => rect.Width * rect.Height;

        Rectangle rect = new Rectangle(4,5);
        Console.WriteLine($"{rect.ToString()}, Area: {Area(rect)}");
        
        Rectangle sq = new Square();
        sq.Width = 2;
        Console.WriteLine($"{sq.ToString()}, Area: {Area(sq)}");
        
    }
}

public class Rectangle
{
    // make these properties virtual so we can override them later
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }

    public Rectangle() {}
    public Rectangle(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public override string ToString()
    {
        return $"Width: {Width}, Height: {Height}";
    }
}

public class Square : Rectangle
{
    public override int Width 
    {
        set { base.Width = base.Height = value; }
    }
    public override int Height 
    { 
        set { base.Width = base.Height = value; }
    }

    public Square() {}
//     public Square(int width, int height)
//     {
//         Width = width;
//         Height = height;
//     }
}