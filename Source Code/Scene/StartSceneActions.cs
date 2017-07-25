using UnityEngine;
using System.Collections;

public class StartSceneActions : MonoBehaviour 
{
	void Start()
	{
		RuntimeVariables.GetInstance().isSinglePlayerToggled = false;
	}

	public void PlayVersusButton()
	{
		Application.LoadLevel("GameScene");
	}
	
	public void PlaySinglePlayerButton()
	{
		RuntimeVariables.GetInstance().isSinglePlayerToggled = true;
		Application.LoadLevel("GameScene");
	}
    
    public void QuitButton()
    {
        Application.Quit();
    }
    
    public void Update()
    {
    	if (Input.GetKeyDown(KeyCode.Space))
    		PlayVersusButton();
    }
}
