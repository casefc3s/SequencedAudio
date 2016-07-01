# SequencedAudio
Simple scripts that can be used to play audio tracks seamlessly (i.e. intro track to loop track) in Unity.

# Usage
Add the SequencedMusicPlayer to a GameObject in the scene and also add two AudioSource components to that same object, then assign them to the SMP in the Inspector. Then add a MusicSet to another GameObject in the same scene. Assign the audio tracks to the MusicSet and select the options you want (fading/play on awake). It is not necessary to have tracks assigned to both fields if they're not needed.

# Recommendations
I suggest using the Singleton class from the Unify Wiki and applying that to the SequencedMusicPlayer, then placing the SMP on an object in your initial loading scene that is persistent via DontDestroyOnLoad(). This will eliminate the need to use FindObjectOfType in the MusicSet's Start(), and instead you can call SequencedMusicPlayer.Instance.PlayTracks() directly. This will also enable you to place MusicSet's in individual scenes, so that you can have a persistent music player controlling what audio is being played as scenes change.
