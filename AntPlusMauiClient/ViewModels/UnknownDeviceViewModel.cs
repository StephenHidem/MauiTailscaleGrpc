using SmallEarthTech.AntPlus.DeviceProfiles;
using System.Collections.ObjectModel;

namespace AntPlusMauiClient.ViewModels
{
    public class UnknownDeviceViewModel
    {
        private UnknownDevice _unknownDevice;
        public ObservableCollection<string> DataPages { get; private set; } = new ObservableCollection<string>();

        public UnknownDeviceViewModel(UnknownDevice unknownDevice)
        {
            _unknownDevice = unknownDevice;
            foreach (byte[] page in _unknownDevice.DataPages)
            {
                DataPages.Add(BitConverter.ToString(page));
            }

            _unknownDevice.DataPages.CollectionChanged += (s, e) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    DataPages.Clear();
                    foreach (byte[] page in _unknownDevice.DataPages)
                    {
                        DataPages.Add(BitConverter.ToString(page));
                    }
                });
            };
        }
    }
}
