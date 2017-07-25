using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndSceneActions : MonoBehaviour 
{

	public AudioSource player1Wins;
	public AudioSource player2Wins;
	void Start()
	{
		GameObject.Find("WinText").GetComponent<Text>().text = "Player " + (RuntimeVariables.GetInstance().lastPlayerToWin + 1) + " Wins!";

		if(RuntimeVariables.GetInstance().lastPlayerToWin == 0){
			player1Wins.Play();
		}else if(RuntimeVariables.GetInstance().lastPlayerToWin == 1)
		{
			player2Wins.Play ();
		}
	}



	
    public void RestartButton()
    {
        Application.LoadLevel("StartScene");
    }
    
    public void QuitButton()
    {
        Application.Quit();
    }
    
	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			RestartButton();
	}
}
