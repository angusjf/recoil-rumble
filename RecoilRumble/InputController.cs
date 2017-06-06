using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace RecoilRumble
{
	public class InputController : Updatable
	{
		private Dictionary<Keys, bool> heldKeys, hitKeys;

		private KeyboardState oldState;

		private Keys [] usedKeys = {
			Keys.Up,
			Keys.Down,
			Keys.Enter,
			Keys.W, Keys.A, Keys.S, Keys.D, Keys.Space
		};

		public InputController ()
		{
			heldKeys = new Dictionary<Keys, bool> (usedKeys.Length);
			hitKeys = new Dictionary<Keys, bool> (usedKeys.Length);
		}

		public void Update ()
		{
			KeyboardState newState = Keyboard.GetState ();
			foreach (Keys k in usedKeys) {
				heldKeys[k] = newState.IsKeyDown (k);
				hitKeys [k] = !oldState.IsKeyDown (k) && newState.IsKeyDown (k);
			}
			oldState = newState;
		}

		public bool GetKey (Keys k) {
			return heldKeys.ContainsKey (k) && heldKeys [k];
		}

		public bool GetKey (params Keys [] keys)
		{
			bool ret = true;
			foreach (Keys k in keys) {
				ret = ret && heldKeys.ContainsKey (k) && heldKeys [k];
			}
			return ret;
		}

		public bool GetKeyDown (Keys k)
		{
			return hitKeys.ContainsKey(k) && hitKeys[k];
		}
	}
}
