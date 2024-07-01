using UnityEngine;

public class CharacterSoundController : MonoBehaviour
{
    const float SEMITONE_CONSTANT = 1.05946f;
    public int[] semitonePallette;

    private float _initialPitch;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        _initialPitch = audioSource.pitch;
        _initialPitch = GetPitchFromSemitones(semitonePallette[0]);
        audioSource.pitch = _initialPitch;
    }

    public void PlayNextSound()
    {
        float p = GetNextNote();
        audioSource.pitch = p;
        audioSource.Play();
    }

    public float GetNextNote()
    {
        int i = Random.Range(0, semitonePallette.Length);
        return GetPitchFromSemitones(semitonePallette[i]);
    }

    public float GetPitchFromSemitones(float s)
    {
        return Mathf.Pow(SEMITONE_CONSTANT, s);
    }

    public void ShiftAudio(float s)
    {
        audioSource.pitch = GetPitchFromSemitones(s);
    }

    public void RevertAudio()
    {
        audioSource.pitch = _initialPitch;
    }
}
