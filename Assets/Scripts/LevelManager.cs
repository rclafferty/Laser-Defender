using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEditor;

public class LevelManager : MonoBehaviour {

    /// <summary>
    /// Centralized method for handling requests to load levels. Allows for further expansion such as debug statements.
    /// </summary>
    /// <param name="name">Name of the level to load</param>
	public void LoadLevel(string name){
		SceneManager.LoadScene (name);
	}

    /// <summary>
    /// Centralized method for handling requests to quit the application.
    /// </summary>
	public void QuitRequest(){
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
	}
}
