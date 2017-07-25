using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToggleManager : MonoBehaviour 
{
	RuntimeVariables runtimeVariables;

    private Toggle player0Toggle;
    private Toggle player1Toggle;
    
    private Image player0Controls;
    private Image player1Controls;
    
    public Sprite p0_L;
    public Sprite p0_R;
    public Sprite p1_L;
    public Sprite p1_R;
        
    void Awake()
    {
		runtimeVariables = RuntimeVariables.GetInstance();
		
		player0Toggle = GameObject.Find("Player0Toggle").GetComponent<Toggle>();
		player1Toggle = GameObject.Find("Player1Toggle").GetComponent<Toggle>();

		player0Toggle.isOn = runtimeVariables.isPlayer0Toggled;
		player1Toggle.isOn = runtimeVariables.isPlayer1Toggled;
		
		player0Controls = player0Toggle.GetComponentInChildren<Image>();
		player1Controls = player1Toggle.GetComponentInChildren<Image>();
		
		InvokeRepeating ("UpdateRuntimeVariables", 0.2f, 0.2f);
    }
    
	void UpdateRuntimeVariables()
	{
    	runtimeVariables.isPlayer0Toggled = player0Toggle.isOn;
		runtimeVariables.isPlayer1Toggled = player1Toggle.isOn;
		
		UpdateImages();
	}
	
	void UpdateImages()
	{
		if (runtimeVariables.isPlayer0Toggled)
			player0Controls.sprite = p0_L;
		else
			player0Controls.sprite = p0_R;
		
		if (runtimeVariables.isPlayer1Toggled)
			player1Controls.sprite = p1_L;
		else
			player1Controls.sprite = p1_R;
	}
}
