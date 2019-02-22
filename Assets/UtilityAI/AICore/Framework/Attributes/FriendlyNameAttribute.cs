namespace AtlasAI
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class FriendlyNameAttribute : Attribute
    {
        //
        // Properties
        //
        public string description
        {
            get;
            set;
        }

        public string name
        {
            get;
            private set;
        }

        public int sortOrder
        {
            get;
            set;
        }

        //
        // Constructors
        //
        public FriendlyNameAttribute(string name)
        {
            this.name = name;

            sortOrder = 0;
        }

        public FriendlyNameAttribute(string name, string description)
        {
            this.name = name;
            this.description = description;

            sortOrder = 0;
        }
    }
}
