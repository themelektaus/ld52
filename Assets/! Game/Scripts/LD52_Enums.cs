namespace Prototype
{
    public enum Ability
    {
        MoveSpeed,
        HarvestRadius, HarvestStrength,
        ShootSpeed, ShootDamage,
        CarryingCapacity
    }

    public enum GameOverState
    {
        Canceled = 0,
        Failed = 1,
        Victory = 2
    }
}