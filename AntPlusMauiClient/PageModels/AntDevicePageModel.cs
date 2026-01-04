using AntPlusMauiClient.ViewModels;
using AntPlusMauiClient.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using SmallEarthTech.AntPlus;
using SmallEarthTech.AntPlus.DeviceProfiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntPlusMauiClient.PageModels
{
    public partial class AntDevicePageModel : ObservableObject, IQueryAttributable
    {
        [ObservableProperty]
        public partial AntDevice? Device { get; set; }

        [ObservableProperty]
        public partial ContentView? AntDeviceView { get; set; }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Device = (AntDevice)query["AntDevice"];

            // switch on device type to create appropriate view
            switch (Device)
            {
                case UnknownDevice unknownDevice:
                    AntDeviceView = new UnknownDeviceView(new UnknownDeviceViewModel(unknownDevice));
                    break;
                default:
                    break;
            }    
        }
    }
}
