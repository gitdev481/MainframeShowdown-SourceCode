using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SceneManager : MonoBehaviour 
{
    public List<GameObject> players;
    public GameObject playerRef;

	public GameObject nonPlayerCharacterRef;

	private GameObject newNPC = null;
	private GameObject newPlayer = null;

	private int numberOfPlayers = 2;
    
    public GameObject defenseTriggerRef;
    public List<GameObject> defenseTriggers;
    
	public int desiredDesks = 4;
	public List<GameObject> desks;
	public GameObject deskRef;

    private bool controlEnabled;
    private float BASE_DEFENSE_TRIGGER_OFFSET_X = 2.5f;
	private RuntimeVariables runtimeVariables;

	public GameObject computerRef;
	public List<GameObject> leftComputers;
	public List<GameObject> rightComputers;
	
	public GameObject stationRef;
	public GameObject roundOverText;
	public float BASE_DESK_OFFSET_Y = 1.5f;

	public Sprite server;
	public Sprite serverExploded;
	public GameObject serverRef;
	public List<GameObject> leftServers;
	public List<GameObject> rightServers;

	private float serverOffsetX = 1.425f;
	private float serverOffsetY = 0.75f;
	private float distanceBetweenServers = 0.93f;
	
	public bool[] player0ThreatenedByProjectileInLane;
	public bool[] player1ThreatenedByProjectileInLane;
	public Sprite computerThreatenedSprite;
	public Sprite computerIdleSprite;

	void Awake()
    {
		runtimeVariables = RuntimeVariables.GetInstance ();
		runtimeVariables.player0RoundsWon = 0;
		runtimeVariables.player1RoundsWon = 0;
		
		// Check if the player has selected single player mode. 
		if (runtimeVariables.isSinglePlayerToggled) 
		{
			numberOfPlayers = 1;
		} else {
			numberOfPlayers = 2;
		}
		
		player0ThreatenedByProjectileInLane = new bool[desiredDesks];
		player1ThreatenedByProjectileInLane = new bool[desiredDesks];
		
        SpawnDesks();
		SpawnComputers();
		SpawnServers ();
		SetServerPositions (leftServers);
		SetServerPositions (rightServers);
        ZoomCamera();
        SpawnDefenseTriggers();
        SpawnPlayers();  
    }
    
    void Update()
    {
		UpdateComputerSceens(player0ThreatenedByProjectileInLane, leftComputers);
		UpdateComputerSceens(player1ThreatenedByProjectileInLane, rightComputers);
    }
    
    void UpdateComputerSceens(bool[] playerThreatenedByProjectileInLane, List<GameObject> list)
    {
		for (int i = 0; i < list.Count; ++i)
		{
			if (playerThreatenedByProjectileInLane[i])
				list[i].GetComponent<Image>().sprite = computerThreatenedSprite;
			else
				list[i].GetComponent<Image>().sprite = computerIdleSprite;
		}
    }
    
	void SpawnDesks()
	{
		GameObject deskParent = GameObject.Find("Desks");
		for (int i = 0; i < desiredDesks; ++i) 
		{
			GameObject newDesk = Instantiate(deskRef);
			newDesk.name = "Desk" + i;
			
			newDesk.transform.SetParent(deskParent.transform);
			newDesk.transform.position = new Vector2(0, deskParent.transform.position.y + (i * BASE_DESK_OFFSET_Y));
			desks.Add(newDesk);
		}
		
		deskParent.transform.position = new Vector2(0, 0 - (desiredDesks - 1.5f));
	}
	
	void SpawnServers()
	{
		GameObject serverParent = GameObject.Find ("Servers");

		for (int i = 0; i < desiredDesks * 2; i++) 
		{
			SpawnServer(serverParent, i, leftServers);
		}

		for (int i = 0; i < desiredDesks * 2; i++)
		{
			SpawnServer(serverParent, i, rightServers);
		}
	}
	
	public void ResetServers()
	{
		GameObject.Find ("Player0Defense").GetComponent<PlayerDefense> ().hitsTaken = 0;
		GameObject.Find ("Player1Defense").GetComponent<PlayerDefense> ().hitsTaken = 0;
		for (int i = 0; i < leftServers.Count; i++) {
			leftServers[i].gameObject.GetComponent<Image> ().sprite = GameObject.Find ("SceneManager").GetComponent<SceneManager> ().server;
			rightServers[i].gameObject.GetComponent<Image> ().sprite = GameObject.Find ("SceneManager").GetComponent<SceneManager> ().server;
		}
	}
	
	void SpawnServer(GameObject serverParent, int i, List<GameObject> list)
	{

		GameObject newServer = Instantiate (serverRef);
		newServer.name = "Server" + i;
		newServer.transform.SetParent (serverParent.transform);
		list.Add (newServer);
	}
	
	void SetServerPositions(List<GameObject> serverList)
	{
		float tempOffsetX = -(desks[0].transform.localScale.x / serverOffsetX);
		if (serverList == leftServers) 
		{
		
		} else if (serverList == rightServers) 
		{
			tempOffsetX = -tempOffsetX;
		}

		serverList [0].transform.position = new Vector2 (tempOffsetX, desks [0].transform.position.y - serverOffsetY);
		for (int i = 1; i < serverList.Count; i++) 
		{
			serverList[i].transform.position = new Vector2(serverList[i-1].transform.position.x, serverList[i-1].transform.position.y +distanceBetweenServers );
		}
	}

	void SpawnComputers()
	{
		GameObject computerParent = GameObject.Find("Computers");
		GameObject stationParent = GameObject.Find("Stations");
		
		for (int i = 0; i < desiredDesks; i++) 
		{
			SpawnComputer(computerParent, stationParent, i, leftComputers, 270);
		}
		
		for (int i = 0; i < desiredDesks; i++) 
		{
			SpawnComputer(computerParent, stationParent, i, rightComputers, 90);
		}
	}

	
	void SpawnComputer(GameObject computerParent, GameObject stationParent, int i, List<GameObject> list, float rotationZ)
	{
		GameObject newComputer = Instantiate(computerRef);
		newComputer.name = "Computer" + i;
		newComputer.transform.SetParent(computerParent.transform);
		newComputer.transform.localEulerAngles = new Vector3(0, 0, rotationZ);
		newComputer.transform.Find("ComputerScreenFirewall").transform.GetComponent<Image>().fillAmount = 0;
		list.Add(newComputer);
		
		int tempSide = 0;
		float tempOffsetX = -(desks[0].transform.localScale.x / 2);
		if (list == leftComputers)
		{
			tempSide = 0;
		}
		else if (list == rightComputers)
		{
			tempSide = 1;
			tempOffsetX = -tempOffsetX;
		}
		
		newComputer.transform.position = new Vector2(tempOffsetX, desks[i].transform.position.y);
		SpawnStation(stationParent, newComputer.transform, tempSide, rotationZ);
	}

	public void ResetComputerFill(List<GameObject> list){
		for (int i = 0; i < list.Count; i++) {
			list[i].transform.Find("Arrow").GetComponent<Image> ().fillAmount = 0;
		}
	}
	
	void SpawnStation(GameObject stationParent, Transform newComputer, int side, float rotationZ)
	{
		GameObject newStation = Instantiate(stationRef);
		newStation.name = "Station";
		newStation.transform.SetParent(stationParent.transform);
		newStation.transform.localEulerAngles = new Vector3(0, 0, rotationZ);
		
		float tempOffsetX = 1.2f;
		if (side == 1)
			tempOffsetX = -tempOffsetX;
			
		newStation.transform.position = new Vector2(newComputer.position.x - tempOffsetX, newComputer.position.y);
	}
	
	void ZoomCamera()
	{
		Camera cameraRef = GameObject.Find("Main Camera").GetComponent<Camera>();
		if (desiredDesks > 4)
			cameraRef.orthographicSize = desiredDesks;
	}
	
	void SpawnDefenseTriggers()
	{
		float deskLength = desks[0].transform.localScale.x;
        float offsetX = -((deskLength / 2) + BASE_DEFENSE_TRIGGER_OFFSET_X);
        
		for (int i = 0; i < 2; ++i)
		{
			GameObject newDefenseTrigger = Instantiate(defenseTriggerRef);
			newDefenseTrigger.name = "Player" + i + "Defense";
			
			if (i == 1) 
                offsetX = -offsetX;
                
			newDefenseTrigger.transform.position = new Vector2(offsetX, 0);
			newDefenseTrigger.transform.localScale = new Vector2(1, desiredDesks * 2);
            defenseTriggers.Add(newDefenseTrigger);
		}
	}

	void SpawnPlayers()
	{
		float offsetX = -((desks[0].transform.localScale.x / 2) + 1.2f);
		
		for (int i = 0; i < numberOfPlayers; ++i)
		{
            newPlayer = Instantiate(playerRef);
            newPlayer.name = "Player" + i;
            
            if (i == 1)
                offsetX = -offsetX;
                
			newPlayer.transform.position = new Vector2 (0 + offsetX, desks[0].transform.position.y);
            newPlayer.GetComponent<PlayerProperties>().playerID = i;
            players.Add(newPlayer);

			if (numberOfPlayers == 1)
			{
				offsetX = -offsetX;
				SpawnNPC(offsetX);
			}
		}

		if (newPlayer.GetComponent<PlayerProperties>().playerID == 1) 
		{
			newPlayer.transform.localEulerAngles = new Vector3(0,0,180);
		}
	}

	void SpawnNPC(float offSetX)
	{
		newNPC = Instantiate (nonPlayerCharacterRef);
		newNPC.name = "Player1";

		newNPC.transform.position = new Vector2 (0 + offSetX, desks [0].transform.position.y);
		newNPC.GetComponent<PlayerProperties> ().playerID = 1;
		newNPC.transform.localEulerAngles = new Vector3(0,0,180);

		players.Add(newNPC);
	}
	
	public int DesksCount
	{
		get { return desks.Count; }
	}
    
    public void EnablePlayerControl(bool value)
	{
        foreach (GameObject p in players)
        {
			if (p.gameObject.tag == "Player")
				p.GetComponent<PlayerControl>().isControllable = value;
				
			if (p.gameObject.tag == "NPC")
				newNPC.GetComponent<NPCControl>().isControllable = value;
        }
    }
	
    public void EndOfRound()
    {
		roundOverText.GetComponent<Text> ().text = "ROUND OVER";
		//GameObject.FindGameObjectWithTag ("AUDIOMANAGER").GetComponent<AudioManager> ().PlayRoundOverSound ();
		DetermineRoundWinner();

		ResetComputerScreens ();
		ResetPlayerPositions();
    	RemoveAllFirewalls();
    	RemoveAllProjectiles();
    	ResetDefense();
    	ResetBuildTimers();
    	ResetThreatenLists();
    	
		//wait for 2 seconds
		StartCoroutine(WaitAfterRoundFinished(2.0f));
    }

	private IEnumerator WaitAfterRoundFinished(float time){
		//stop the background music
		//play the round end music
		roundOverText.SetActive (true);
		EnablePlayerControl (false);
		GameObject.Find("CountdownManager").GetComponent<CountdownManager>().isRoundFinished = true;
		
		yield return new WaitForSeconds (time);
		DetermineGameOver();
		//disable round over text
		roundOverText.SetActive (false);
		//GameObject.FindGameObjectWithTag ("AUDIOMANAGER").GetComponent<AudioManager>().StopRoundOverSound ();
		ResetCountdown();
	}
    
    void DetermineRoundWinner()
    {
		int tempWinnerID = 0;
		float healthComparison = 0f;
		for (int i = 0; i < 2; ++i)
		{
			PlayerDefense defenseRef = defenseTriggers[i].GetComponent<PlayerDefense>();
			if (defenseRef.defenseHealthCurrent > healthComparison)
			{
				healthComparison = defenseRef.defenseHealthCurrent;
				tempWinnerID = i;
			}
		}
		PlayerDefense player0DefenseRef = defenseTriggers [0].GetComponent<PlayerDefense> ();
		PlayerDefense player1DefenseRef = defenseTriggers [1].GetComponent<PlayerDefense> ();

		if (player0DefenseRef.defenseHealthCurrent == player1DefenseRef.defenseHealthCurrent) {
			GameObject.FindGameObjectWithTag ("AUDIOMANAGER").GetComponent<AudioManager> ().PlayTieSound ();
			roundOverText.GetComponent<Text> ().text = "TIE!";
			return;
		} else {
			//GameObject.FindGameObjectWithTag ("AUDIOMANAGER").GetComponent<AudioManager> ().StopBackgroundMusic();
			GameObject.FindGameObjectWithTag ("AUDIOMANAGER").GetComponent<AudioManager> ().PlayRoundOverSound ();
		}
		
		EnableScore (tempWinnerID);
	}
	
	void EnableScore(int tempWinnerID)
	{
		// ADD ON SCORE
		if (tempWinnerID == 0)
			++runtimeVariables.player0RoundsWon;
		
		if (tempWinnerID == 1)
			++runtimeVariables.player1RoundsWon;

		// DISPLAY SCORE
		if (runtimeVariables.player0RoundsWon == 2 
		    || runtimeVariables.player1RoundsWon == 2) 
		{
			GameObject.Find ("Player" + tempWinnerID + "WinCounter1").GetComponent<Image> ().enabled = true;	
		}
		else
		{
			GameObject.Find ("Player" + tempWinnerID + "WinCounter0").GetComponent<Image> ().enabled = true;	
		}
	}
    
    void DetermineGameOver()
    {

    	if (runtimeVariables.player0RoundsWon == 2 || runtimeVariables.player1RoundsWon == 2)
			//GameObject.FindGameObjectWithTag ("AUDIOMANAGER").GetComponent<AudioManager> ().StopBackgroundMusic();
			Application.LoadLevel("EndScene");
    }
    
    void ResetPlayerPositions()
    {
		float offsetX = -(desks[0].transform.localScale.x / 2);
		for (int i = 0; i < 2; ++i)
		{
			if (i == 1)
				offsetX = -offsetX;
				
			players[i].GetComponent<PlayerMovement>().MoveToLane(0);
		}
    }
    
    void RemoveAllProjectiles()
    {
		foreach (GameObject player in players)
		{
			player.GetComponent<PlayerShooting>().RemoveAllProjectiles();
		}
    }
    
    void RemoveAllFirewalls()
    {
    	foreach (GameObject player in players)
    	{
    		player.GetComponent<PlayerBuilding>().RemoveAllFirewalls();
    	}
    }

	void ResetComputerScreens(){
		for (int i = 0; i < leftComputers.Count; i++) {
			leftComputers[i].transform.Find("ComputerScreenFirewall").transform.GetComponent<Image>().fillAmount = 0;
		}
		for (int i = 0; i < rightComputers.Count; i++) {
			rightComputers[i].transform.Find("ComputerScreenFirewall").transform.GetComponent<Image>().fillAmount = 0;
		}
	}
    
    void ResetDefense()
    {
    	foreach (GameObject trigger in defenseTriggers)
    	{
    		trigger.GetComponent<PlayerDefense>().Reset();
    	}
    }
    
    void ResetBuildTimers()
    {
    	foreach (GameObject player in players)
    	{
    		if (player.gameObject.tag == "Player")
    			player.GetComponent<PlayerControl>().ResetBuildTimer();
    			
			if (player.gameObject.tag == "NPC")
				player.GetComponent<NPCControl>().actionList.Clear();
    	}
    }
    
    void ResetCountdown()
    {
    	GameObject.Find("CountdownManager").GetComponent<CountdownManager>().Reset();
    }
    
    void ResetThreatenLists()
    {
    	for (int i = 0; i < desiredDesks; ++i)
    	{
    		player0ThreatenedByProjectileInLane[i] = false;
    		player1ThreatenedByProjectileInLane[i] = false;
    	}
    }
}


