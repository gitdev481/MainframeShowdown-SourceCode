using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
	private int currentLane = 0;
    private int numDesks;
    private float distanceToMove;

    void Start()
    {
    	SceneManager sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        numDesks = sceneManager.DesksCount;
        distanceToMove = sceneManager.BASE_DESK_OFFSET_Y;
    }
    
    public void MovePlayerUp(GameObject player)
    {
        if (currentLane == numDesks - 1)
            return;
        
        TranslatePlayerY(distanceToMove);
        ++currentLane;
    }
    
    public void MovePlayerDown(GameObject player)
    {
        if (currentLane == 0)
            return;
        
        TranslatePlayerY(-distanceToMove);
        --currentLane;
    }

	private void TranslatePlayerY(float amount)
	{
		gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + amount);
	}
	
	public int CurrentLane
	{
		get { return currentLane; }
	}
	
	public void MoveToLane(int lane)
	{
		float amountToMove = (currentLane + lane) * 1.5f;
		if (lane < currentLane)
			amountToMove = -amountToMove;
		
		currentLane = lane;
		TranslatePlayerY(amountToMove);
	}
}
