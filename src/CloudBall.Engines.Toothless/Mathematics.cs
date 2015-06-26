using Common;
using System;

namespace CloudBall.Engines.Toothless
{
	public static class Mathematics
	{
		/// <summary>Gets the angle between this and the other velocity.</summary>
		public static Single GetAngle(Vector v0, Vector v1)
		{
			var a = v0.X * v1.X + v0.Y * v1.Y;
			var b = v0.Length * v1.Length;
			return (Single)Math.Acos(a / b);
		}
	}
}
