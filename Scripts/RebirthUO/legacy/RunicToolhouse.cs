/*
 created by:
     /\            888                   888     .d8888b.   .d8888b.  
____/_ \____       888                   888    d88P  Y88b d88P  Y88b 
\  ___\ \  /       888                   888    888    888 888    888 
 \/ /  \/ /    .d88888  8888b.   8888b.  888888 Y88b. d888 Y88b. d888 
 / /\__/_/\   d88" 888     "88b     "88b 888     "Y888P888  "Y888P888 
/__\ \_____\  888  888 .d888888 .d888888 888           888        888 
    \  /      Y88b 888 888  888 888  888 Y88b.  Y88b  d88P Y88b  d88P 
     \/        "Y88888 "Y888888 "Y888888  "Y888  "Y8888P"   "Y8888P"  

description: a small mini house item that can hold runic tools up to 60000 uses each.
rewrited from scratch at 20/06/2005
*/
using System;
using System.Collections;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.RebirthUO.CustomCraftingSystem.Items.Tools;
using Server.Targeting;

namespace Server.Items
{
	public class RunicHouse : Item
	{
		public Hashtable Tinker{ get{ return ht_Tinker; } set{ ht_Tinker = value; } }
		private Hashtable ht_Tinker = new Hashtable();

		public Hashtable Tailor{ get{ return ht_Tailor; } set{ ht_Tailor = value; } }
		private Hashtable ht_Tailor = new Hashtable();

		public Hashtable Rest{ get{ return ht_Rest; } set{ ht_Rest = value; } }
		private Hashtable ht_Rest = new Hashtable();

		[Constructable]
		public RunicHouse() : base( 0x22c4 )
		{
			Movable = true;
			Weight = 1.0;
			Hue = 88;
			Name = "Runic House";
			//LootType = LootType.Blessed;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if (!(from is PlayerMobile))
				return;
			if ( IsChildOf(from.Backpack) || IsChildOf(from.BankBox) )
				from.SendGump( new RunicHouseGump( from, this ) );
			else
				from.SendMessage("The runic house must be in your backpack or your bank box.");
		}

		public void BeginCombine( Mobile from ) { from.Target = new RunicHouseTarget( this ); }		
		
		public void EndCombine( Mobile from, object o )
		{
			if (!(o is BaseRunicTool))
			{
				from.SendMessage(32, "That isn't a runic tool");
				return;
			}
			int i_Resource = (int)((BaseRunicTool)o).Resource;
			if ( o is RunicTinkerTool )
				AddTinker( (CraftResource)i_Resource, (BaseRunicTool)o, from );
			else if ( o is RunicSewingKit )
				AddTailor( (CraftResource)i_Resource, (BaseRunicTool)o, from );
			else 
				AddRest( (CraftResource)i_Resource, (BaseRunicTool)o, from );
		}

		public void AddTinker( CraftResource cr, BaseRunicTool brt, Mobile from)
		{
			if ( ht_Tinker == null )
				ht_Tinker = new Hashtable();
			int i_Have = 0;
			if ( ht_Tinker[cr] != null )
				i_Have = (int)ht_Tinker[cr];

			if ( i_Have < 60000 )
			{
				if ( i_Have + brt.UsesRemaining > 60000 )
				{
					brt.UsesRemaining = i_Have + brt.UsesRemaining - 60000;
					i_Have = 60000;
				}
				else
				{
					i_Have += brt.UsesRemaining;
					brt.Delete();
				}

				if ( ht_Tinker.ContainsKey(cr) )
					ht_Tinker[cr] = i_Have;
				else
					ht_Tinker.Add(cr, i_Have);
				
				BeginCombine( from );
				from.SendGump( new RunicHouseGump( from, this ) );
				from.SendMessage(88, "You added the runic charges.");
				return;
			}
			from.SendGump( new RunicHouseGump( from, this ) );
			from.SendMessage(33, "You can't add more charges, the limit is 60,000.");
		}

