﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomization
{
	public delegate void OnValueChanged<in S, T>(S sender, T previousValue, T newValue);
	public delegate void OnValueAdded<T>(IEnumerable<T> sender, T addedItem);
	public delegate void OnValueDeleted<T>(IEnumerable<T> sender, T deletedItem);
	public class GameObjectList<T> : List<T>
	{
		public new int Capacity { get; set; } = 0;
		public bool IsLimitedCapacity { get; set; } = false;

		public event Action<GameObjectList<T>, T, T> OnElementChanged;

		public event Action<GameObjectList<T>, T> OnAddItem;
		public new bool Add(T item)
		{
			if (Count >= Capacity && IsLimitedCapacity)
			{
				return false;
			}
			else
			{
				base.Add(item);
				OnAddItem?.Invoke(this, item);

				return false;
			}
		}
	}
}
