using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;

    public SaveManager saveManager;

    public AudioSource musicAudioSource;
    public AudioSource sfxAudioSource;
    public AudioClip buttonClick;

    public float musicVol;
    public float sfxVol;
    public int pitch;

    public Slider musicVolSlider;
    public TextMeshProUGUI musicVolTxt;

    public GameObject pitch1;
    public GameObject pitch2;
    public GameObject pitch3;
    public GameObject pitch4;

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

        if (pitch == 1) {
            musicAudioSource.pitch = 1f;
            pitch1.SetActive(true);
            pitch2.SetActive(false);
            pitch3.SetActive(false);
            pitch4.SetActive(false);
        } else if (pitch == 2) {
            musicAudioSource.pitch = 1.5f;
            pitch1.SetActive(false);
            pitch2.SetActive(true);
            pitch3.SetActive(false);
            pitch4.SetActive(false);
        } else if (pitch == 3) {
            musicAudioSource.pitch = 1.25f;
            pitch1.SetActive(false);
            pitch2.SetActive(false);
            pitch3.SetActive(true);
            pitch4.SetActive(false);
        } else if (pitch == 4) {
            musicAudioSource.pitch = -1f;
            pitch1.SetActive(false);
            pitch2.SetActive(false);
            pitch3.SetActive(false);
            pitch4.SetActive(true);
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
}
