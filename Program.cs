using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

class NobelPrize
{
    public int Year { get; set; }
    public string Category { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public override string ToString()
    {
        return $"{Year};{Category};{FirstName};{LastName}";
    }
}

class Program
{
    static void Main(string[] args)
    {
        var nobelPrizes = File.ReadAllLines("nobel.csv")
            .Skip(1)
            .Select(line =>
            {
                var parts = line.Split(';');
                return new NobelPrize
                {
                    Year = int.Parse(parts[0]),
                    Category = parts[1],
                    FirstName = parts[2],
                    LastName = parts[3]
                };
            })
            .ToList();

        Console.WriteLine("3. feladat: " + GetCategory("Arthur B.", "McDonald", nobelPrizes));

        NobelPrize y2017Literature = nobelPrizes.FirstOrDefault(np => np.Year == 2017 && np.Category == "irodalmi");
        if (y2017Literature != null)
        {
            Console.WriteLine($"4. feladat: {y2017Literature.FirstName} {y2017Literature.LastName}");
        }

        var organizations90s = nobelPrizes.Where(np => np.LastName == "" && np.Year >= 1990);
        Console.WriteLine("5. feladat:");
        foreach (var org in organizations90s)
        {
            Console.WriteLine($"        {org.Year}: {org.FirstName}");
        }

        var curieFamily = nobelPrizes.Where(np => np.LastName.Contains("Curie"));
        Console.WriteLine("6. feladat:");
        foreach (var curie in curieFamily)
        {
            Console.WriteLine($"        {curie.Year}: {curie.FirstName} {curie.LastName} ({curie.Category})");
        }

        var prizeCounts = nobelPrizes.GroupBy(np => np.Category).Select(group => new
        {
            Category = group.Key,
            Count = group.Count()
        });
        Console.WriteLine("7. feladat:");
        foreach (var count in prizeCounts)
        {
            Console.WriteLine($"        {count.Category} {count.Count} db");
        }

        var medicalPrizes = nobelPrizes.Where(np => np.Category == "orvosi").OrderBy(np => np.Year);
        Console.WriteLine("8. feladat: orvosi.txt");
        using (StreamWriter writer = new StreamWriter("orvosi.txt"))
        {
            foreach (var prize in medicalPrizes)
            {
                writer.WriteLine($"{prize.Year}:{prize.FirstName} {prize.LastName}");
            }
        }
    }

    static string GetCategory(string firstName, string lastName, List<NobelPrize> prizes)
    {
        var prize = prizes.FirstOrDefault(np => np.FirstName == firstName && np.LastName == lastName);
        return prize != null ? prize.Category : "Nincs ilyen adat!";
    }
}
