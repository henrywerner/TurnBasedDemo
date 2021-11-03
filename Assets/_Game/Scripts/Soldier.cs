namespace _Game.Scripts
{
    public class Soldier
    {
        public bool Team;
        public int AmmoCount, MaxAmmo;
        public int CurrentHP, MaxHP;
        public bool IsDead => CurrentHP <= MaxHP;

        public short RespawnTimer = 0;
        
        public enum Cover
        {
            NoCover = 0,
            HalfCover = 1,
            FullCover = 2
        }

        public Cover CharacterCover;
        
        public Soldier(bool team, int maxHp, int maxAmmo)
        {
            Team = team;
            MaxHP = maxHp;
            CurrentHP = maxHp;
            MaxAmmo = maxAmmo;
            AmmoCount = maxAmmo;

            CharacterCover = Cover.NoCover;
        }
    }
}