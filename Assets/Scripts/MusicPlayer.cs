using UnityEngine;

public class MusicPlayer : MonoBehaviour {

    // Singleton music player
	static MusicPlayer instance = null;
	
	void Start () {
        // If this is NOT the first instance of the object
		if (instance != null && instance != this) {
            // Destroy this object
			Destroy (gameObject);
			print ("Duplicate music player self-destructing!");
		} else {
            // Set this object as the singleton
			instance = this;
            // Do not destroy between scenes
			DontDestroyOnLoad(gameObject);
		}
	}
}