		public void AddTailor( CraftResource cr, BaseRunicTool brt, Mobile from)
		{
			if ( ht_Tailor == null )
				ht_Tailor = new Hashtable();
			int i_Have = 0;
			if ( ht_Tailor[cr] != null )
				i_Have = (int)ht_Tailor[cr];

			if ( i_Have < 60000 )
			{
				if ( i_Have + brt.UsesRemaining > 60000 )
				{
					brt.UsesRemaining = i_Have + brt.UsesRemaining - 60000;
					i_Have = 60000;
				}
				else
				{
					i_Have += brt.UsesRemaining;
					brt.Delete();
				}
				if ( ht_Tailor.ContainsKey(cr) )
					ht_Tailor[cr] = i_Have;
				else
					ht_Tailor.Add(cr, i_Have);
				BeginCombine( from );
				from.SendGump( new RunicHouseGump( from, this ) );
				from.SendMessage("You added the runic charges.");
				return;
			}
			from.SendGump( new RunicHouseGump( from, this ) );
			from.SendMessage("You can't add more charges, the limit is 60,000.");
		}
		
		public void AddRest( CraftResource cr, BaseRunicTool brt, Mobile from)
		{
			if ( ht_Rest == null )
				ht_Rest = new Hashtable();
			int i_Have = 0;
			if ( ht_Rest[cr] != null )
				i_Have = (int)ht_Rest[cr];

			if ( i_Have < 60000 )
			{
				if ( i_Have + brt.UsesRemaining > 60000 )
				{
					brt.UsesRemaining = i_Have + brt.UsesRemaining - 60000;
					i_Have = 60000;
				}
				else
				{
					i_Have += brt.UsesRemaining;
					brt.Delete();
				}
				if ( ht_Rest.ContainsKey(cr) )
					ht_Rest[cr] = i_Have;
				else
					ht_Rest.Add(cr, i_Have);
				BeginCombine( from );
				from.SendGump( new RunicHouseGump( from, this ) );
				from.SendMessage("You added the runic charges.");
				return;
			}
			from.SendGump( new RunicHouseGump( from, this ) );
			from.SendMessage("You can't add more charges, the limit is 60,000.");
		}

		public RunicHouse( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 3 ); // version
			//version 3 moved to hash table
			
			writer.Write( ht_Tinker.Count );
			foreach ( DictionaryEntry de in ht_Tinker )
			{
				writer.Write( (int)((CraftResource)de.Key) );
				writer.Write( (int)de.Value );
			}

			writer.Write( ht_Tailor.Count );
			foreach ( DictionaryEntry de in ht_Tailor )
			{
				writer.Write( (int)((CraftResource)de.Key) );
				writer.Write( (int)de.Value );
			}

			writer.Write( ht_Rest.Count );
			foreach ( DictionaryEntry de in ht_Rest )
			{
				writer.Write( (int)((CraftResource)de.Key) );
				writer.Write( (int)de.Value );
			}
		} 
		
		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt();
			
