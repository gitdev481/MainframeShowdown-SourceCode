using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShooting : MonoBehaviour 
{
	public GameObject projectile;
	public List<GameObject> projectiles;
    public float projectileSpeed = 0.25f;
    public float offsetFromPlayer = 1f;

	private Quaternion playerOneSpawnRot = new Quaternion(180.0f, 180.0f, 0.0f, 0.0f);
	private Quaternion playerTwoSpawnRot = new Quaternion(217.0f, -217.0f, 0.0f, 0.0f);
	
	private SceneManager sceneManager;
	private PlayerProperties playerProperties;
    
    void Start()
    {
    	sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
		playerProperties = GetComponent<PlayerProperties>();
		
        if (playerProperties.playerID == 1) 
        {
            projectileSpeed = -projectileSpeed;
            offsetFromPlayer = -offsetFromPlayer;
        }
    }
    
    void Update()
    {
    	for (int i = 0; i < projectiles.Count; ++i)
    	{
    		if (playerProperties.playerID == 0)
    		{
    			sceneManager.player1ThreatenedByProjectileInLane[projectiles[i].GetComponent<Projectile>().currentLane] = true;
    		} else {
				sceneManager.player0ThreatenedByProjectileInLane[projectiles[i].GetComponent<Projectile>().currentLane] = true;
    		}
    	}
    }
	
	public void ShootProjectile(GameObject player, int playerID)
	{
        if (projectiles.Count >= 3)
            return;

		// Depending on the playerID change the rotation of the projectile
		if (RuntimeVariables.GetInstance().isSinglePlayerToggled && playerID == 1)
			gameObject.GetComponent<NPCControl> ().audioManager.PlayShootVirusSound ();
		else
			gameObject.GetComponent<PlayerControl> ().audioManager.PlayShootVirusSound ();
			
        GameObject projectileInstance = (GameObject)Instantiate(projectile, new Vector3(player.transform.position.x + offsetFromPlayer, player.transform.position.y, player.transform.position.z), player.transform.rotation);
		projectileInstance.name = "Player" + playerID + "Projectile";
        projectileInstance.GetComponent<Projectile>().SetProperties(playerID, projectileSpeed, player.GetComponent<PlayerMovement>().CurrentLane);
		
		if (playerID == 0)
			projectileInstance.transform.rotation = playerOneSpawnRot;
		if(playerID == 1)
			projectileInstance.transform.rotation = playerTwoSpawnRot;

		projectiles.Add(projectileInstance);
	}

	public void RemoveProjectile(GameObject projectile)
	{
		if (playerProperties.playerID == 0)
		{
			sceneManager.player1ThreatenedByProjectileInLane[projectile.GetComponent<Projectile>().currentLane] = false;
		} else {
			sceneManager.player0ThreatenedByProjectileInLane[projectile.GetComponent<Projectile>().currentLane] = false;
		}
		
		projectiles.Remove(projectile);
		Destroy(projectile);
	}
	
	public void RemoveAllProjectiles()
	{
		foreach (GameObject projectile in projectiles)
		{
			Destroy(projectile);
		}
		
		projectiles.Clear();
	}
}
