﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Atomization
{
	/// <summary>
	/// Interaction logic for DeployNuke_Window.xaml
	/// </summary>
	public partial class DeployNuke_Window : Window
	{
		public Nuclear ParentPage;

		private NuclearWeapon selectedWeapon = null;
		private bool isNewNuke = true;
		public DeployNuke_Window(Nuclear parentPage)
		{
			InitializeComponent();

			ParentPage = parentPage;

			Closing += DeployNuke_Window_Closing;
			SelectionList.ItemsSource = Data.Me.NuclearPlatforms;

			Target.Items.Clear();
			Data.Regions.ForEach(
				r => Target.Items.Add(
					new ComboBoxItem
					{
						Content = r.Name,
						Padding = new Thickness(3)
					}
				)
			);

			if ((selectedWeapon = ParentPage.NukeList.SelectedItem as NuclearWeapon) != null)
			{
				isNewNuke = false;
				CarrierType.IsEnabled = false;
				WarheadType.IsEnabled = false;
				Status.Content = "Configuring an existing nuclear weapon";
			}
		}

		private void DeployNuke_Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			ParentPage.DeployNuke_Window = null;
		}

		private void ExecuteOrder(object sender, RoutedEventArgs e)
		{
			switch (((ComboBoxItem)CarrierType.SelectedItem)?.Content.ToString())
			{
				case "Cruise Missile":
					selectedWeapon = new NuclearMissile();
					break;

				case "Medium Range Missile":
					selectedWeapon = new NuclearMissile();
					break;

				case "ICBM":
					selectedWeapon = new NuclearMissile();
					break;

				case "Aerial Bomb":
					selectedWeapon = new NuclearBomb();
					break;

				case null:
					if (isNewNuke) return;
					else break;

				default:
					return;
			}

			Warhead warhead = null;
			switch (((ComboBoxItem)WarheadType.SelectedItem)?.Content.ToString())
			{
				case "Hydrogen Bomb":
					warhead = new Hydrogen();
					break;

				case "Atomic Bomb Gen 1":
					warhead = new Atomic();
					break;

				case "Atomic Bomb Gen 2":
					warhead = new Atomic();
					break;

				case "Dirty Bomb":
					warhead = new Dirty();
					break;

				case null:
					if (isNewNuke) return;
					else break;

				default:
					return;
			}

			if (warhead != null) selectedWeapon.Warheads.Add(warhead);

			selectedWeapon.Name = TextBox_Name.Text;

			string targetName = (Target.SelectedItem as ComboBoxItem)?.Content.ToString();
			if (isNewNuke)
			{
				if (targetName == null)
				{
					return;
				}
				else
				{
					selectedWeapon.Target = Data.Regions.Single(n => n.Name == targetName);
				}
			}
			else
			{
				if (targetName != null)
				{
					selectedWeapon.Target = Data.Regions.Single(n => n.Name == targetName);
				}
			}

			if (isNewNuke)
			{
				if (SelectionList.SelectedItem != null)
				{
					selectedWeapon.Platform = SelectionList.SelectedItem as Platform;
					selectedWeapon.Platform.NuclearWeapons.Add(selectedWeapon);
				}
				else
				{
					return;
				}
			}
			else
			{
				if (SelectionList.SelectedItem != null)
				{
					selectedWeapon.Platform.NuclearWeapons.Remove(selectedWeapon);
					selectedWeapon.Platform = SelectionList.SelectedItem as Platform;
					selectedWeapon.Platform.NuclearWeapons.Add(selectedWeapon);
				}
			}

			this.Close();
		}

		private void SelectionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			#region Control the comboboxes of `CarrierType`
			if (SelectionList.SelectedItem is StrategicBomber)
			{
				for (int i = 0; i < 3; i++)
				{
					var current = CarrierType.Items[i] as ComboBoxItem;
					current.IsEnabled = false;
				}
				(CarrierType.Items[3] as ComboBoxItem).IsEnabled = true;
			}
			else
			{
				(CarrierType.Items[3] as ComboBoxItem).IsEnabled = false;
				for (int i = 0; i < 3; i++)
				{
					var current = CarrierType.Items[i] as ComboBoxItem;
					current.IsEnabled = true;
				}
			}
			#endregion
		}

		private void CarrierType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
		}
	}
}
