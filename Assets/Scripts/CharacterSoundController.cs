using UnityEngine;

public class CharacterSoundController : MonoBehaviour
{
    const float SEMITONE_CONSTANT = 1.05946f;
    public int[] semitonePallette;
    public float fadeLength = 0.5f;
    public float maxGain = 1.0f;

    private float _initialPitch;
    private AudioSource audioSource;
    private float _timer = 0f;
    private float _t = 0f;
    private float _targetGain = 0f;
    private float _currentGain = 0f;
    private bool _startFade = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        _initialPitch = audioSource.pitch;
        _initialPitch = GetPitchFromSemitones(semitonePallette[0]);
        audioSource.pitch = _initialPitch;
        audioSource.volume = maxGain;
    }

    private void Update()
    {
        if(_startFade)
        {
            _timer += Time.deltaTime;
            _t = Mathf.Clamp(_timer/fadeLength, 0, 1);

            Debug.Log(_t);

            _currentGain = Mathf.Lerp(_currentGain, _targetGain, _t);
            audioSource.volume = _currentGain;

            if(_t >= 1)
            {
                _startFade = false;
                _timer = 0f;

                if (_targetGain == 0f)
                {
                    StopSound();
                }
            }
        }
    }

    public void PlayNextSound()
    {
        float p = GetNextNote();
        audioSource.pitch = p;
        audioSource.Play();
    }

    public void StopSound()
    {
        audioSource.Stop();
    }

    public void PauseSound()
    {
        audioSource.Pause();
    }

    public void FadeInSound()
    {
        _currentGain = audioSource.volume;
        _targetGain = maxGain;
        _startFade = true;
        PlayNextSound();
    }

    public void FadeOutSound()
    {
        _currentGain = audioSource.volume;
        _targetGain = 0f;
        _startFade = true;
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
