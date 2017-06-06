using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RecoilRumble.Game
{
	public class PhysicsGameObject : GameObject
	{
		protected Vector2 velocity;
		protected Vector2 acceleration;
		protected bool canMove;

		public PhysicsGameObject (Vector2 position, Texture2D texture) : base(position, texture)
		{
			
		}

		public override void Update ()
		{
			this.velocity += acceleration;
			this.position += velocity;
			base.Update ();
		}

		protected void AddVelocity (Vector2 v)
		{
			this.velocity += v;
		}

		protected void Stop ()
		{
			this.velocity = Vector2.Zero;
		}

		protected void AddAcceleration (Vector2 a)
		{
			this.acceleration += a;
		}

		protected bool IsOnGround ()
		{
			return false;
		}

		protected void SetPosition (Vector2 pos)
		{
			this.position = pos;
		}
	}
}
