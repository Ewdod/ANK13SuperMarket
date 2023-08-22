namespace ANK13SuperMarket.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public bool IsInStock { get; set; }

        public string ImageName { get; set; } = null!;


    }
}
