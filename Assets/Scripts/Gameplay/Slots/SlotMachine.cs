public class SlotMachine
{
	public SlotStates State { get; private set; }

	public enum SlotStates
	{
		None,
		Ready,
		Running,
		Skip,
		Finish
	}

	public SlotReel[] slotReels { get; private set; }


	private SlotConfigData _data;



	public void Init(SlotConfigData configData)
	{
		_data = configData;

		//_data.levels[0].reel.items[0].reward.items[0].

		/*slotReels = new SlotReel[_data.numReels];

		for (int i = 0; i < slotReels.Length; i++)
		{
			slotReels[i] = new SlotReel();



			//slotReels[i].OnStart();
		}*/



	}



}