using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBuilding : MonoBehaviour 
{
	public List<List<GameObject>> firewalls = new List<List<GameObject>>();
    public GameObject firewallRef;
    public int maxFirewalls = 8;
    public int currentFirewalls = 0;
    
    public  int MAX_FIREWALLS_PER_LANE = 3;
    private float BASE_OFFSET_X = 2.5f;
    private float FIREWALL_SPACING = 0.5f;

	public bool stopPlayingBuildingSound1 = false;
	public bool stopPlayingBuildingSound2 = false;
    
	void Start() 
	{
        CreateLists();
	}
    
    void CreateLists()
    {
        int numDesks = GameObject.Find("SceneManager").GetComponent<SceneManager>().DesksCount;
        for (int i = 0; i < numDesks; ++i)
        {
            firewalls.Add(new List<GameObject>());
        }
    }
    
    public void HandleBuild(GameObject player, int playerID, int currentLane)
    {
        CountFirewalls();
        PlaceFirewall(player, playerID, currentLane);
    }
    
    void CountFirewalls()
    {
        currentFirewalls = 0;
        foreach (var list in firewalls)
        {
            currentFirewalls += list.Count;
        }
    }
	public int FirewallsInLane (int currentLane){
		return firewalls [currentLane].Count;
	}

    void PlaceFirewall(GameObject player, int playerID, int currentLane)
	{
        if (currentFirewalls == maxFirewalls) {
			gameObject.GetComponent<PlayerControl> ().audioManager.PlayCannotPlaceFirewallSound ();
			stopPlayingBuildingSound2 = true;
			return;
		}
		else{
			stopPlayingBuildingSound2 = false;
		}
            
        int numFirewalls = firewalls[currentLane].Count;
        if (numFirewalls == MAX_FIREWALLS_PER_LANE) {
			stopPlayingBuildingSound1 = true;
			gameObject.GetComponent<PlayerControl> ().audioManager.PlayCannotPlaceFirewallSound ();
			return;
		} else {
			stopPlayingBuildingSound1 = false;
		}
        
        float offsetX = BASE_OFFSET_X + (numFirewalls * FIREWALL_SPACING);
        if (playerID == 1)
            offsetX = -offsetX;
		transform.Find("PlayerAudioManager").gameObject.GetComponent<PlayerAudioManager> ().PlayFirewallPlacedSound ();
        GameObject newFirewall = Instantiate(firewallRef);
        newFirewall.name = "Player" + playerID + "Firewall";
        newFirewall.transform.position = new Vector2(gameObject.transform.position.x + offsetX, gameObject.transform.position.y);
        
        firewalls[currentLane].Add(newFirewall);
    }
    
    public void RemoveFirewall(GameObject wall, int lane)
    {
        firewalls[lane].Remove(wall);        
        Destroy(wall);
    }
    
	public void RemoveAllFirewalls()
	{
		foreach (List<GameObject> list in firewalls)
		{
			foreach (GameObject wall in list)
			{
				Destroy(wall);
			}
			
			list.Clear();
		}
	}
}
