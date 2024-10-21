using UnityEngine;

[CreateAssetMenu(menuName = "Game/Common/GameStatsStartData", fileName = "GameStatsStart", order = 5)]
public class GameStatsStartData : ScriptableObject
{
	[Space]
	public int energy;
	public int coins;
	public int oil;
	public int spareParts;

	[Space]
	public int level;
	public int points;
}