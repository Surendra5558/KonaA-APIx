namespace KonaAI.Master.API
{
    /// <summary>
    /// Represents a sample weather forecast item used for demonstration and testing.
    /// </summary>
    public class WeatherForecast
    {
        /// <summary>
        /// Gets or sets the forecast date.
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Gets or sets the temperature in degrees Celsius.
        /// </summary>
        public int TemperatureC { get; set; }

        /// <summary>
        /// Gets the temperature in degrees Fahrenheit.
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        /// <summary>
        /// Gets or sets a textual summary of the weather conditions.
        /// </summary>
        public string? Summary { get; set; }
    }
}