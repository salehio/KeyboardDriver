using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;
using System.Threading;
using NAudio.CoreAudioApi.Interfaces;

namespace KeyboardDriver
{
    internal class AudioHandler
    {
        private MMDevice _device;

        public AudioHandler()
        {
            var deviceEnumerator = new MMDeviceEnumerator();
            _device = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);

            // Need to do _something_ with the AudioEndpointVolume, otherwise the calls won't return
            // for some weird reason.
            _device.AudioEndpointVolume.Mute = _device.AudioEndpointVolume.Mute;

            var notificationClient = new NotificationClient();
            notificationClient.SetNotificationHandler(() =>
            {
                _device = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
            });

            deviceEnumerator.RegisterEndpointNotificationCallback(notificationClient);
        }

        public void StepVolumeUp()
        {
            _device.AudioEndpointVolume.VolumeStepUp();
            Logger.WriteDebug($"Volume: {_device.AudioEndpointVolume.MasterVolumeLevelScalar}");
        }

        public void StepVolumeDown()
        {
            _device.AudioEndpointVolume.VolumeStepDown();
            Logger.WriteDebug($"Volume: {_device.AudioEndpointVolume.MasterVolumeLevelScalar}");
        }

        public void ToggleMute()
        {
            _device.AudioEndpointVolume.Mute = !_device.AudioEndpointVolume.Mute;
            Logger.WriteDebug($"Volume: {_device.AudioEndpointVolume.MasterVolumeLevelScalar}");
        }

        private class NotificationClient : IMMNotificationClient
        {
            private Action handler { get; set; }

            public void SetNotificationHandler(Action f)
            {
                handler = f;
            }

            public void OnDefaultDeviceChanged(DataFlow flow, Role role, string defaultDeviceId)
            {
                if (flow == DataFlow.Render && role == Role.Console)
                {
                    handler();
                }
            }

            public void OnDeviceAdded(string pwstrDeviceId)
            {
                return;
            }

            public void OnDeviceRemoved(string deviceId)
            {
                return;
            }

            public void OnDeviceStateChanged(string deviceId, DeviceState newState)
            {
                return;
            }

            public void OnPropertyValueChanged(string pwstrDeviceId, PropertyKey key)
            {
                return;
            }
        }
    }

}
