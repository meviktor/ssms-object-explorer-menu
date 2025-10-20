namespace SSMSObjectExplorerMenu.advancedfiltering.models
{
    internal abstract class PropertyBase
    {
        internal bool IsDefined { get; private set; }

        protected PropertyBase(bool isDefined) => IsDefined = isDefined;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;

            if (obj == null || GetType() != obj.GetType()) return false;

            var other = (PropertyBase)obj;
            return IsDefined == other.IsDefined;
        }

        public override int GetHashCode() => IsDefined.GetHashCode();
    }
}
