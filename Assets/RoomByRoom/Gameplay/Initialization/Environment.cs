using System.IO;
using RoomByRoom.Utility;

namespace RoomByRoom.Initialization
{
	public static class Environment
	{
		public static void Prepare()
		{
			if (!Directory.Exists(Idents.FilePaths.SavingDirectory))
			{
				Directory.CreateDirectory(Idents.FilePaths.SavingDirectory);
			}
		}
	}
}