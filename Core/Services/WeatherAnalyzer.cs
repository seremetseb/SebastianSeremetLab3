using System;
using System.Collections.Generic;
using System.Linq;
using TempFukt.Core.Models;

namespace TempFukt.Core.Services
{
    public static class WeatherAnalyzer
    {
       
        public static double? GetDailyAverageTemperature(IEnumerable<Measurement> measurements,LocationType location,DateTime date)
        {
            var day = measurements.Where(m => m.Location == location && m.Timestamp.Date == date.Date);

            if (!day.Any())
                return null;

            return day.Average(m => m.Temperature);
        }

        
        public static IEnumerable<DailyStats> GetDailyStats(IEnumerable<Measurement> measurements,LocationType location)
        {
            return measurements
                .Where(m => m.Location == location)
                .GroupBy(m => m.Timestamp.Date)
                .Select(g =>
                {
                    var avgTemp = g.Average(x => x.Temperature);
                    var avgHum = g.Average(x => x.Humidity);

                    return new DailyStats
                    {
                        Date = g.Key,
                        Location = location,
                        AvgTemp = avgTemp,
                        AvgHumidity = avgHum,
                        MoldRiskIndex = CalculateMoldRisk(avgTemp, avgHum)
                    };
                })
                .OrderBy(s => s.Date);
        }

        
        public static IEnumerable<DailyStats> SortByTempDesc(
            IEnumerable<Measurement> measurements,
            LocationType location)
        {
            return GetDailyStats(measurements, location)
                .OrderByDescending(s => s.AvgTemp);
        }

        public static IEnumerable<DailyStats> SortByHumidityAsc(
            IEnumerable<Measurement> measurements,
            LocationType location)
        {
            return GetDailyStats(measurements, location)
                .OrderBy(s => s.AvgHumidity);
        }

        
        public static IEnumerable<DailyStats> SortByMoldRisk(
            IEnumerable<Measurement> measurements,
            LocationType location)
        {
            return GetDailyStats(measurements, location)
                .OrderBy(s => s.MoldRiskIndex);
        }

        
        public static double CalculateMoldRisk(double temp, double humidity)
        {
            
            var rhFactor = Math.Max(0, humidity - 75);
            var tFactor = Math.Max(0, temp - 5);

            
            return rhFactor * 0.1 + tFactor * 0.2;
        }
    

        public static DateTime? GetMeteorologicalAutumn(IEnumerable<Measurement> measurements)
        {
            var daily = GetDailyStats(measurements, LocationType.Ute)
                .OrderBy(d => d.Date)
                .ToList();

            int streak = 0;
            DateTime? firstInPeriod = null;

            foreach (var day in daily)
            {
                if (day.AvgTemp > 0 && day.AvgTemp < 10)
                {
                    streak++;
                    if (streak == 1)
                        firstInPeriod = day.Date;

                    if (streak >= 5)
                        return firstInPeriod;
                }
                else
                {
                    streak = 0;
                    firstInPeriod = null;
                }
            }

            return null; 
        }

        
        public static DateTime? GetMeteorologicalWinter(IEnumerable<Measurement> measurements)
        {
            var daily = GetDailyStats(measurements, LocationType.Ute)
                .OrderBy(d => d.Date)
                .ToList();

            int streak = 0;
            DateTime? firstInPeriod = null;

            foreach (var day in daily)
            {
                if (day.AvgTemp <= 0)
                {
                    streak++;
                    if (streak == 1)
                        firstInPeriod = day.Date;

                    if (streak >= 5)
                        return firstInPeriod;
                }
                else
                {
                    streak = 0;
                    firstInPeriod = null;
                }
            }

            return null;
        }

    }
}
