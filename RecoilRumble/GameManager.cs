using RecoilRumble.Game;
namespace RecoilRumble
{
	public class GameManager : Updatable
	{
		public GameRound CurrentRound { get; private set; }

		public GameManager ()
		{

		}

		public void Update ()
		{
			if (CurrentRound != null) {
				
			}
		}

		public void NewRound (int mapNumber)
		{
			CurrentRound = new GameRound ();
		}
	}
}