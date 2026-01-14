using SmallEarthTech.AntPlus.DeviceProfiles;
using System.Collections.ObjectModel;

namespace AntPlusMauiClient.ViewModels
{
    public class UnknownDeviceViewModel
    {
        public ObservableCollection<byte[]> DataPages { get; private set; } = [];

        public UnknownDeviceViewModel(UnknownDevice unknownDevice)
        {
            DataPages = unknownDevice.DataPages;
        }
    }
}
