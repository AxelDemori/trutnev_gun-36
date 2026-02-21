namespace Netologia.TowerDefence
{
	[System.Flags]
	public enum ElementalType : byte
	{
		Physic = 0,
		Fire = 1 << 1,
		Ice = 1 << 2,
		Earth = 1 << 3,
        PhysicAir = 1 << 4,
        FireAir = 1 << 5,
        IceAir = 1 << 6,
        EarthAir = 1 << 7,
    }
}