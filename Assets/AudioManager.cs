using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource audioSource;

    public bool playMusic;
    public bool playSfx;

    private void Awake() {
        if (PlayerPrefs.GetInt("PlayMusic", 1) == 1) {
            playMusic = true;
        } else {
            playMusic = false;
        }

        if (PlayerPrefs.GetInt("PlaySfx", 1) == 1 ) {
            playSfx = true;
        } else {
            playSfx = false;
        }

        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        audioSource.Play();
    }
}
