// **********
// ServUO - BaseStorageContainerGump.cs
// **********

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Server.Gumps;
using Server.Network;

namespace Server.RebirthUO.CustomCraftingSystem.Items.StorageContainer
{
	[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
	[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
	[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
	public abstract class BaseStorageContainerGump<T1, T2> : Gump
	{
		private Dictionary<int, Tuple<string, T1>> ButtonReference { get; }

		protected abstract string ValueToString(T2 value);

		protected BaseStorageContainerGump(string title, Mobile mobile, BaseStorageContainer<T1, T2> container) :
			base(0, 0)
		{
			User = mobile;
			Container = container;
			Title = title;
			ButtonReference = new Dictionary<int, Tuple<string, T1>>();

			Dragable = true;
			Closable = true;
			Resizable = false;
			Disposable = false;

			InitBackGround();
			InitGroups();
			InitActions();
		}

		private void InitActions()
		{
			var startTop = (2 * BorderThickness) + HeaderAreaHeight +
			               (Container.GetMaxAmountOfEntries() * GroupEntrySpaceHeight);
			var startLeft = BorderThickness + LabelOffsetLeft;

			AddLabel(startLeft, startTop + (GroupEntrySpaceHeight * 2), TitleTextColor, "Actions");
			AddLabel(startLeft, startTop + (GroupEntrySpaceHeight * 3), GroupTextColor, "Collect");
			AddLabel(startLeft, startTop + (GroupEntrySpaceHeight * 4), GroupTextColor, "Add");
			AddLabel(startLeft, startTop + (GroupEntrySpaceHeight * 5), GroupTextColor, "Amount");

			AddButton(startLeft + GroupEntryButtonOffsetLeft, startTop + (GroupEntrySpaceHeight * 2) + LabelOffsetTop,
				4014, 4015, 1, GumpButtonType.Reply, 0);
			AddButton(startLeft + GroupEntryButtonOffsetLeft, startTop + (GroupEntrySpaceHeight * 3) + LabelOffsetTop,
				4014, 4015, 2, GumpButtonType.Reply, 0);
			AddButton(startLeft + GroupEntryButtonOffsetLeft, startTop + (GroupEntrySpaceHeight * 4) + LabelOffsetTop,
				5600, 5604, 3, GumpButtonType.Reply, 0);
			AddButton(startLeft + GroupEntryButtonOffsetLeft + LabelOffsetLeft,
				startTop + (GroupEntrySpaceHeight * 5) + LabelOffsetTop,
				5602, 5606, 4, GumpButtonType.Reply, 0);
		}

		private void InitGroups()
		{
			var count = 0;

			foreach (var key in Container.Data.Keys)
			{
				var startLeft = BorderThickness + (GroupSpaceWidth * count);
				var startTop = (2 * BorderThickness) + HeaderAreaHeight;

				AddLabel(startLeft, startTop, GroupTextColor, key);

				InitEntry(startLeft, startTop, key, Container.Data[key]);

				count++;
			}
		}

		private void InitEntry(int startLeft, int startTop, string key, Dictionary<T1, T2> dictionary)
		{
			var count = 0;

			foreach (var subKey in dictionary.Keys)
			{
				var buttonId = ResponseOffsetValue + ButtonReference.Count;
				var value = dictionary[subKey];
				var entry = new Tuple<string, T1>(key, subKey);

				var labelTop = startTop + (GroupEntrySpaceHeight * count) + 5;

				AddLabel(startLeft + LabelOffsetLeft, labelTop + LabelOffsetTop, TextColor,
					Container.GetEntryName(subKey));

				AddLabel(startLeft + GroupEntryValueOffsetLeft, labelTop + LabelOffsetTop, TextColor,
					ValueToString(value));

				AddButton(startLeft + GroupEntryButtonOffsetLeft, labelTop + LabelOffsetTop, ButtonNormalId,
					ButtonPressedId, buttonId,
					GumpButtonType.Reply, 0);

				ButtonReference.Add(buttonId, entry);

				count++;
			}
		}

		private void InitBackGround()
		{
			AddPage(0);

			var backgroundWidth = (2 * BorderThickness) + (Container.Data.Count * GroupSpaceWidth);
			var backgroundHeight = (2 * BorderThickness) + HeaderAreaHeight +
			                       (Container.GetMaxAmountOfEntries() * GroupEntrySpaceHeight) +
			                       (2 * GroupEntrySpaceHeight) + BackGroundActionsHeight;

			AddBackground(0, 0, backgroundWidth, backgroundHeight, BackgroundId);

			AddLabel(BorderThickness + OffsetTitle.X, BorderThickness + OffsetTitle.Y, TitleTextColor, Title);
		}

		public int BackGroundActionsHeight => 5 * GroupEntrySpaceHeight;

		protected string Title { get; }

		protected Mobile User { get; }

		protected BaseStorageContainer<T1, T2> Container { get; }

		protected int HeaderAreaHeight => 40;
		protected int BorderThickness => 15;
		protected int GroupSpaceWidth => 250;
		protected int GroupEntrySpaceHeight => 30;
		protected int BackgroundId => 9270;
		protected int TextColor => 1152;
		protected int TitleTextColor => 0x1F;
		protected int GroupTextColor => 0x2F;
		protected int LabelOffsetTop => 20;
		protected int LabelOffsetLeft => 25;
		protected Point2D OffsetTitle => new Point2D(25, 15);
		protected int ResponseOffsetValue => 1000;
		protected int ButtonNormalId => 4006;
		protected int ButtonPressedId => 4007;
		protected int GroupEntryValueOffsetLeft => 150;
		protected int GroupEntryButtonOffsetLeft => 200;

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			if (Container.IsAccessibleTo(User))
			{
				switch (info.ButtonID)
				{
					case 1: 
					{
						Container.Collect(User);
						Container.ShowGump(User);
						break;
					}
					case 2: 
					{
						Container.Add(User);
						Container.ShowGump(User);
						break;
					}
					case 3: 
					{
						Container.IncreaseStep();
						break;
					}
					case 4: 
					{
						Container.DecreaseStep();
						Container.ShowGump(User);
						break;
					}
					default:
					{
						if (ButtonReference.TryGetValue(info.ButtonID, out var value))
						{
							Container.TakeOff(User,value);
							Container.ShowGump(User);
						}

						break;
					}
				}
			}
			else
			{
				User.SendLocalizedMessage(500447);
				User.CloseGump(GetType());
			}
		}
	}
}
