namespace Credit_Monitor.Interfaces
{
    public interface IBackendAPIService
    {
        public Task<T> GetAsync<T>(string endpoint);
        public Task<T> PostAsync<T>(object payload, string endpoint);
        public Task<T> PutAsync<T>(object payload, string endpoint);

        public Task<T> PatchAsync<T>(object payload, string endpoint);
        public Task<T> DeleteAsync<T>(string endpoint);

    }
}
