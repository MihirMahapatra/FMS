using Microsoft.AspNetCore.Http;

namespace FMS.Utility
{
    public static class PictureStorage
    {
        public static String UploadPhoto(IFormFile model, string storageLocation)
        {
            string folderPath = storageLocation;
            folderPath += Guid.NewGuid().ToString() + "_" + model.FileName;
            return folderPath;
        }
    }
}