			switch ( version )
			{
				case 3:
				{
					int len;
					len = reader.ReadInt();
					for (int i=0; i < len; i++)
						ht_Tinker.Add( (CraftResource)reader.ReadInt(), reader.ReadInt() );

					len = reader.ReadInt();
					for (int i=0; i < len; i++)
						ht_Tailor.Add( (CraftResource)reader.ReadInt(), reader.ReadInt() );

					len = reader.ReadInt();
					for (int i=0; i < len; i++)
						ht_Rest.Add( (CraftResource)reader.ReadInt(), reader.ReadInt() );
					break;
				}
				case 2:
				{
					Console.Write("Upgrading daat99 runic house...");
					int len;
					if ( ht_Tinker == null )
						ht_Tinker = new Hashtable();
					len = reader.ReadInt();//iron tinker deleted
					for (int i = 0; i < 13; i++)//tinker
						ht_Tinker.Add(((CraftResource)i+2),reader.ReadInt());
					
					int i_BlazeL = reader.ReadInt();

					if ( ht_Rest == null )
						ht_Rest = new Hashtable();
					len = reader.ReadInt(); //iron hammer deleted
					for (int i = 0; i < 13; i++)//blacksmith
						ht_Rest.Add(((CraftResource)i+2),reader.ReadInt());

					for (int i = 301; i < 312; i++) //fletcher
						ht_Rest.Add(((CraftResource)i),reader.ReadInt());

					len = reader.ReadInt(); //regular leather kit deleted

					if ( ht_Tailor == null )
						ht_Tailor = new Hashtable();
					for (int i = 102; i < 112; i++) //tailor
					{
						if (i == 107)
							ht_Tailor.Add(((CraftResource)i),i_BlazeL);
						else
							ht_Tailor.Add(((CraftResource)i),reader.ReadInt());
					}
					len = reader.ReadInt();//deleting limit
					break;
				}
			}
		} 
	} 

	public class RunicHouseGump : Gump
	{
		private Mobile m_From;
		private RunicHouse rh_House;

		public RunicHouseGump( Mobile from, RunicHouse house ) : base( 25, 25 )
		{
			m_From = from;
			rh_House = house;

			from.CloseGump( typeof( RunicHouseGump ) );

			AddPage( 0 );

			AddBackground( 50, 10, 600, 360, 5170 );
			AddPage(0);
			AddLabel( 280, 30, 25, "daat99's Runic House" );

			for (int line = 0; line < 13; line++)
			{
				AddLabel( 80, 70+line*20, 0x486, (CraftResources.GetName( ((CraftResource)line+2) )).ToString() );

				AddLabel( 160, 50, 1164, "Tinker" );
				if ( rh_House.Tinker.ContainsKey( ((CraftResource)line+2) ) )
				{
					AddButton( 155, 68+line*20, 4015, 4016, 52+line, GumpButtonType.Reply, 0 );
					AddLabel( 190, 70+line*20, 0x480, rh_House.Tinker[((CraftResource)line+2)].ToString() );
				}
				else
				{
					AddButton( 155, 68+line*20, 4006, 4007, 52+line, GumpButtonType.Reply, 0 );
					AddLabel( 190, 70+line*20, 0x480, "0" );
				}
				
				AddLabel( 240, 50, 1164, "Blacksmith" );
				if ( rh_House.Rest.ContainsKey( ((CraftResource)line+2) ) )
				{
					AddButton( 235, 68+line*20, 4015, 4016, 2+line, GumpButtonType.Reply, 0 );
					AddLabel( 270, 70+line*20, 0x480, rh_House.Rest[((CraftResource)line+2)].ToString() );
				}
				else
				{
					AddButton( 235, 68+line*20, 4006, 4007, 2+line, GumpButtonType.Reply, 0 );
					AddLabel( 270, 70+line*20, 0x480, "0" );
				}

				AddLabel( 390, 50, 1164, "Tailor" );
				if ( line < 10 )
				{
					AddLabel( 320, 70+line*20, 0x486, (CraftResources.GetName( ((CraftResource)line+102) )).ToString() );
					if ( rh_House.Tailor.ContainsKey( ((CraftResource)line+102) ) )
					{
						AddButton( 385, 68+line*20, 4015, 4016, 102+line, GumpButtonType.Reply, 0 );
						AddLabel( 420, 70+line*20, 0x480, rh_House.Tailor[((CraftResource)line+102)].ToString() );
					}
					else
					{
						AddButton( 385, 68+line*20, 4006, 4007, 102+line, GumpButtonType.Reply, 0 );
						AddLabel( 420, 70+line*20, 0x480, "0" );
					}
					
				}

				AddLabel( 550, 50, 1164, "Fletcher" );
				if ( line < 11 )
				{
					AddLabel( 470, 70+line*20, 0x486, (CraftResources.GetName( ((CraftResource)line+302) )).ToString() );
					if ( rh_House.Rest.ContainsKey( ((CraftResource)line+302) ) )
					{
						AddButton( 545, 68+line*20, 4015, 4016, 302+line, GumpButtonType.Reply, 0 );
						AddLabel( 580, 70+line*20, 0x480, rh_House.Rest[((CraftResource)line+302)].ToString() );
					}
					else
					{
						AddButton( 545, 68+line*20, 4006, 4007, 301+line, GumpButtonType.Reply, 0 );
						AddLabel( 580, 70+line*20, 0x480, "0" );
					}
				}
				AddLabel( 470, 310, 0x486, "Add" );
				AddButton( 545, 308, 4015, 4016, 999, GumpButtonType.Reply, 0 );
			}
		}

		public override void OnResponse( NetState sender, RelayInfo info)
		{
			if ( rh_House.Deleted )
				return;

			if ( info.ButtonID >= 2 && info.ButtonID < 50 ) //blacksmith
			{
				if ( rh_House.Rest.ContainsKey( ((CraftResource)info.ButtonID) ) )
				{
					m_From.AddToBackpack( new RunicHammer(((CraftResource)info.ButtonID), (int)(rh_House.Rest[((CraftResource)info.ButtonID)])) );
					rh_House.Rest.Remove( ((CraftResource)info.ButtonID) );
				}
				else
					rh_House.BeginCombine( m_From );
				m_From.SendGump( new RunicHouseGump( m_From, rh_House ) );
			}
			else if ( info.ButtonID >= 52 && info.ButtonID < 100 ) //tinker
			{
				if ( rh_House.Tinker.ContainsKey( ((CraftResource)info.ButtonID - 50) ) )
				{
					m_From.AddToBackpack( new RunicTinkerTool(((CraftResource)info.ButtonID - 50), (int)(rh_House.Tinker[((CraftResource)info.ButtonID - 50)])) );
					rh_House.Tinker.Remove( ((CraftResource)info.ButtonID - 50) );
				}
				else
					rh_House.BeginCombine( m_From );
				m_From.SendGump( new RunicHouseGump( m_From, rh_House ) );
			}
			else if ( info.ButtonID >= 102 && info.ButtonID < 200 )//tailor
			{
				if ( rh_House.Tailor.ContainsKey( ((CraftResource)info.ButtonID) ) )
				{
					m_From.AddToBackpack( new RunicSewingKit(((CraftResource)info.ButtonID), (int)(rh_House.Tailor[((CraftResource)info.ButtonID)])) );
					rh_House.Tailor.Remove( ((CraftResource)info.ButtonID) );
				}
				else
					rh_House.BeginCombine( m_From );
				m_From.SendGump( new RunicHouseGump( m_From, rh_House ) );
			}
			else if ( info.ButtonID > 300 )//fletcher
			{
				if ( rh_House.Rest.ContainsKey( ((CraftResource)info.ButtonID) ) )
				{
					m_From.AddToBackpack( new RunicFletcherTool(((CraftResource)info.ButtonID), (int)(rh_House.Rest[((CraftResource)info.ButtonID)])) );
					rh_House.Rest.Remove( ((CraftResource)info.ButtonID) );
				}
				else
					rh_House.BeginCombine( m_From );
				m_From.SendGump( new RunicHouseGump( m_From, rh_House ) );
			}
			else if ( info.ButtonID == 999 )//add
			{
				rh_House.BeginCombine( m_From );
				m_From.SendGump( new RunicHouseGump( m_From, rh_House ) );
			}
		}
	}

	public class RunicHouseTarget : Target
	{
		private RunicHouse rh_House;

		public RunicHouseTarget( RunicHouse house ) : base( 18, false, TargetFlags.None )
		{
			rh_House = house;
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			if ( rh_House.Deleted )
				return;

			rh_House.EndCombine( from, targeted );
		}
	}
}
