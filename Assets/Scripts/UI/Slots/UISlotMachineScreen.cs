using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlotMachineScreen : UIScreen
{
	public SlotStates SlotState { get; private set; }

	public enum SlotStates
	{
		None,
		Ready,
		Running,
		Skip,
		Finish
	}


	[SerializeField]
	private SlotConfigData m_config;



	protected override void OnInit()
	{
		

	}


	public void Init()
	{
		if(SlotState != SlotStates.None)
		{
			return;
		}

		SlotState = SlotStates.Ready;



	}


	public void SpinStart()
	{

	}

	public void SpinStop()
	{

	}

}