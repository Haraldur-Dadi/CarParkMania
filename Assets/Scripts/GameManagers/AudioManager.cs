using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;

    public AudioSource musicAudioSource;
    public AudioSource sfxAudioSource;
    public AudioClip buttonClick;
    public AudioClip buySound;
    public AudioClip wheelSpinning;
    public AudioClip carMoved;
    public AudioClip winSound;

    public Slider musicVolSlider;
    public TextMeshProUGUI musicVolTxt;
    public Slider sfxVolSlider;
    public TextMeshProUGUI sfxVolTxt;

    public Button pitch1;
    public Button pitch2;
    public Button pitch3;
    public Button pitch4;
    public Button lastPitch;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
    }

    private void Start() {
        ChangeMusicVol(PlayerPrefs.GetFloat("MusicVol", 1f));
        ChangeSfxVol(PlayerPrefs.GetFloat("SfxVol", 1f));
        ChangePitch(PlayerPrefs.GetInt("Pitch", 1));
        musicAudioSource.Play();

        musicVolSlider.value = musicAudioSource.volume;
        sfxVolSlider.value = sfxAudioSource.volume;

        musicVolSlider.onValueChanged.AddListener(delegate { ChangeMusicVol(musicVolSlider.value); });
        sfxVolSlider.onValueChanged.AddListener(delegate { ChangeSfxVol(sfxVolSlider.value); });
    }

    public void ChangeMusicVol(float vol) {
        musicAudioSource.volume = Mathf.Round(vol * 100) / 100;
        musicVolTxt.text = (int) (musicAudioSource.volume * 100) + "%";
        PlayerPrefs.SetFloat("MusicVol", musicAudioSource.volume);
    }
    public void ChangeSfxVol(float vol) {
        sfxAudioSource.volume = Mathf.Round(vol * 100) / 100;
        sfxVolTxt.text = (int)(sfxAudioSource.volume * 100) + "%";
        PlayerPrefs.SetFloat("SfxVol", sfxAudioSource.volume);
    }
    public void ChangePitch(int pitch_in) {
        if (lastPitch) {
            lastPitch.interactable = true;
            PlayButtonClick();
        }

        if (pitch_in == 1) {
            musicAudioSource.pitch = 1f;
            pitch1.interactable = false;
            lastPitch = pitch1;
        } else if (pitch_in == 2) {
            musicAudioSource.pitch = 1.5f;
            pitch2.interactable = false;
            lastPitch = pitch2;
        } else if (pitch_in == 3) {
            musicAudioSource.pitch = 1.25f;
            pitch3.interactable = false;
            lastPitch = pitch3;
        } else if (pitch_in == 4) {
            musicAudioSource.pitch = -1f;
            pitch4.interactable = false;
            lastPitch = pitch4;
        }

        PlayerPrefs.SetInt("Pitch", pitch_in);
    }

    public void PlayButtonClick() { sfxAudioSource.PlayOneShot(buttonClick); }
    public void PlayBuySound() { sfxAudioSource.PlayOneShot(buySound); }
    public void PlayWheelSpinning() {
        sfxAudioSource.clip = wheelSpinning;
        sfxAudioSource.Play();
    }
    public void StopWheelSpinning() { sfxAudioSource.Stop(); }
    public void PlayCarMoved() { sfxAudioSource.PlayOneShot(carMoved); }
    public void PlayWinSound() {
        sfxAudioSource.PlayOneShot(winSound);
        StartCoroutine("FadeOutSfx");
    }

    IEnumerator FadeOutSfx() {
        float currentTime = 0;
        float duration = 2;
        float start = sfxAudioSource.volume;

        while (currentTime < duration) {
            currentTime += Time.deltaTime;
            sfxAudioSource.volume = Mathf.Lerp(start, 0, currentTime / duration);
            yield return null;
        }

        sfxAudioSource.Stop();
        sfxAudioSource.clip = null;
        sfxAudioSource.volume = PlayerPrefs.GetFloat("SfxVol", 1f);
    }
}
