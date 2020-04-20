using UnityGameFramework.Editor.DataTableTools;
using UnityGameFramework.Runtime;

namespace StarForce.Editor
{
    public abstract class ListGenericDataProcessor<T> : DataTableProcessor.GenericDataProcessor<T> where T : VarList
    {
        public sealed override bool IsSystem
        {
            get
            {
                return false;
            }
        }

        public sealed override string LanguageKeyword
        {
            get
            {
                return typeof(T).FullName;
            }
        }
    }
}