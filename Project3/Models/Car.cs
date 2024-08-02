namespace Project3.Models
{
    public class Car
    {
        public string Name { get; set; } = string.Empty; // Initialize with a default value
        public int Code { get; set; }
        public string Codename { get; set; } = string.Empty; // Initialize with a default value
        public List<Model> Models { get; set; } = new List<Model>(); // Initialize with a default value
    }

    public class Model
    {
        public string Name { get; set; } = string.Empty; // Initialize with a default value
        public int Code { get; set; }
        public string Codename { get; set; } = string.Empty; // Initialize with a default value
        public int Year { get; set; }
        public string Description { get; set; } = string.Empty; // Initialize with a default value
        public string Price { get; set; } = string.Empty; // Initialize with a default value
        public string Image { get; set; } = string.Empty; // Initialize with a default value
        public List<Trim> Trims { get; set; } = new List<Trim>(); // Initialize with a default value
    }

    public class Trim
    {
        public string Name { get; set; } = string.Empty; // Initialize with a default value
        public int Code { get; set; }
        public string Codename { get; set; } = string.Empty; // Initialize with a default value
    }
}
