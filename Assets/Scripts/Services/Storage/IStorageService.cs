namespace Services.Storage
{
    public interface IStorageService
    {
        void SaveData<T>(string key, T data);
        T LoadData<T>(string key, T defaultValue);
    }
}