﻿using LCGuidebook;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System;

namespace LCGuidebook.Core
{
	public abstract class Region
	{
		public Region(string name)
		{
			Name = name;
		}
		public string Name { get; set; }
		public List<Region> Neighbors { get; internal set; }
	}

	/// <summary>
	///		Defining the behaviors of all waters. 
	/// </summary>
	public class Waters : Region
	{
		public Waters(string name) : base(name) { }
		// The number of international waters in this game is set to be 5
		public const int NUM_INTL_WATERS = 5;
		// Each sea belongs to a certain nation. 
		// if the property is null, then it is international water
		public Nation Sovereign { get; set; }
	}

	/// <summary>
	///		The base class of all nations, including regular nation and superpower
	/// </summary>
	public abstract class Nation : Region
	{
		public const int NUM_VALUES = 11;
		public Nation(string name) : base(name)
		{
			Inclination = new DynamicDictionary<Nation, double>(valueRestriction: d => d <= 1 && d >= -1);

			//// Initialize its territorial water, and name the water as well
			//TerritorialSea = new Waters() { Name = ResourceManager.WatersNames.Dequeue() };
			//TerritorialSea.Sovereign = this;
			//ResourceManager.Regions.Add(TerritorialSea);
		}

		protected readonly Dictionary<string, IUniqueObject> _Reserve = new Dictionary<string, IUniqueObject>();

		public readonly ValueComplexNTuple NationalIndices = new ValueComplexNTuple();
		public readonly ValueComplexNTuple OutdatedNationalIndices = new ValueComplexNTuple();
		// null stands for independence
		public DynamicDictionary<Nation, double> Inclination { get; }

		public List<Waters> TerritorialSea { get; } = new List<Waters>();

		public double Tactics { get; set; }

		public double AdjustedBureaucracyIndex => 0.1 * Math.Pow(1.2, NationalIndices[MainIndexType.Bureaucracy].CurrentValue);

		public TaskSequence TaskSequence { get; } = new TaskSequence();

		// considering replace this data structure with command group later
		[Obsolete]
		public TechTreeNode TechTree { get; }

		public ActionGroup Operations { get; }
		public ActionGroup Intelligence { get; }
		public ActionGroup Defence { get; }
		public ActionGroup Technology { get; }
		public ActionGroup Domestic { get; }
		public ActionGroup Diplomacy { get; }

		public void UpdateValue(int nationalIndex)
		{
			Census census = new Census("Generating statistics", nationalIndex);
			ResourceManager.Me.TaskSequence.AddNewTask(census);
		}
	}

	public class RegularNation : Nation
	{
		public RegularNation(string name) : base(name)
		{
			
		}
	}
}
