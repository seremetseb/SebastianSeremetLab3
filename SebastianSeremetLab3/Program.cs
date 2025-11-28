using System;
using System.Linq;
using TempFukt.Core.Models;
using TempFukt.Core.Services;
using TempFukt.DataAccess;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;


        DbInitializer.Initialize();

        using var db = new AppDbContext();

        Console.WriteLine($"Antal mätningar i databasen: {db.Measurements.Count()}");
        Console.WriteLine();

   
        var datum = new DateTime(2016, 10, 1); 
        var uteMedel = WeatherAnalyzer.GetDailyAverageTemperature(
            db.Measurements,
            LocationType.Ute,
            datum);

        Console.WriteLine($"[UTE] Medeltemperatur {datum:yyyy-MM-dd}: " +
                          (uteMedel.HasValue ? $"{uteMedel.Value:F1} °C" : "ingen data"));

      
        var inneMedel = WeatherAnalyzer.GetDailyAverageTemperature(
            db.Measurements,
            LocationType.Inne,
            datum);

        Console.WriteLine($"[INNE] Medeltemperatur {datum:yyyy-MM-dd}: " +
                          (inneMedel.HasValue ? $"{inneMedel.Value:F1} °C" : "ingen data"));

        Console.WriteLine();

      
        Console.WriteLine("UTE – varmaste 10 dagarna (medeltemp per dag):");
        foreach (var s in WeatherAnalyzer.SortByTempDesc(db.Measurements, LocationType.Ute).Take(10))
        {
            Console.WriteLine($"{s.Date:yyyy-MM-dd}  {s.AvgTemp,5:F1} °C  RH: {s.AvgHumidity,3:F0}%  Mögelindex: {s.MoldRiskIndex,4:F1}");
        }

        Console.WriteLine();


        Console.WriteLine("UTE – torraste 10 dagarna (medelluftfuktighet per dag):");
        foreach (var s in WeatherAnalyzer.SortByHumidityAsc(db.Measurements, LocationType.Ute).Take(10))
        {
            Console.WriteLine($"{s.Date:yyyy-MM-dd}  RH: {s.AvgHumidity,3:F0}%  Temp: {s.AvgTemp,5:F1} °C  Mögelindex: {s.MoldRiskIndex,4:F1}");
        }

        Console.WriteLine();


        Console.WriteLine("UTE – minst → störst risk för mögel (10 dagar):");
        foreach (var s in WeatherAnalyzer.SortByMoldRisk(db.Measurements, LocationType.Ute).Take(10))
        {
            Console.WriteLine($"{s.Date:yyyy-MM-dd}  Mögelindex: {s.MoldRiskIndex,4:F1}  Temp: {s.AvgTemp,5:F1} °C  RH: {s.AvgHumidity,3:F0}%");
        }

        Console.WriteLine();


        Console.WriteLine("INNE – varmaste 10 dagarna (medeltemp per dag):");
        foreach (var s in WeatherAnalyzer.SortByTempDesc(db.Measurements, LocationType.Inne).Take(10))
        {
            Console.WriteLine($"{s.Date:yyyy-MM-dd}  {s.AvgTemp,5:F1} °C  RH: {s.AvgHumidity,3:F0}%  Mögelindex: {s.MoldRiskIndex,4:F1}");
        }

        Console.WriteLine();


        var host = WeatherAnalyzer.GetMeteorologicalAutumn(db.Measurements);
        var vinter = WeatherAnalyzer.GetMeteorologicalWinter(db.Measurements);

        if (host.HasValue)
            Console.WriteLine($"Meteorologisk höst (ute): {host.Value:yyyy-MM-dd}");
        else
            Console.WriteLine("Meteorologisk höst kunde inte bestämmas enligt vald regel.");

        if (vinter.HasValue)
            Console.WriteLine($"Meteorologisk vinter (ute): {vinter.Value:yyyy-MM-dd}");
        else
            Console.WriteLine("Meteorologisk vinter kunde inte bestämmas (mild vinter?).");

        Console.WriteLine();
        Console.WriteLine("Klart. Tryck valfri tangent för att avsluta.");
        Console.ReadKey();
    }
}
