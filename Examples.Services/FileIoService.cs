namespace Examples.Services
{
    public class FileIoService : IFileIoService
    {
        public void Copy(string sourcePath, string destinationPath, bool overwrite = true)
        {
            File.Copy(sourcePath, destinationPath, overwrite);
        }

        public void Move(string sourcePath, string destinationPath, bool overwrite = true)
        {
            File.Move(sourcePath, destinationPath, overwrite);
        }

        public void Delete(string filePath)
        {
            File.Delete(filePath);
        }

        public bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
