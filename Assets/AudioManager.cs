using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioManager : MonoBehaviour {

    public SaveManager saveManager;

    public AudioSource audioSource;

    public float musicVol;
    public float sfxVol;
    public int pitch;

    public Slider musicVolSlider;
    public TextMeshProUGUI musicVolTxt;

    public Toggle pitch1;
    public Toggle pitch2;
    public Toggle pitch3;
    public Toggle pitch4;

    private void Awake() {
        musicVol = PlayerPrefs.GetFloat("MusicVol", 1f);
        sfxVol = PlayerPrefs.GetFloat("SfxVol", 1f);
        pitch = PlayerPrefs.GetInt("Pitch", 1);

        saveManager = SaveManager.Instance;
        audioSource = GetComponent<AudioSource>();
        musicVolSlider = GameObject.Find("MusicVolSlider").GetComponent<Slider>();
        musicVolTxt = GameObject.Find("MusicVolTxt").GetComponent<TextMeshProUGUI>();
        pitch1 = GameObject.Find("Pitch1").GetComponent<Toggle>();
        pitch2 = GameObject.Find("Pitch2").GetComponent<Toggle>();
        pitch3 = GameObject.Find("Pitch3").GetComponent<Toggle>();
        pitch4 = GameObject.Find("Pitch4").GetComponent<Toggle>();

        musicVolSlider.onValueChanged.AddListener(delegate { ChangeMusicVol(musicVolSlider.value); });
        pitch1.onValueChanged.AddListener(delegate { ChangePitch(pitch1, 1); });
        pitch2.onValueChanged.AddListener(delegate { ChangePitch(pitch2, 2); });
        pitch3.onValueChanged.AddListener(delegate { ChangePitch(pitch3, 3); });
        pitch4.onValueChanged.AddListener(delegate { ChangePitch(pitch4, 3); });
    }

    private void Start() {
        musicVolTxt.text = musicVol * 100 + "%";
        musicVolSlider.value = musicVol;

        if (musicVol > 0f) {
            audioSource.volume = musicVol;
            audioSource.Play();
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
        audioSource.volume = musicVol;

        saveManager.SaveFloatData("MusicVol", musicVol);
    }
    
    public void ChangePitch(Toggle toggler, int pitch_in) {
        if (toggler.isOn) {
            if (pitch_in == 1) {
                audioSource.pitch = 1f;
                pitch2.isOn = false;
                pitch3.isOn = false;
                pitch4.isOn = false;
            } else if (pitch_in == 2) {
                audioSource.pitch = 1.5f;
                pitch1.isOn = false;
                pitch3.isOn = false;
                pitch4.isOn = false;
            } else if (pitch_in == 3) {
                audioSource.pitch = 1.25f;
                pitch1.isOn = false;
                pitch2.isOn = false;
                pitch4.isOn = false;
            } else if (pitch_in == 4) {
                audioSource.pitch = -1f;
                pitch1.isOn = false;
                pitch2.isOn = false;
                pitch3.isOn = false;
            }
            pitch = pitch_in;

            saveManager.SaveIntData("Pitch", pitch);
        }
    }
}
