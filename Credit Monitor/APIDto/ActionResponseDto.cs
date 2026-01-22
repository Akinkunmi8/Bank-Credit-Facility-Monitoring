namespace Credit_Monitor.APIsDto
{
    public class ActionResponseDto
    {
        // Note: $id metadata is handled by the serializer
        public bool success { get; set; }
        public string message { get; set; }
    }
}
