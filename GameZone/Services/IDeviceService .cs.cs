namespace GameZone.Services
{
    public interface IDeviceService
    {
        IEnumerable<SelectListItem> GetDevices();
    }
}
