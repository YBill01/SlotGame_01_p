using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Match3/Match3ConfigData", fileName = "Match3Config", order = 11)]
public class Match3ConfigData : ScriptableObject
{
	public Vector2Int size;

	public Item[] items;

	[Space]
	public float rewardMultiplier;

	[Space]
	public float gameTime;
	public float cooldown;

	[Serializable]
	public struct Item
	{
		public Match3ItemData item;
		[Range(0, 1)]
		public float probability;
	}
}