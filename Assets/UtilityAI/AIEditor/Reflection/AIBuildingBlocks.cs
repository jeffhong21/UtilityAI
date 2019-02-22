using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AtlasAI.AIEditor
{
    
    public sealed class AIBuildingBlocks
    {
        //
        // Static Fields
        //
        private static readonly object _instanceLock;
        private static AIBuildingBlocks _instance;

        //
        // Fields
        //
        private List<NamedType> _selectors;
        private List<NamedType> _qualifiers;
        private List<NamedType> _actions;
        private Dictionary<Type, List<NamedType>> _customItems;


        //
        // Static Properties
        //
        public static AIBuildingBlocks Instance{
            get{
                if (_instance == null) 
                    _instance = new AIBuildingBlocks();
                return _instance; }
        }

        //
        // Properties
        //
        public IEnumerable<NamedType> actions{
            get{
                foreach (var item in _actions){
                    yield return item;
                }
            }
        }

        public IEnumerable<NamedType> qualifiers{
            get{
                foreach (var item in _qualifiers){
                    yield return item;
                }
            }
        }

        public IEnumerable<NamedType> selectors{
            get{
                foreach (var item in _selectors){
                    yield return item;
                }
            }
        }


        //
        // Constructors
        //
        private AIBuildingBlocks()
        {
            _instance = this;
            _selectors = new List<NamedType>();
            _qualifiers = new List<NamedType>();
            _actions = new List<NamedType>();
            _customItems = new Dictionary<Type, List<NamedType>>();
        }



        //
        // Static Methods
        //
        private static IEnumerable<Type> GetConstructableTypes()
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<Type> ReflectDerivatives(Type forType, IEnumerable<Type> relevantTypes)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<Type> ReflectDerivatives(Type forType)
        {
            throw new NotImplementedException();
        }

        private static List<NamedType> Wrap(IEnumerable<Type> types)
        {
            throw new NotImplementedException();
        }

        //
        // Methods
        //
        public IEnumerable<NamedType> GetForType(Type t)
        {
            //  Gets all custom Types in this assembly and adds it to a list.

            //var optionTypes = Assembly.GetExecutingAssembly().GetTypes().
                                       //Where(type => type.IsClass && t.IsAssignableFrom(type));

            var optionTypes = AppDomain.CurrentDomain.GetAssemblies().
                                       SelectMany(s => s.GetTypes()).
                                       Where(type => type.IsClass && t.IsAssignableFrom(type));

            //var optionTypes = System.Reflection.Assembly.GetAssembly(t).GetTypes()
                                    //.Where(type => type.IsClass && type.IsAssignableFrom(t))
                                    //.ToList();



            //  Go through each item in list and check if its base class equals the Types we defined.
            foreach (Type type in optionTypes)
            {
                if (Attribute.GetCustomAttribute(type, typeof(FriendlyNameAttribute)) != null)
                {
                    var namedType = new NamedType(type);
                    namedType.friendlyName = type.GetCustomAttribute<FriendlyNameAttribute>().name;
                    namedType.description = type.GetCustomAttribute<FriendlyNameAttribute>().description;
                    yield return namedType;
                }
            }
        }


        private void Init()
        {
            
        }


        //
        // Nested Types
        //
        public class NamedType
        {
            public string friendlyName
            {
                get;
                internal set;
            }

            public string description
            {
                get;
                internal set;
            }

            public Type type
            {
                get;
                internal set;
            }

            public NamedType(Type t){
                type = t;
                friendlyName = t.Name;
            }
        }
    }
}
