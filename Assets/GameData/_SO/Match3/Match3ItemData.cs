using UnityEngine;

[CreateAssetMenu(menuName = "Game/Match3/Match3ItemData", fileName = "Match3Item", order = 12)]
public class Match3ItemData : ScriptableObject
{
	public ItemViewData view;
	public RewardData[] reward;
}