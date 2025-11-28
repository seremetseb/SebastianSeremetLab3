using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TempFukt.Core.Models;

namespace TempFukt.DataAccess
{
    public static class DbInitializer
    {
        private static string CsvPath =>
            Path.Combine(AppContext.BaseDirectory, "TempFuktData.csv");

        public static void Initialize()
        {
            using var db = new AppDbContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            Console.WriteLine("Bas-mapp (AppContext.BaseDirectory):");
            Console.WriteLine(AppContext.BaseDirectory);
            Console.WriteLine();

            if (!File.Exists(CsvPath))
            {
                Console.WriteLine($"Hittar inte CSV-filen: {CsvPath}");
                Console.WriteLine("Filer i den här mappen är:");

                foreach (var file in Directory.GetFiles(AppContext.BaseDirectory))
                {
                    Console.WriteLine("  " + Path.GetFileName(file));
                }

                return;
            }

            Console.WriteLine($"Läser in data från: {CsvPath}");

            var lines = File.ReadAllLines(CsvPath);
            if (lines.Length <= 1)
            {
                Console.WriteLine("CSV-filen verkar vara tom.");
                return;
            }

            var measurements = new List<Measurement>();


            var culture = CultureInfo.InvariantCulture;

            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = TrySplit(line);
                if (parts.Length < 4)
                    continue;

                var dateText = parts[0].Trim();
                var placeText = parts[1].Trim();
                var tempText = parts[2].Trim();
                var humText = parts[3].Trim();

                if (!DateTime.TryParse(dateText, out var timestamp))
                    continue;

                if (!TryMapLocation(placeText, out var location))
                    continue;

                if (!double.TryParse(tempText, NumberStyles.Any, culture, out var temp))
                    continue;

                if (!double.TryParse(humText, NumberStyles.Any, culture, out var humidity))
                    continue;

                var m = new Measurement
                {
                    Timestamp = timestamp,
                    Location = location,
                    Temperature = temp,
                    Humidity = humidity
                };

                measurements.Add(m);
            }

            db.Measurements.AddRange(measurements);
            db.SaveChanges();

            Console.WriteLine($"Importerade {measurements.Count} mätningar till databasen.");
        }

        private static string[] TrySplit(string line)
        {

            var p = line.Split('\t');
            if (p.Length >= 4) return p;


            p = line.Split(';');
            if (p.Length >= 4) return p;

           
            p = line.Split(',');
            return p;
        }

        private static bool TryMapLocation(string placeText, out LocationType location)
        {
            switch (placeText.Trim().ToLowerInvariant())
            {
                case "ute":
                case "utomhus":
                    location = LocationType.Ute;
                    return true;

                case "inne":
                case "inomhus":
                    location = LocationType.Inne;
                    return true;

                case "balkong":
                    location = LocationType.Balkong;
                    return true;

                case "vardagsrum":
                case "vardagsrummet":
                    location = LocationType.Vardagsrum;
                    return true;

                default:
                    location = default;
                    return false;
            }
        }
    }
}
