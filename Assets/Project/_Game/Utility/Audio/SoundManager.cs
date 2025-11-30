using System.Collections.Generic;
using MoveStopMove.Utility.Extension;
using UnityEngine;

namespace MoveStopMove.Utility.Audio
{
    public class SoundManager : Singleton<SoundManager>
    {
        #region -- Fields --

        [Header("Audio Sources")]
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private AudioSource sfxLoopSource;

        [Header("SFX Clips")]
        [SerializeField] private List<SfxEntry> sfxEntries = new();

        [Header("BGM")]
        [SerializeField] private AudioClip defaultBgm;

        private Dictionary<ESfxType, AudioClip> m_sfxDict;

        #endregion

        #region -- Methods --

        protected override void Awake()
        {
            base.Awake();

            BuildSfxDictionary();

            if (bgmSource != null && defaultBgm != null)
            {
                PlayBGM(defaultBgm);
            }
        }

        private void BuildSfxDictionary()
        {
            m_sfxDict = new Dictionary<ESfxType, AudioClip>();

            foreach (var entry in sfxEntries)
            {
                if (entry == null || entry.clip == null) continue;

                if (!m_sfxDict.ContainsKey(entry.type))
                {
                    m_sfxDict.Add(entry.type, entry.clip);
                }
                else
                {
                    Debug.LogWarning($"[SoundManager] Trùng key SFX: {entry.type}");
                }
            }
        }

        #region SFX

        public void PlaySFX(ESfxType type)
        {
            if (!m_sfxDict.TryGetValue(type, out var clip) || clip == null) return;
            if (sfxSource == null) return;

            sfxSource.Stop();
            sfxSource.clip = clip;
            sfxSource.loop = false;
            sfxSource.Play();
        }

        public void PlayLoopSFX(ESfxType type)
        {
            if (sfxLoopSource == null)
            {
                Debug.LogWarning("[SoundManager] Chưa gắn SFX Loop AudioSource");
                return;
            }

            if (!m_sfxDict.TryGetValue(type, out var clip) || clip == null)
            {
                Debug.LogWarning($"[SoundManager] Không tìm thấy clip loop cho: {type}");
                return;
            }

            // Nếu đang phát đúng clip rồi thì khỏi làm gì
            if (sfxLoopSource.isPlaying && sfxLoopSource.clip == clip)
                return;

            sfxLoopSource.Stop();
            sfxLoopSource.clip = clip;
            sfxLoopSource.loop = true;
            sfxLoopSource.Play();
        }

        public void StopLoopSFX()
        {
            if (sfxLoopSource == null) return;
            sfxLoopSource.Stop();
            sfxLoopSource.clip = null;
        }

        #endregion

        #region BGM

        public void PlayBGM(AudioClip clip, bool loop = true)
        {
            if (bgmSource == null || clip == null) return;

            if (bgmSource.clip == clip && bgmSource.isPlaying) return;

            bgmSource.clip = clip;
            bgmSource.loop = loop;
            bgmSource.Play();
        }

        public void StopBGM()
        {
            if (bgmSource == null) return;
            bgmSource.Stop();
            bgmSource.clip = null;
        }

        #endregion

        #endregion
    }
}