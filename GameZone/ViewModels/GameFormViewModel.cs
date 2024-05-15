namespace GameZone.ViewModels
{
    public class GameFormViewModel
    {
        [MaxLength(250)]
        public string Name { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public IEnumerable<SelectListItem>? Categories { get; set; }
        [Display(Name = "Supported Devices")]

        public List<int> SelectedDevices { get; set; }
        public IEnumerable<SelectListItem>? Devices { get; set; }

        [MaxLength(2500)]
        public string Description { get; set; }
    }
}
