namespace BirthdayCalendarMVC.Services
{
    public class ImageProcessing
    {
        public static async Task<string?> StoreImage(IFormFile image, IWebHostEnvironment webHostEnvironment)
        {
            if (image != null && image.Length > 0)
            {
                var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "photos");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(image.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                return $"/photos/{fileName}";
            }

            return null;
        }
    }
}
