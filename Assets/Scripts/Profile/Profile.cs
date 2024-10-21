using BayatGames.SaveGameFree;
using BayatGames.SaveGameFree.Serializers;
using System;
using System.Collections.Generic;

namespace SlotGame.Profile
{
	public class Profile : IDisposable
	{
		private static Profile _instance;
		public static Profile Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new Profile();
				}

				return _instance;
			}
		}

		private Dictionary<Type, IProfileController> _profileDic;

		public Profile()
		{
			SaveGame.Serializer = new SaveGameBinarySerializer();
			SaveGame.SavePath = SaveGamePath.PersistentDataPath;

			_profileDic = new Dictionary<Type, IProfileController>();

			_profileDic.Add(typeof(AppData), new ProfileController<AppData>("_app.dat"));
			_profileDic.Add(typeof(PlayerData), new ProfileController<PlayerData>("_player.dat"));
			//...
		}

		public ProfileController<T> Get<T>() where T : class, IProfileData, new()
		{
			if (_profileDic.TryGetValue(typeof(T), out IProfileController profileData))
			{
				return (ProfileController<T>)profileData;
			}

			return default;
		}

		public void Dispose()
		{
			_instance = null;
		}
	}
}