using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public AudioClip[] soundEffects;  // Array untuk efek suara
    private AudioSource soundEffectSource;  // AudioSource untuk memainkan efek suara

    [Range(0f, 1f)] public float effectsVolume = 0.5f;  // Volume efek suara

    private void Awake()
    {
        // Menambahkan komponen AudioSource ke objek ini
        soundEffectSource = gameObject.AddComponent<AudioSource>();

        // Mengatur volume efek suara
        soundEffectSource.volume = effectsVolume;
    }

    // Method untuk memainkan efek suara berdasarkan indeks dalam array soundEffects
    public void PlaySoundEffect(int soundIndex)
    {
        if (soundIndex >= 0 && soundIndex < soundEffects.Length)
        {
            soundEffectSource.PlayOneShot(soundEffects[soundIndex]);
        }
        else
        {
            Debug.LogWarning("Sound effect index out of range");
        }
    }

    // Method untuk mengubah volume efek suara
    public void SetEffectsVolume(float volume)
    {
        effectsVolume = Mathf.Clamp(volume, 0f, 1f);  // Menjaga volume antara 0 dan 1
        soundEffectSource.volume = effectsVolume;  // Mengatur volume AudioSource
    }
}
