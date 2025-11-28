using System;
using TempFukt.Core.Models;

namespace TempFukt.Core.Services
{
    public class DailyStats
    {
        public DateTime Date { get; set; }
        public LocationType Location { get; set; }
        public double AvgTemp { get; set; }
        public double AvgHumidity { get; set; }
        public double MoldRiskIndex { get; set; }
    }
}
