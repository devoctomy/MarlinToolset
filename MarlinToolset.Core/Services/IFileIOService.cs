namespace MarlinToolset.Core.Services
{
    public interface IFileIOService
    {
        string ReadAllText(string path);
        void WriteAllText(
            string path,
            string contents);

        bool Exists(string path);
    }
}
