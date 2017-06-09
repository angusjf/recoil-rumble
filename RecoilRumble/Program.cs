using System;
using System.Collections.Generic;
using System.Linq;

namespace RecoilRumble
{
	static class Program
	{
		private static Engine game;

		internal static void RunGame ()
		{
			game = new Engine ();
			game.Run ();
		}

		static void Main (string [] args)
		{
			RunGame ();
		}

	}

}
