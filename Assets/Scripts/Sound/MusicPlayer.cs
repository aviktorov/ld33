//#define DEBUG_FIELDS

using UnityEngine;
using System.Collections;

public enum MusicPlayerMode {
	FadingIn,
	Playing,
	FadingOut
}

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer :  MonoSingleton<MusicPlayer> {
	public float fadeInTime = 5.0f;
	public float fadeOutTime = 5.0f;
	public AudioClip[] tracks;

	#if DEBUG_FIELDS
	public MusicPlayerMode mode;
	public float timer;
	public int trackIndex;
	#else
	private MusicPlayerMode mode;
	private float timer;
	private int trackIndex;
	#endif
	private float cachedMaxVolume;

	private void Awake() {
		DontDestroyOnLoad(gameObject);

		cachedMaxVolume = GetComponent<AudioSource>().volume;

		if(tracks.Length != 0)
			StartFadingIn(tracks[0]);
	}

	private void Update() {
		timer -= Time.fixedDeltaTime;

		if (timer <= 0.0f) {
			switch(mode) {
				case MusicPlayerMode.FadingIn: StartPlaying(); break;
				case MusicPlayerMode.Playing: StartFadingOut(); break;
				case MusicPlayerMode.FadingOut: StartFadingIn(tracks[(tracks.Length == 1) ? 0 : (++trackIndex % (tracks.Length - 1))]); break;
			}
		}

		UpdateVolume();
	}

	private void UpdateVolume() {
		switch(mode) {
			case MusicPlayerMode.FadingIn: GetComponent<AudioSource>().volume = cachedMaxVolume * (1.0f - timer / fadeInTime); break;
			case MusicPlayerMode.FadingOut: GetComponent<AudioSource>().volume = cachedMaxVolume * (timer / fadeOutTime); break;
		}
	}

	public void StartFadingIn(AudioClip clip) {
		GetComponent<AudioSource>().clip = clip;
		GetComponent<AudioSource>().volume = 0.0f;

		if(!GetComponent<AudioSource>().isPlaying)
			GetComponent<AudioSource>().Play();

		mode = MusicPlayerMode.FadingIn;
		timer = fadeInTime;
	}

	public void StartPlaying() {
		GetComponent<AudioSource>().volume = cachedMaxVolume;

		mode = MusicPlayerMode.Playing;
		timer = Mathf.Max(0.0f, GetComponent<AudioSource>().clip.length - fadeInTime - fadeOutTime);
	}

	public void StartFadingOut() {
		GetComponent<AudioSource>().volume = cachedMaxVolume;

		mode = MusicPlayerMode.FadingOut;
		timer = fadeOutTime;
	}
}
