using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class PlayerDefense : MonoBehaviour 
{
	public float defenseHealthMax = 100f;
	public float defenseHealthCurrent;
	private SceneManager sceneManager;
	private float projectileDamage = 0f;
	public int hitsTaken = 0;


	void Start() 
	{
		sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
		
		Reset();
	}

	public void Reset()
	{
		projectileDamage = defenseHealthMax / (sceneManager.DesksCount * 2);
		defenseHealthCurrent = defenseHealthMax;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag != "Projectile")
			return;

		int projectileID = other.gameObject.GetComponent<Projectile>().PlayerID;
		GameObject.Find("Player" + projectileID).GetComponent<PlayerShooting>().RemoveProjectile(other.gameObject);
		
		if (gameObject.name == "Player1Defense")
		{
			if (RuntimeVariables.GetInstance().isSinglePlayerToggled)
			{
				GameObject.Find("Player" + projectileID).GetComponent<PlayerControl>().audioManager.PlayVirusHitComputerSound();
			}
			else if (!RuntimeVariables.GetInstance().isSinglePlayerToggled)
			{
				GameObject.Find("Player" + projectileID).GetComponent<PlayerControl>().audioManager.PlayVirusHitComputerSound();
			}
			
		}
		else if (gameObject.name == "Player0Defense")
		{
			if (RuntimeVariables.GetInstance().isSinglePlayerToggled)
			{
				GameObject.Find("Player" + projectileID).GetComponent<NPCControl>().audioManager.PlayVirusHitComputerSound();
			}
			else if (!RuntimeVariables.GetInstance().isSinglePlayerToggled)
			{
				GameObject.Find("Player" + projectileID).GetComponent<PlayerControl>().audioManager.PlayVirusHitComputerSound();
			}
		}
		if (gameObject.name == "Player0Defense") {
			ChangeServerSprites(GameObject.Find("SceneManager").GetComponent<SceneManager>().leftServers, hitsTaken);

		} else if (gameObject.name == "Player1Defense") {
			ChangeServerSprites(GameObject.Find("SceneManager").GetComponent<SceneManager>().rightServers, hitsTaken);
		}
		defenseHealthCurrent -= projectileDamage;
		hitsTaken++;
		if (defenseHealthCurrent <= 0)
		{
            RuntimeVariables.GetInstance().lastPlayerToWin = projectileID;
            sceneManager.EndOfRound();
		}
	}
	void ChangeServerSprites(List<GameObject> list, int serverNumber){
		list [serverNumber].gameObject.GetComponent<Image> ().sprite = GameObject.Find ("SceneManager").GetComponent<SceneManager> ().serverExploded;
	}


}
