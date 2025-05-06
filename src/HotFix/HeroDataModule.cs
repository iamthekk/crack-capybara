using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using Proto.Common;
using Proto.User;
using Server;

namespace HotFix
{
	public class HeroDataModule : IDataModule
	{
		public CardData MainCardData
		{
			get
			{
				return this.m_mainCardData;
			}
		}

		public int GetName()
		{
			return 103;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public void Reset()
		{
			this.m_cardDatasDicForRowID.Clear();
			this.m_cardDatasDicForID.Clear();
			this.m_mainCardData = null;
		}

		public void SetLoginData(UserLoginResponse resp)
		{
			if (resp == null)
			{
				return;
			}
			if (resp.Heros == null)
			{
				return;
			}
			this.m_cardDatasDicForRowID.Clear();
			this.m_cardDatasDicForID.Clear();
			for (int i = 0; i < resp.Heros.Count; i++)
			{
				HeroDto heroDto = resp.Heros[i];
				if (heroDto != null)
				{
					CardData cardData = new CardData();
					cardData.m_rowID = (int)heroDto.RowId;
					cardData.m_memberID = (int)heroDto.HeroId;
					cardData.SetMemberRace(MemberRace.Hero);
					this.m_cardDatasDicForRowID[cardData.m_rowID] = cardData;
					List<CardData> list;
					this.m_cardDatasDicForID.TryGetValue(cardData.m_memberID, out list);
					if (list == null)
					{
						list = new List<CardData>();
					}
					list.Add(cardData);
					this.m_cardDatasDicForID[cardData.m_memberID] = list;
				}
			}
			if (resp.Actor == null)
			{
				return;
			}
			if (resp.Actor.RowId == 0UL)
			{
				return;
			}
			this.m_mainCardData = this.FindCardData((int)resp.Actor.RowId);
			this.m_mainCardData.m_isMainMember = true;
			GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.Hero);
		}

		private void UpdateHeros(RepeatedField<HeroDto> heros)
		{
			if (heros == null)
			{
				return;
			}
			for (int i = 0; i < heros.Count; i++)
			{
				HeroDto heroDto = heros[i];
				if (heroDto != null)
				{
					int num = (int)heroDto.RowId;
					CardData cardData;
					if (this.m_cardDatasDicForRowID.TryGetValue(num, out cardData))
					{
						cardData.m_rowID = num;
						cardData.m_memberID = (int)heroDto.HeroId;
					}
					else
					{
						if (cardData == null)
						{
							cardData = new CardData();
						}
						cardData.m_rowID = num;
						cardData.m_memberID = (int)heroDto.HeroId;
						cardData.SetMemberRace(MemberRace.Hero);
						this.m_cardDatasDicForRowID[cardData.m_rowID] = cardData;
						List<CardData> list;
						this.m_cardDatasDicForID.TryGetValue(cardData.m_memberID, out list);
						if (list == null)
						{
							list = new List<CardData>();
						}
						list.Add(cardData);
						this.m_cardDatasDicForID[cardData.m_memberID] = list;
					}
				}
			}
		}

		private void RemoveHeros(RepeatedField<ulong> rowIDs)
		{
			if (rowIDs == null)
			{
				return;
			}
			for (int i = 0; i < rowIDs.Count; i++)
			{
				int num = (int)rowIDs[i];
				if (num != 0)
				{
					CardData cardData;
					this.m_cardDatasDicForRowID.TryGetValue(num, out cardData);
					if (cardData != null)
					{
						this.m_cardDatasDicForRowID.Remove(num);
						int memberID = cardData.m_memberID;
						List<CardData> list;
						this.m_cardDatasDicForID.TryGetValue(memberID, out list);
						if (list != null)
						{
							list.Remove(cardData);
						}
						this.m_cardDatasDicForID[memberID] = list;
					}
				}
			}
		}

		public CardData FindCardData(int rowID)
		{
			CardData cardData;
			this.m_cardDatasDicForRowID.TryGetValue(rowID, out cardData);
			return cardData;
		}

		public List<CardData> FindCardDatas(int id)
		{
			List<CardData> list;
			this.m_cardDatasDicForID.TryGetValue(id, out list);
			return list;
		}

		private CardData m_mainCardData;

		private Dictionary<int, CardData> m_cardDatasDicForRowID = new Dictionary<int, CardData>();

		private Dictionary<int, List<CardData>> m_cardDatasDicForID = new Dictionary<int, List<CardData>>();
	}
}
