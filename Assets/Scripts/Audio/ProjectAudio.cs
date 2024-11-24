using System;
using System.Collections.Generic;
using Audio.Data;
using Services;
using Services.Storage;
using UnityEngine;
using Zenject;

namespace Audio
{
    public class ProjectAudio : MonoBehaviour, IAudioManager
    {
        [SerializeField]
        private AudioConfig _audioConfig;

        [SerializeField]
        private AudioContainer _audioContainer;

        private readonly Dictionary<AudioGroupType, AudioContainer> _audioGroupContainer = new();
        private Dictionary<AudioMixerGroups, float> _volumeSettings = new();
        private IStorageService _storageService;

        [Inject]
        private void Construct(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public void Start()
        {
            LoadVolumeSettings();
            InitializeAudioGroups();
            ApplySavedVolumeSettings();
        }

        private void LoadVolumeSettings()
        {
            _volumeSettings =
                _storageService.LoadData(StorageConstants.AUDIO_VOLUMES, new Dictionary<AudioMixerGroups, float>());
        }

        private void InitializeAudioGroups()
        {
            foreach (AudioGroup audioGroup in _audioConfig.AudioGroups)
            {
                AudioContainer audioContainer = Instantiate(_audioContainer, transform);
                audioContainer.Initialize(audioGroup.GroupType, audioGroup.AudioMixerGroup,
                    audioGroup.IsOneAudioSource);
                _audioGroupContainer.Add(audioGroup.GroupType, audioContainer);
            }
        }

        private void ApplySavedVolumeSettings()
        {
            bool isTurnOnMaster = Math.Abs(GetVolume(AudioMixerGroups.Master) - 1) < ValueConstants.EPSILON;
            MasterSwitcher(isTurnOnMaster);

            ChangeMusicVolume(GetVolume(AudioMixerGroups.Music));
            ChangeSoundVolume(GetVolume(AudioMixerGroups.Sounds));
        }

        public void Play(AudioGroupType audioGroupType, string name, Vector3 position = new())
        {
            AudioGroup audioGroup = _audioConfig.FindGroup(audioGroupType);
            AudioInfo audioInfo = audioGroup.FindAudioInfo(name);

            AudioSourceModel audioSourceModel = _audioGroupContainer[audioGroupType].GetAudioSourceModel(name);

            audioSourceModel.Source.clip = audioInfo.Clip;
            audioSourceModel.Source.volume = audioInfo.Volume;
            audioSourceModel.Source.loop = audioInfo.Loop;
            audioSourceModel.Source.Play();

            if (position != Vector3.zero)
            {
                audioSourceModel.transform.position = position;
            }
        }

        public void MuteSwitcher(AudioGroupType audioGroupType, string name, bool isMute)
        {
            AudioSourceModel audioSourceModel = _audioGroupContainer[audioGroupType].GetAudioSourceModel(name);

            if (audioSourceModel.Source.clip == null)
            {
                AudioGroup audioGroup = _audioConfig.FindGroup(audioGroupType);
                AudioInfo audioInfo = audioGroup.FindAudioInfo(name);
                audioSourceModel.Source.clip = audioInfo.Clip;
                audioSourceModel.Source.Play();
            }

            audioSourceModel.Source.mute = isMute;
        }

        public float GetVolume(AudioMixerGroups audioMixerGroup) =>
            _volumeSettings.GetValueOrDefault(audioMixerGroup, 1f);

        public void MasterSwitcher(bool state)
        {
            float volume = state ? 1 : 0;
            ChangeVolume(AudioMixerGroups.Master, volume);
        }

        public void ChangeMusicVolume(float volume)
        {
            ChangeVolume(AudioMixerGroups.Music, volume);
        }

        public void ChangeSoundVolume(float volume)
        {
            ChangeVolume(AudioMixerGroups.Sounds, volume);
            ChangeVolume(AudioMixerGroups.UI, volume);
        }

        private void ChangeVolume(AudioMixerGroups type, float volume)
        {
            _audioConfig.AudioMixer.SetFloat(type.ToString(), SqrtToDecibel(volume));
            _volumeSettings[type] = volume;
        }

        private float SqrtToDecibel(float volume)
        {
            return Mathf.Lerp(AudioConstants.MIN_VOLUME, AudioConstants.MAX_VOLUME, Mathf.Sqrt(volume));
        }

        public void SaveVolumeSettings()
        {
            _storageService.SaveData(StorageConstants.AUDIO_VOLUMES, _volumeSettings);
        }
    }
}