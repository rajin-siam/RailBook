namespace RailBook.ApiModels
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }           // Was the operation successful?
        public string Message { get; set; }         // Human-readable message
        public T? Data { get; set; }                // The actual payload (your DTO)
        public List<string>? Errors { get; set; }   // Error messages (if any)
        public DateTime Timestamp { get; set; }     // When response was generated
        public int StatusCode { get; set; }         // HTTP status code
    }
}
