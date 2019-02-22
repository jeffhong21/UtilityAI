namespace AtlasAI
{
    using System;
    using System.Runtime.CompilerServices;


    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class AICategoryAttribute : Attribute
    {
        //
        // Properties
        //
        public string name
        {
            [CompilerGenerated]
            get;
            [CompilerGenerated]
            private set;
        }

        public int sortOrder
        {
            get;
            private set;
        }

        //
        // Constructors
        //
        public AICategoryAttribute(string name)
        {
            this.name = name;
        }

        public AICategoryAttribute(string name, int sortOrder)
        {
            this.name = name;
            this.sortOrder = sortOrder;
        }
    }
}
