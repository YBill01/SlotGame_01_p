using System;
using UnityEngine.Device;

namespace SlotGame.Profile
{
	[Serializable]
	public class AppData : IProfileData
	{
		public DateTime firstEntryDate;
		public DateTime lastEntryDate;

		private string _name;

		public bool musicOn;
		public bool soundOn;

		public bool firstPlay;

		public AppData()
		{
			SetDefault();
		}

		public string Name
		{
			get => _name;
			set
			{
				if (value.Length < 1)
				{
					_name = SystemInfo.deviceName;
				}
				else
				{
					_name = value;
				}
			}
		}

		public void SetDefault()
		{
			firstEntryDate = DateTime.UtcNow;
			lastEntryDate = DateTime.UtcNow;

			_name = SystemInfo.deviceName;

			musicOn = true;
			soundOn = true;

			firstPlay = true;
		}
	}
}