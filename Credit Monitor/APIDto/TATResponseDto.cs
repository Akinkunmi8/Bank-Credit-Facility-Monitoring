namespace Credit_Monitor.APIsDto
{
    public class TATResponseDto
    {
        // $id is metadata handled by the serializer

        public bool isSuccessful { get; set; }

        // In your JSON, this is a string "0"
        public string tatCount { get; set; }
    }
}
