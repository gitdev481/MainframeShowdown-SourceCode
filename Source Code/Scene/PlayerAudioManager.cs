using UnityEngine;
using System.Collections;

public class PlayerAudioManager : MonoBehaviour {

	public GameObject firewallBuildingSound;
	public GameObject firewallPlacedSound;
	public GameObject shootVirusSound;
	public GameObject cannotPlaceFirewallSound;
	public GameObject virusHitComputerSound;


	public void PlayFirewallBuildingSound(){
		if (!firewallBuildingSound.GetComponent<AudioSource>().isPlaying)
			firewallBuildingSound.GetComponent<AudioSource>().Play();
			
		firewallBuildingSound.GetComponent<AudioSource> ().volume = 1;
	}
	
	public void StopFirewallBuildingSound()
	{
		firewallBuildingSound.GetComponent<AudioSource> ().volume = 0;
	}


	public void PlayFirewallPlacedSound(){
		firewallPlacedSound.GetComponent<AudioSource> ().Play ();
	}

	public void PlayShootVirusSound(){
		shootVirusSound.GetComponent<AudioSource> ().Play ();
	}

	public void PlayCannotPlaceFirewallSound(){
		cannotPlaceFirewallSound.GetComponent<AudioSource> ().Play ();
	}

	public void PlayVirusHitComputerSound(){
		virusHitComputerSound.GetComponent<AudioSource> ().Play ();
	}

}
