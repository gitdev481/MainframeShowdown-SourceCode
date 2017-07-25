using UnityEngine;
using System.Collections;

public class RuntimeVariables
{
	public static RuntimeVariables instance;
	public int lastPlayerToWin = 0;
	public bool isPlayer0Toggled = false;
	public bool isPlayer1Toggled = false;
	public bool isSinglePlayerToggled = false;
	
	public int player0RoundsWon = 0;
	public int player1RoundsWon = 0;
	
	public static RuntimeVariables GetInstance()
	{
		if (instance == null)
			instance = new RuntimeVariables();
		
		 return instance; 
	}
}
