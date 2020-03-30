using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioManager : MonoBehaviour {

    public SaveManager saveManager;

    public AudioSource musicAudioSource;
    public AudioSource sfxAudioSource;

    public float musicVol;
    public float sfxVol;
    public int pitch;

    public Slider musicVolSlider;
    public TextMeshProUGUI musicVolTxt;

    public Toggle pitch1;
    public Toggle pitch2;
    public Toggle pitch3;
    public Toggle pitch4;

    public Slider sfxVolSlider;
    public TextMeshProUGUI sfxVolTxt;

    private void Awake() {
        musicVol = PlayerPrefs.GetFloat("MusicVol", 1f);
        sfxVol = PlayerPrefs.GetFloat("SfxVol", 1f);
        pitch = PlayerPrefs.GetInt("Pitch", 1);

        saveManager = SaveManager.Instance;
        AudioSource[] audioSources = GetComponents<AudioSource>();
        musicAudioSource = audioSources[0];
        sfxAudioSource = audioSources[1];
        musicVolSlider = GameObject.Find("MusicVolSlider").GetComponent<Slider>();
        musicVolTxt = GameObject.Find("MusicVolTxt").GetComponent<TextMeshProUGUI>();
        pitch1 = GameObject.Find("Pitch1").GetComponent<Toggle>();
        pitch2 = GameObject.Find("Pitch2").GetComponent<Toggle>();
        pitch3 = GameObject.Find("Pitch3").GetComponent<Toggle>();
        pitch4 = GameObject.Find("Pitch4").GetComponent<Toggle>();
        sfxVolSlider = GameObject.Find("SfxVolSlider").GetComponent<Slider>();
        sfxVolTxt = GameObject.Find("SfxVolTxt").GetComponent<TextMeshProUGUI>();

        musicVolSlider.onValueChanged.AddListener(delegate { ChangeMusicVol(musicVolSlider.value); });
        pitch1.onValueChanged.AddListener(delegate { ChangePitch(pitch1, 1); });
        pitch2.onValueChanged.AddListener(delegate { ChangePitch(pitch2, 2); });
        pitch3.onValueChanged.AddListener(delegate { ChangePitch(pitch3, 3); });
        pitch4.onValueChanged.AddListener(delegate { ChangePitch(pitch4, 3); });
        sfxVolSlider.onValueChanged.AddListener(delegate { ChangeSfxVol(sfxVolSlider.value); });
    }

    private void Start() {
        musicVolTxt.text = musicVol * 100 + "%";
        musicVolSlider.value = musicVol;
        musicAudioSource.volume = musicVol;
        sfxAudioSource.volume = sfxVol;
        sfxVolSlider.value = sfxVol;

        if (musicVol > 0f) {
            musicAudioSource.Play();
        }

        if (pitch == 1) {
            pitch1.isOn = true;
        } else if (pitch == 2) {
            pitch2.isOn = true;
        } else if (pitch == 3) {
            pitch3.isOn = true;
        } else if (pitch == 4) {
            pitch4.isOn = true;
        }
    }

    public void ChangeMusicVol(float vol) {
        musicVol = Mathf.Round(vol * 100) / 100;
        musicVolTxt.text = (int) (musicVol * 100) + "%";
        musicAudioSource.volume = musicVol;

        saveManager.SaveFloatData("MusicVol", musicVol);
    }
    
    public void ChangePitch(Toggle toggler, int pitch_in) {
        if (toggler.isOn) {
            if (pitch_in == 1) {
                musicAudioSource.pitch = 1f;
                pitch2.isOn = false;
                pitch3.isOn = false;
                pitch4.isOn = false;
            } else if (pitch_in == 2) {
                musicAudioSource.pitch = 1.5f;
                pitch1.isOn = false;
                pitch3.isOn = false;
                pitch4.isOn = false;
            } else if (pitch_in == 3) {
                musicAudioSource.pitch = 1.25f;
                pitch1.isOn = false;
                pitch2.isOn = false;
                pitch4.isOn = false;
            } else if (pitch_in == 4) {
                musicAudioSource.pitch = -1f;
                pitch1.isOn = false;
                pitch2.isOn = false;
                pitch3.isOn = false;
            }
            pitch = pitch_in;

            saveManager.SaveIntData("Pitch", pitch);
        }
    }

    public void ChangeSfxVol(float vol) {
        sfxVol = Mathf.Round(vol * 100) / 100;
        sfxVolTxt.text = (int)(sfxVol * 100) + "%";
        sfxAudioSource.volume = sfxVol;

        saveManager.SaveFloatData("SfxVol", sfxVol);
    }
}
