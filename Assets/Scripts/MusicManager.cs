using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour{

    private const string PLAYER_PREFS_MUSIC_EFFECTS_VOLUME = "MusicEffectsVolume";

    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource;
    private float volume = .3f;

    private void Awake() {
        Instance = this;
        audioSource = GetComponent<AudioSource>();

        // naložimo predhodno shranjeno vrednost (1f je default vrednost)
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_EFFECTS_VOLUME, .3f);
        audioSource.volume = volume;
    }

    public void ChangeVolume() {
        volume += .1f;
        if (volume  > 1f) {
            volume = 0f;
        }
        audioSource.volume = volume;

        // Vrednost bomo shranili, da ostane za naslednjič. To naredimo s pomočjo PlayerPrefs
        // Deluje kot dictionary, samo shraniš vrednost v določen key
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_EFFECTS_VOLUME, volume);
        // Sicer se shrani avtomatsko, ampak če vmes crasha se izgubi zato lahko shranimo takoj
        PlayerPrefs.Save();
    }

    public float GetVolume() {
        return volume;
    }

}
