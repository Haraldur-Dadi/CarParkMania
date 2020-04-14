using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;

    public SaveManager saveManager;

    public AudioSource musicAudioSource;
    public AudioSource sfxAudioSource;
    public AudioClip buttonClick;
    public AudioClip buySound;
    public AudioClip wheelSpinning;
    public AudioClip carSelected;

    public float musicVol;
    public float sfxVol;
    public int pitch;

    public Slider musicVolSlider;
    public TextMeshProUGUI musicVolTxt;

    public Button pitch1;
    public Button pitch2;
    public Button pitch3;
    public Button pitch4;
    public Button lastPitch;

    public Slider sfxVolSlider;
    public TextMeshProUGUI sfxVolTxt;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            return;
        }
    }

    private void Start() {
        ChangeMusicVol(PlayerPrefs.GetFloat("MusicVol", 1f));
        ChangeSfxVol(PlayerPrefs.GetFloat("SfxVol", 1f));
        ChangePitch(PlayerPrefs.GetInt("Pitch", 1));
        musicAudioSource.Play();

        musicVolSlider.value = musicVol;
        sfxVolSlider.value = sfxVol;

        musicVolSlider.onValueChanged.AddListener(delegate { ChangeMusicVol(musicVolSlider.value); });
        sfxVolSlider.onValueChanged.AddListener(delegate { ChangeSfxVol(sfxVolSlider.value); });
    }

    public void ChangeMusicVol(float vol) {
        musicVol = Mathf.Round(vol * 100) / 100;
        musicVolTxt.text = (int) (musicVol * 100) + "%";
        musicAudioSource.volume = musicVol;

        saveManager.SaveFloatData("MusicVol", musicVol);
    }
    
    public void ChangePitch(int pitch_in) {
        pitch = pitch_in;
        if (lastPitch) {
            lastPitch.interactable = true;
            PlayButtonClick();
        }

        if (pitch == 1) {
            musicAudioSource.pitch = 1f;
            pitch1.interactable = false;
            lastPitch = pitch1;
        } else if (pitch == 2) {
            musicAudioSource.pitch = 1.5f;
            pitch2.interactable = false;
            lastPitch = pitch2;
        } else if (pitch == 3) {
            musicAudioSource.pitch = 1.25f;
            pitch3.interactable = false;
            lastPitch = pitch3;
        } else if (pitch == 4) {
            musicAudioSource.pitch = -1f;
            pitch4.interactable = false;
            lastPitch = pitch4;
        }

        saveManager.SaveIntData("Pitch", pitch);
    }

    public void ChangeSfxVol(float vol) {
        sfxVol = Mathf.Round(vol * 100) / 100;
        sfxVolTxt.text = (int)(sfxVol * 100) + "%";
        sfxAudioSource.volume = sfxVol;

        saveManager.SaveFloatData("SfxVol", sfxVol);
    }

    public void PlayButtonClick() {
        sfxAudioSource.PlayOneShot(buttonClick);
    }
    
    public void PlayBuySound() {
        sfxAudioSource.PlayOneShot(buySound);
    }

    public void PlayWheelSpinning() {
        sfxAudioSource.clip = wheelSpinning;
        sfxAudioSource.Play();
    }
    public void StopWheelSpinning() {
        sfxAudioSource.Stop();
    }

    public void PlayCarSelected() {
        StopCoroutine("FadeOutSfx");
        sfxAudioSource.volume = sfxVol;
        sfxAudioSource.clip = carSelected;
        sfxAudioSource.Play();
    }

    public void StopCarSelected() {
        StartCoroutine("FadeOutSfx");
    }

    IEnumerator FadeOutSfx() {
        float currentTime = 0;
        float duration = 0.25f;
        float start = sfxAudioSource.volume;

        while (currentTime < duration) {
            currentTime += Time.deltaTime;
            sfxAudioSource.volume = Mathf.Lerp(start, 0, currentTime / duration);
            yield return null;
        }

        sfxAudioSource.Stop();
        sfxAudioSource.clip = null;
        sfxAudioSource.volume = sfxVol;
    }
}
