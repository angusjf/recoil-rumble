﻿#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#if MONOMAC
using MonoMac.AppKit;
using MonoMac.Foundation;
#elif __IOS__ || __TVOS__
using Foundation;
using UIKit;
#endif
#endregion

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
