namespace AtlasAI.Utilities
{
    using System;
    using System.Reflection;
    using System.Collections.Generic;


    public static class AIReflection
    {
        //
        // Static Methods
        //


        /// <summary>
        /// Gets a all Type instances matching the specified class name with just non-namespace qualified class name.
        /// </summary>
        /// <param name="className">Name of the class sought.</param>
        /// <returns>Types that have the class name specified. They may not be in the same namespace.</returns>
        public static Type[] GetTypeByName(string className)
        {
            List<Type> returnVal = new List<Type>();

            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] assemblyTypes = a.GetTypes();
                for (int j = 0; j < assemblyTypes.Length; j++)
                {
                    if (assemblyTypes[j].Name == className)
                    {
                        returnVal.Add(assemblyTypes[j]);
                    }
                }
            }

            return returnVal.ToArray();
        }


        public static IEnumerable<Type> GetRelevantTypes(){

            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] assemblyTypes = a.GetTypes();
                for (int j = 0; j < assemblyTypes.Length; j++){
                    if (assemblyTypes[j].GetType() == typeof(Type)){
                        yield return assemblyTypes[j];
                    }
                }
            }
        }


        ///// <summary>
        ///// Get all available classes of type.
        ///// </summary>
        ///// <returns> All available options. </returns>
        ///// <typeparam name="T"> Is one the AI blocks. </typeparam>
        //public static List<Type> GetAllOptions<T>() //where T: Selector, IQualifier, IAction, IContextualScorer
        //{
        //    List<Type> availableTypes = new List<Type>();
        //    Type type = typeof(T);
        //    //  Gets all custom Types in this assembly and adds it to a list.
        //    //var optionTypes = Assembly.GetAssembly(type).GetTypes()
        //    //.Where(t => t.IsClass && t.Namespace == typeof(AIManager).Namespace)
        //    //.ToList();
        //    var optionTypes = Assembly.GetAssembly(type).GetTypes()
        //                              .Where(t => t.IsClass).ToList();

        //    //optionTypes.ForEach(t => Debug.Log(t));

        //    foreach (Type option in optionTypes)
        //    {
        //        if (option.BaseType == type)
        //            availableTypes.Add(option);
        //    }
        //    //availableTypes.ForEach(t => Debug.Log(t));
        //    return availableTypes;
        //}
    }
}
