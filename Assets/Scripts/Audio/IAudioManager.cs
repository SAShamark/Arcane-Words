using System.Collections.Generic;
using Audio.Data;
using UnityEngine;

namespace Audio
{
    public interface IAudioManager
    {
        void Play(AudioGroupType audioGroupType, string name, Vector3 position = new());
        void MuteSwitcher(AudioGroupType audioGroupType, string name, bool isMute);
        float GetVolume(AudioMixerGroups audioMixerGroups);
        void MasterSwitcher(bool state);
        void ChangeMusicVolume(float volume);
        void ChangeSoundVolume(float volume);
        void SaveVolumeSettings();
    }
}