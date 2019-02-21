using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour {

    // Array of character objects to choose from
    GameObject[] characters;
    // Array to track the indeces of the sprites for each of the characters
    int[] characterIndexes;
    // Sprites loaded from the Resources folder
    Sprite[] characterSprites;

    const int CENTER_INDEX = 1;

    const float BLOCK_TIMER_MAX = 0.2f;
    float blockTimer;

	// Use this for initialization
	void Start () {
        // Set the character objects by finding them in the scene
        characters = new GameObject[3];
        characters[0] = GameObject.Find("Left 1");
        characters[1] = GameObject.Find("Selected Ship");
        characters[2] = GameObject.Find("Right 1");

        // Load the sprites from the Resouces folder
        characterSprites = Resources.LoadAll<Sprite>("ShipSprites/Player");
        Debug.Log("# of sprites: " + characterSprites.Length);

        // Set the indeces of the sprites to be initially shown
        characterIndexes = new int[3];
        characterIndexes[0] = characterSprites.Length - 1;
        characterIndexes[1] = 0;
        characterIndexes[2] = 1;

        // Set the appropriate sprites to the game objects in the scene
        SetCharacterSprites();
    }

    private static bool isAxisInUse = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Continue
        }
        else if (Input.GetButtonDown("Fire"))
        {
            SelectSpaceship();
        }

        if (!isAxisInUse)
        {
            isAxisInUse = true;
            blockTimer = BLOCK_TIMER_MAX;

            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                ChooseRight();
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                ChooseLeft();
            }
        }
        else
        {
            blockTimer -= Time.deltaTime;

            if (blockTimer <= 0.0f)
            {
                isAxisInUse = false;
            }
        }
    }

    /// <summary>
    /// Iterates through each of the 5 game objects in the scene and sets the corresponding sprite to that game object
    /// </summary>
    void SetCharacterSprites()
    {
        for (int i = 0; i < characterIndexes.Length; i++)
        {
            //characters[i].GetComponent<SpriteRenderer>().sprite = characterSprites[characterIndexes[i]];
            characters[i].GetComponent<Image>().sprite = characterSprites[characterIndexes[i]];
        }
    }

    /// <summary>
    /// Shifts all indeces to the left, wrapping around as appropriate, and showing the corresponding sprite
    /// </summary>
    public void ChooseLeft()
    {
        for (int i = 0; i < characterIndexes.Length; i++)
        {
            characterIndexes[i]--;
            if (characterIndexes[i] == -1)
            {
                characterIndexes[i] = characterSprites.Length - 1;
            }

            //characters[i].GetComponent<SpriteRenderer>().sprite = characterSprites[characterIndexes[i]];
        }

        SetCharacterSprites();
    }

    /// <summary>
    /// Shifts all indeces to the right, wrapping around as appropriate, and showing the corresponding sprite
    /// </summary>
    public void ChooseRight()
    {
        for (int i = 0; i < characterIndexes.Length; i++)
        {
            characterIndexes[i]++;
            if (characterIndexes[i] == characterSprites.Length)
            {
                characterIndexes[i] = 0;
            }

            //characters[i].GetComponent<SpriteRenderer>().sprite = characterSprites[characterIndexes[i]];
        }

        SetCharacterSprites();
    }

    /// <summary>
    /// Handler for the "Select Space Craft" button in the scene
    /// </summary>
    public void SelectSpaceship()
    {
        GameObject player = GameObject.Find("Player");
        // Set the player's sprite to use in the game
        player.GetComponent<SpriteRenderer>().sprite = characterSprites[characterIndexes[CENTER_INDEX]];
        player.GetComponent<PlayerController>().SetActive(true);
        SceneManager.LoadScene("Game");
    }
}
