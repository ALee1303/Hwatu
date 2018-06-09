namespace Hwatu.Card
{
    public enum TtiType { Hong, Chung, Cho }
    public abstract class Tti : Hanafuda
    {
        TtiType dan;

        public TtiType Dan { get => dan; }

        protected Tti(Month _month, TtiType _dan) : base(_month, CardType.Tti)
        {
            dan = _dan;
        }
        //helper override
        protected override bool isEqual(Hanafuda other)
        {
            if (base.isEqual(other)
                && this.dan == ((Tti)other).Dan)
                return true;
            return false;
        }
    }
}
