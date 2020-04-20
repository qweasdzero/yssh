using System;
using System.Collections.Generic;

namespace SG1
{
	internal static class UIEffectListPool<T>
	{
		private static readonly UiEffectObjectPool<List<T>> s_ListUiEffectObjectPool = new UiEffectObjectPool<List<T>>(null, l => l.Clear());

		public static List<T> Get()
		{
			return s_ListUiEffectObjectPool.Get();
		}

		public static void Release(List<T> element)
		{
			s_ListUiEffectObjectPool.Release(element);
		}
	}
	internal class UiEffectObjectPool<T> where T : new()
	{
		private readonly Stack<T> _stack = new Stack<T>();

		private readonly Action<T> _actionOnGet;

		private readonly Action<T> _actionOnRecycle;

		public int count { get; private set; }
		public int activeCount { get { return count - inactiveCount; } }
		public int inactiveCount { get { return _stack.Count; } }

		public UiEffectObjectPool(Action<T> actionOnGet, Action<T> actionOnRecycle)
		{
			_actionOnGet = actionOnGet;
			_actionOnRecycle = actionOnRecycle;
		}

		public T Get()
		{
			T element;
			if (_stack.Count == 0)
			{
				element = new T();
				count++;
			}
			else
			{
				element = _stack.Pop();
			}
			if (_actionOnGet != null)
				_actionOnGet(element);
			return element;
		}

		public void Release(T element)
		{
			if (_stack.Count > 0 && ReferenceEquals(_stack.Peek(), element))
			{
				throw new Exception("Internal error. Trying to destroy object that is already released to pool.");
			}

			if (_actionOnRecycle != null)
			{
				_actionOnRecycle(element);
			}
			_stack.Push(element);
		}

		public void Clear()
		{
			_stack.Clear();
			count = 0;
		}
	}
}
