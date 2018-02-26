using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour {

    GameObject[] characters;
    int[] characterIndexes;

    int selectedIndex;
    int right1Index;
    int right2Index;
    int left1Index;
    int left2Index;

    Sprite[] characterSprites;

	// Use this for initialization
	void Start () {
        
        characters = new GameObject[5];
        characters[0] = GameObject.Find("Left 2");
        characters[1] = GameObject.Find("Left 1");
        characters[2] = GameObject.Find("Selected Ship");
        characters[3] = GameObject.Find("Right 1");
        characters[4] = GameObject.Find("Right 2");

        characterSprites = Resources.LoadAll<Sprite>("ShipSprites/Player");

        characterIndexes = new int[5];
        characterIndexes[0] = characterSprites.Length - 2;
        characterIndexes[1] = characterSprites.Length - 1;
        characterIndexes[2] = 0;
        characterIndexes[3] = 1;
        characterIndexes[4] = 2;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChooseLeft()
    {
        for (int i = 0; i < characterIndexes.Length; i++)
        {
            characterIndexes[i]--;
            if (characterIndexes[i] == -1)
            {
                characterIndexes[i] = characterSprites.Length - 1;
            }

            characters[i].GetComponent<SpriteRenderer>().sprite = characterSprites[characterIndexes[i]];
        }
    }

    public void ChooseRight()
    {
        for (int i = 0; i < characterIndexes.Length; i++)
        {
            characterIndexes[i]++;
            if (characterIndexes[i] == characterSprites.Length)
            {
                characterIndexes[i] = 0;
            }

            characters[i].GetComponent<SpriteRenderer>().sprite = characterSprites[characterIndexes[i]];
        }
    }

    public void SelectSpaceship()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<SpriteRenderer>().sprite = characterSprites[characterIndexes[2]];
        player.GetComponent<PlayerController>().SetActive(true);
        SceneManager.LoadScene("Game");
    }
}
