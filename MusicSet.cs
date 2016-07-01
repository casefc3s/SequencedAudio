using UnityEngine;

public class MusicSet : MonoBehaviour
{
	public AudioClip introClip;
	public AudioClip loopClip;
	public bool useFade = true;
	public bool playOnAwake = true;

	void Start()
	{
		if (playOnAwake)
		{
			SequencedMusicPlayer smp = FindObjectOfType<SequencedMusicPlayer>();
			if (smp != null)
			{
				smp.SetTracks(this);
			}
		}
	}
}