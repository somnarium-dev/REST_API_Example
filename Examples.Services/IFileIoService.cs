namespace Examples.Services
{
    public interface IFileIoService
    {
        void Copy(string sourcePath, string destinationPath, bool overwrite = true);
        void Delete(string filePath);
        void Move(string sourcePath, string destinationPath, bool overwrite = true);
        bool Exists(string filePath);
    }
}
