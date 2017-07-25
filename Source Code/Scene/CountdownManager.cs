using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountdownManager : MonoBehaviour 
{
	private SceneManager sceneManager;
    private Text countdownText;
    private AudioSource source;
    public AudioClip beep;
    public AudioClip beepEnd;
    private bool isCountdownFinished = false;
    public bool isRoundFinished = false;
    
    public float countdownFrom = 3f;
    private float currentTime = 0f;
    
    public float roundTime = 60f;
    private float timer;


        
	void Start() 
    {
    	sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
	    countdownText = GameObject.Find("CountdownText").GetComponent<Text>();
        source = GetComponent<AudioSource>();
        
		Reset();
	}
	
	void Update() 
    {
    	if (isRoundFinished)
    		return;
    		
    	if (!isCountdownFinished)
    	{
        	UpdateCountdownText();
        	return;
        }
        
		if (timer > 0)
		{
			timer -= Time.deltaTime;
		} else {
			isRoundFinished = true;
			sceneManager.EndOfRound();
		}
			
    	UpdateRoundText();
	}
    
    void UpdateCountdownText()
    {
        countdownText.text = currentTime.ToString();
        countdownText.color = currentTime <= 5.0f && Mathf.Floor(currentTime) % 2 != 0 ? Color.red : Color.black;
    }
    
    void UpdateRoundText()
    {
		//countdownText.text = timer.ToString("f0");
		countdownText.text = ((int)timer).ToString ();
		countdownText.color = Color.red;
    }
    
    private IEnumerator Countdown(float _time)
    {

        yield return new WaitForSeconds(1.0f);
        
        if (_time <= 0)
        {
			source.PlayOneShot(beepEnd, 0.1f);
            countdownText.color = Color.white;
			GameObject.FindGameObjectWithTag ("AUDIOMANAGER").GetComponent<AudioManager> ().PlayHackSound();
			GameObject.FindGameObjectWithTag ("AUDIOMANAGER").GetComponent<AudioManager> ().PlayBackgroundMusic();
            GameObject.Find("SceneManager").GetComponent<SceneManager>().EnablePlayerControl(true);
            
            isCountdownFinished = true;
        }
        else if (_time <= 5.0f)
        {
            source.PlayOneShot(beep, 0.5f);

        }
        
        if (_time > 0) 
        {
            --_time; 
            currentTime = _time; 
            StartCoroutine(Countdown(currentTime));
        }
    }
    
    public void Reset()
    {
		GameObject.FindGameObjectWithTag ("AUDIOMANAGER").GetComponent<AudioManager> ().PlayGetReadySound ();
    	sceneManager.EnablePlayerControl(false);
		sceneManager.ResetServers ();
		sceneManager.ResetComputerFill (sceneManager.leftComputers);
		sceneManager.ResetComputerFill (sceneManager.rightComputers);
    	isCountdownFinished = false;
    	isRoundFinished = false;
		timer = roundTime;
		currentTime = countdownFrom;
		
		StartCoroutine(Countdown(currentTime));
    }
}
