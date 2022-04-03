using UnityEngine;

/// <summary>
///     Helper class to be used to provide functions to be used by Menu buttons
/// </summary>
public class SoundManager : MonoBehaviour
{
    public AudioClip moveSound;

    public AudioClip clickSound;

    public AudioClip startSound;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayMoveSound()
    {
        _audioSource.PlayOneShot(moveSound);
    }

    public void PlayClickSound()
    {
        _audioSource.PlayOneShot(clickSound);
    }

    public void PlayStartSound()
    {
        _audioSource.PlayOneShot(startSound);
    }
}