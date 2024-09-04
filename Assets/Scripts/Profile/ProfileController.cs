using BayatGames.SaveGameFree;
using System.IO;

namespace Game.Profile
{
	public class ProfileController<T> : IProfileController where T : class, IProfileData, new()
	{
		public int indexData = 0;
		public string identifier = "_save.dat";

		public T data { get; private set; }

		public ProfileController(string identifier)
		{
			this.identifier = identifier;
		}

		public T Save()
		{
			SaveGame.Save(GetIdentifier(), data);

			return data;
		}
		public T Load()
		{
			data = SaveGame.Load(GetIdentifier(), Clear());

			return data;
		}

		public T Clear()
		{
			if (data != null)
			{
				data.SetDefault();
			}
			else
			{
				data = new T();
			}

			return data;
		}

		public string GetIdentifier()
		{
			return $"{Path.GetFileNameWithoutExtension(identifier)}{indexData:D4}{Path.GetExtension(identifier)}";
		}
	}
}