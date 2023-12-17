namespace NewShore.Api.FunctionalTests.Models
{
    public class HealthCheckReadyModel
    {
        public string Status { get; set; }
        public Results Results { get; set; }
    }

    public class Results
    {
        public ResultDetail Npgsql { get; set; }
        public ResultDetail Wan { get; set; }
    }

    public class ResultDetail
    {
        public string Status { get; set; }
        public object Description { get; set; }
        public Data Data { get; set; }
    }

    public class Data
    {
    }
}