using UnityEngine;
using System.Collections;

// plays a set of intro/loop tracks
public class SequencedMusicPlayer : MonoBehaviour
{
	[SerializeField]
	private AudioSource introSource;
	[SerializeField]
	private AudioSource loopSource;
	private AudioSource currentSource;

	[SerializeField, Range(0f, 1f)]
	private float volume = 1f;

	private bool fadeout = false;
	private bool set = false;

	#region MonoBehaviour Methods
	private void Awake()
	{
		if (introSource == null || loopSource == null)
		{
			Debug.LogError("AudioSources must be set in the Inspector.", gameObject);
			enabled = false;
			return;
		}
		currentSource = introSource;
		introSource.playOnAwake = loopSource.playOnAwake = false;
		introSource.volume = loopSource.volume = 0f;
	}

	private void Update()
	{
		if (!set) return;

		if (!fadeout && currentSource.volume != volume)
		{
			UpdateVolume();
		}
	}
	#endregion

	#region Track Assignment
	public void SetTracks(MusicSet musicSet)
	{
		SetTracks(musicSet.introClip, musicSet.loopClip, musicSet.useFade);
	}

	private void SetTracks(AudioClip intro, AudioClip loop, bool doFade = true) {
		set = false;
		
		// just in case we've returned to the same tracks, don't start/stop music again
		if (currentSource.isPlaying && 
		    ((introSource.clip != null && introSource.clip == intro) || (loopSource.clip != null && loopSource.clip == loop))) {
			set = true;
			return;
		}

		StartCoroutine(SetNextTracks((doFade ? 1f : 0f), intro, loop));
	}

	IEnumerator SetNextTracks(float t, AudioClip intro, AudioClip loop) {
		FadeAndStopAll(t);
		yield return new WaitForSeconds(t);
		// assign new tracks
		introSource.clip = intro;
		loopSource.clip = loop;
		
		loopSource.loop = true;
		
		// setup our current source
		if (introSource.clip != null) {
			set = true;
			currentSource = introSource;
			StaticUpdateDelay(true);
		} else if (loopSource.clip != null) {
			set = true;
			currentSource = loopSource;
			StaticUpdateDelay(false);
		}
		yield return null;
	}

	private void StaticUpdateDelay(bool intro = true) {
		double t0 = AudioSettings.dspTime+0.1f;
		double clipTime1 = currentSource.clip.samples;
		clipTime1 /= currentSource.clip.frequency;
		currentSource.PlayScheduled(t0);
		if (intro) {
			currentSource.SetScheduledEndTime(t0+clipTime1);
			loopSource.PlayScheduled(t0+clipTime1);
		}
	}
	#endregion

	#region Track Fading
	private void FadeAndStopAll(float t) {
		if (fadeout) return;

		fadeout = true;
		StartCoroutine(FadeOut(t));
	}

	IEnumerator FadeOut(float t) {
		float pre = volume;
		fadeout = true;

		while (volume > 0f) {
			volume = Mathf.MoveTowards (volume, -0.05f, t*Time.deltaTime);
			introSource.volume = loopSource.volume = Mathf.Clamp01(volume);
			yield return new WaitForEndOfFrame();
		}
		
		StopAll();
		volume = pre;
		fadeout = false;
		
		yield return null;
	}

	private void StopAll() {
		introSource.Stop ();
		loopSource.Stop();
	}
	#endregion

	#region Volume Control
	private void UpdateVolume()
	{
		introSource.volume = loopSource.volume = volume;
	}

	public float Volume 
	{
		get
		{
			return volume;
		}

		set
		{
			volume = Mathf.Clamp01(value);
		}
	}
	#endregion
}