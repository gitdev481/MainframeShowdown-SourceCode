using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public GameObject roundOverSound;
	public GameObject virusHitVirusSound;
	public GameObject virusHitFirewallSound;
	public GameObject getReadySound;
	public GameObject tieSound;
	public GameObject hackSound;
	public GameObject backgroundMusic;
	public GameObject virusHitPlayer;


	public static AudioManager instance;

	public void PlayRoundOverSound(){
		roundOverSound.GetComponent<AudioSource> ().Play ();
	}
	public void StopRoundOverSound(){
		roundOverSound.SetActive (false);
	}
	public void PlayVirusHitVirusSound(){
		virusHitVirusSound.GetComponent<AudioSource> ().Play ();
	}
	public void PlayVirusHitFirewallSound(){
		virusHitFirewallSound.GetComponent<AudioSource> ().Play ();
	}

	public void PlayGetReadySound(){
		getReadySound.GetComponent<AudioSource> ().Play ();
	}
	public void PlayTieSound(){
		tieSound.GetComponent<AudioSource> ().Play ();
	}
	public void PlayHackSound(){
		hackSound.GetComponent<AudioSource> ().Play ();
	}

	public void PlayBackgroundMusic(){
		backgroundMusic.SetActive (true);
	}
	public void StopBackgroundMusic(){
		backgroundMusic.GetComponent<AudioSource> ().Stop ();
	}

	public void PlayVirusHitPlayer(){
		virusHitPlayer.GetComponent<AudioSource> ().Play ();
	}





}
