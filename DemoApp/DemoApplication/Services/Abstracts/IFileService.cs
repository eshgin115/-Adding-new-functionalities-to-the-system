using DemoApplication.Areas.Client.ViewModels.Home.Index;
using DemoApplication.Contracts.File;

namespace DemoApplication.Services.Abstracts
{
    public interface IFileService
    {
         Task<List<string>> UploadAsync(List<IFormFile> formFiles, UploadDirectory uploadDirectory);
         Task DeleteAsync(List<ImageData>? fileNames, UploadDirectory uploadDirectory);
        List<string> GetFileUrl(List<ImageData>? fileNames, UploadDirectory uploadDirectory);
        Task<string> UploadAsync(IFormFile formFile, UploadDirectory uploadDirectory);
        string GetFileUrl(string? fileName, UploadDirectory uploadDirectory);
        Task DeleteAsync(string? fileName, UploadDirectory uploadDirectory);
    }
}
