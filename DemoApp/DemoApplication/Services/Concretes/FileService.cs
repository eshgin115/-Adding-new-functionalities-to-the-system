using DemoApplication.Areas.Client.ViewModels.Home.Index;
using DemoApplication.Contracts.File;
using DemoApplication.Services.Abstracts;

namespace DemoApplication.Services.Concretes
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService>? _logger;

        public FileService(ILogger<FileService>? logger)
        {
            _logger = logger;
        }

        public async Task<List<string>> UploadAsync(List<IFormFile> formFiles, UploadDirectory uploadDirectory)
        {
            string directoryPath = GetUploadDirectory(uploadDirectory);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var imageNamesInSystem = GenerateUniqueFileNames(formFiles);


            for (int i = 0; i < imageNamesInSystem.Count; i++)
            {

                var filePath = Path.Combine(directoryPath, imageNamesInSystem[i]);

                try
                {
                    using FileStream fileStream = new FileStream(filePath, FileMode.Create);
                    await formFiles[i].CopyToAsync(fileStream);
                }
                catch (Exception e)
                {
                    _logger!.LogError(e, "Error occured in file service");

                    throw;
                }

            }
            return imageNamesInSystem;
        }



        public async Task DeleteAsync(List<ImageData>? fileNames, UploadDirectory uploadDirectory)
        {
            foreach (var fileName in fileNames)
            {
                var deletePath = Path.Combine(GetUploadDirectory(uploadDirectory), fileName.ImageName);

                await Task.Run(() => File.Delete(deletePath));
            }
        }





        private List<string> GenerateUniqueFileNames(List<IFormFile> formFiles)
        {
            List<string> uniqueFileName = new List<string>();
            foreach (var fileName in formFiles)
            {
                uniqueFileName.Add($"{Guid.NewGuid()}{Path.GetExtension(fileName.FileName)}");
            };
            return uniqueFileName;
        }

        private string GetUploadDirectory(UploadDirectory uploadDirectory)
        {
            string startPath = Path.Combine("wwwroot", "client", "custom-files");

            switch (uploadDirectory)
            {
                case UploadDirectory.Book:
                    return Path.Combine(startPath, "books");
                default:
                    throw new Exception("Something went wrong");
            }
        }

        public List<string> GetFileUrl(List<ImageData>? fileNames, UploadDirectory uploadDirectory)
        {
            string initialSegment = "client/custom-files/";

            List<string> FileUrls = new List<string>();

            foreach (var fileName in fileNames)
            {
                if (uploadDirectory==UploadDirectory.Book)
                {
                    FileUrls.Add($"{initialSegment}/books/{fileName}");
                }
            }
            return FileUrls;

        }
        public async Task<string> UploadAsync(IFormFile formFile, UploadDirectory uploadDirectory)
        {
            string directoryPath = GetUploadDirectory(uploadDirectory);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var imageNameInSystem = GenerateUniqueFileName(formFile.FileName);
            var filePath = Path.Combine(directoryPath, imageNameInSystem);

            try
            {
                using FileStream fileStream = new FileStream(filePath, FileMode.Create);
                await formFile.CopyToAsync(fileStream);
            }
            catch (Exception e)
            {
                _logger!.LogError(e, "Error occured in file service");

                throw;
            }

            return imageNameInSystem;
        }

        public async Task DeleteAsync(string? fileName, UploadDirectory uploadDirectory)
        {
            var deletePath = Path.Combine(GetUploadDirectory(uploadDirectory), fileName);

            await Task.Run(() => File.Delete(deletePath));
        }

       

        private string GenerateUniqueFileName(string fileName)
        {
            return $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
        }

        public string GetFileUrl(string? fileName, UploadDirectory uploadDirectory)
        {
            string initialSegment = "client/custom-files/";

            switch (uploadDirectory)
            {
                case UploadDirectory.Book:
                    return $"{initialSegment}/books/{fileName}";
                default:
                    throw new Exception("Something went wrong");
            }

        }
    }
}
