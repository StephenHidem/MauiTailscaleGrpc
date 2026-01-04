using SmallEarthTech.AntPlus.DeviceProfiles;
using System.Collections.ObjectModel;

namespace AntPlusMauiClient.ViewModels
{
    public class UnknownDeviceViewModel
    {
        private UnknownDevice _unknownDevice;
        public ObservableCollection<string> DataPages { get; } = [];

        public UnknownDeviceViewModel(UnknownDevice unknownDevice)
        {
            _unknownDevice = unknownDevice;
            _unknownDevice.DataPages.CollectionChanged += (s, e) =>
            {
                DataPages.Clear();
                foreach (byte[] page in _unknownDevice.DataPages)
                {
                    DataPages.Add(BitConverter.ToString(page));
                }
            };
        }
    }
}
