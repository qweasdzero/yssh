using System.Collections.Generic;

namespace UnityGameFramework.Editor
{
	public interface IPreferenceProvider
	{
		void SetKeyValue(string valueName, object value);
		void FetchKeyValues(IDictionary<string, object> prefsLookup);
		object ValueField(string valueName, object value);
	}
}