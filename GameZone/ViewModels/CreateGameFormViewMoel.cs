

namespace GameZone.ViewModels
{
    public class CreateGameFormViewMoel: GameFormViewModel
    {
       
        [AllowedExtensions(FileSettings.AllowedExtensions)
            ,MaxFileSize(FileSettings.MaxFileSizeInBytes)]
        public IFormFile Cover { get; set; }
    }
}
