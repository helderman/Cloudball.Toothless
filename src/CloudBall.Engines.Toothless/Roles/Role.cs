namespace CloudBall.Engines.Toothless.Roles
{
	public static class Role
	{
		public static IRole BallOwner = new BallOwner();
		public static IRole Pickup = new Pickup();
		public static IRole BallOwnerTackler = new BallOwnerTackler();
		public static IRole CatchUp = new CatchUp();
		public static IRole Keeper = new Keeper();
		public static IRole Sweeper = new Sweeper();
	}
}
