using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using Habby.Mail.Data;
using Proto.Common;

namespace HotFix
{
	public static class MailExpand
	{
		public static List<ItemData> ToItemDatas(this MailReward[] rewards)
		{
			List<ItemData> list = new List<ItemData>();
			if (rewards == null)
			{
				return null;
			}
			foreach (MailReward mailReward in rewards)
			{
				if (mailReward != null)
				{
					ItemData itemData = mailReward.ToItemData();
					if (itemData != null)
					{
						list.Add(itemData);
					}
				}
			}
			return list;
		}

		public static List<ItemData> ToItemDatas(this List<MailReward> rewards)
		{
			List<ItemData> list = new List<ItemData>();
			if (rewards == null)
			{
				return null;
			}
			for (int i = 0; i < rewards.Count; i++)
			{
				MailReward mailReward = rewards[i];
				if (mailReward != null)
				{
					ItemData itemData = mailReward.ToItemData();
					if (itemData != null)
					{
						list.Add(itemData);
					}
				}
			}
			return list;
		}

		public static ItemData ToItemData(this MailReward reward)
		{
			if (reward == null)
			{
				return null;
			}
			ItemData itemData = null;
			int num;
			if (int.TryParse(reward.id, out num))
			{
				itemData = new ItemData(num, (long)((int)reward.count));
			}
			return itemData;
		}

		public static bool IsHaveReward(this MailData mailData)
		{
			return mailData != null && mailData.rewards != null && mailData.rewards.Length != 0;
		}

		public static bool IsReadShow(this MailData mailData)
		{
			bool flag = false;
			bool flag2 = mailData.IsHaveReward();
			if (flag2 && mailData.isReward)
			{
				flag = true;
			}
			if (!flag2 && mailData.isRead)
			{
				flag = true;
			}
			return flag;
		}

		public static long GetEffectiveAt(this MailData mailData)
		{
			return Convert.ToInt64((Convert.ToDateTime(mailData.effectiveAt).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds);
		}

		public static long GetExpireAt(this MailData mailData)
		{
			return Convert.ToInt64((Convert.ToDateTime(mailData.expireAt).ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds);
		}

		public static CommonData MergeCommonData(this CommonData a, CommonData b)
		{
			if (b == null)
			{
				return a;
			}
			if (b.UpdateUserCurrency != null)
			{
				a.UpdateUserCurrency = b.UpdateUserCurrency;
			}
			if (b.UpdateUserLevel != null)
			{
				a.UpdateUserLevel = b.UpdateUserLevel;
			}
			if (b.UserTickets != null && b.UserTickets.Count > 0)
			{
				a.UserTickets.AddRange(b.UserTickets);
			}
			if (b.Equipment != null && b.Equipment.Count > 0)
			{
				a.Equipment.AddRange(b.Equipment);
			}
			if (b.CollectionDto != null && b.CollectionDto.Count > 0)
			{
				a.CollectionDto.AddRange(b.CollectionDto);
			}
			if (b.PetDto != null && b.PetDto.Count > 0)
			{
				a.PetDto.AddRange(b.PetDto);
			}
			if (b.ArtifactItemDtos != null && b.ArtifactItemDtos.Count > 0)
			{
				a.ArtifactItemDtos.AddRange(b.ArtifactItemDtos);
			}
			if (b.MountItemDtos != null && b.MountItemDtos.Count > 0)
			{
				a.MountItemDtos.AddRange(b.MountItemDtos);
			}
			if (b.Reward != null)
			{
				a.Reward.AddRange(b.Reward);
			}
			if (b.Items != null)
			{
				foreach (KeyValuePair<ulong, ItemDto> keyValuePair in b.Items)
				{
					if (a.Items.ContainsKey(keyValuePair.Key))
					{
						a.Items[keyValuePair.Key] = keyValuePair.Value;
					}
					else
					{
						a.Items.Add(keyValuePair.Key, keyValuePair.Value);
					}
				}
			}
			if (b.Heros != null)
			{
				a.Heros.AddRange(b.Heros);
			}
			if (b.UserInfoDto != null)
			{
				if (b.UserInfoDto.UnlockAvatarList != null && b.UserInfoDto.UnlockAvatarList.Count > 0)
				{
					if (a.UserInfoDto == null)
					{
						a.UserInfoDto = new UserInfoDto();
					}
					a.UserInfoDto.UnlockAvatarList.AddRange(b.UserInfoDto.UnlockAvatarList);
				}
				if (b.UserInfoDto.UnlockAvatarFrameList != null && b.UserInfoDto.UnlockAvatarFrameList.Count > 0)
				{
					if (a.UserInfoDto == null)
					{
						a.UserInfoDto = new UserInfoDto();
					}
					a.UserInfoDto.UnlockAvatarFrameList.AddRange(b.UserInfoDto.UnlockAvatarFrameList);
				}
				if (b.UserInfoDto.UnlockTitleList != null && b.UserInfoDto.UnlockTitleList.Count > 0)
				{
					if (a.UserInfoDto == null)
					{
						a.UserInfoDto = new UserInfoDto();
					}
					a.UserInfoDto.UnlockTitleList.AddRange(b.UserInfoDto.UnlockTitleList);
				}
				if (b.UserInfoDto.UnlockSkinBodyList != null && b.UserInfoDto.UnlockSkinBodyList.Count > 0)
				{
					if (a.UserInfoDto == null)
					{
						a.UserInfoDto = new UserInfoDto();
					}
					a.UserInfoDto.UnlockSkinBodyList.AddRange(b.UserInfoDto.UnlockSkinBodyList);
				}
				if (b.UserInfoDto.UnlockSkinHeaddressList != null && b.UserInfoDto.UnlockSkinHeaddressList.Count > 0)
				{
					if (a.UserInfoDto == null)
					{
						a.UserInfoDto = new UserInfoDto();
					}
					a.UserInfoDto.UnlockSkinHeaddressList.AddRange(b.UserInfoDto.UnlockSkinHeaddressList);
				}
				if (b.UserInfoDto.UnlockSkinAccessoryList != null && b.UserInfoDto.UnlockSkinAccessoryList.Count > 0)
				{
					if (a.UserInfoDto == null)
					{
						a.UserInfoDto = new UserInfoDto();
					}
					a.UserInfoDto.UnlockSkinAccessoryList.AddRange(b.UserInfoDto.UnlockSkinAccessoryList);
				}
				if (b.UserInfoDto.UnlockBackGroundList != null && b.UserInfoDto.UnlockBackGroundList.Count > 0)
				{
					if (a.UserInfoDto == null)
					{
						a.UserInfoDto = new UserInfoDto();
					}
					a.UserInfoDto.UnlockBackGroundList.AddRange(b.UserInfoDto.UnlockBackGroundList);
				}
			}
			return a;
		}

		public static void ToOverlay(this RepeatedField<RewardDto> reward)
		{
			if (reward == null)
			{
				return;
			}
			Dictionary<uint, RewardDto> dictionary = new Dictionary<uint, RewardDto>(reward.Count);
			for (int i = 0; i < reward.Count; i++)
			{
				RewardDto rewardDto = reward[i];
				if (rewardDto != null)
				{
					RewardDto rewardDto2;
					if (!dictionary.TryGetValue(rewardDto.ConfigId, out rewardDto2))
					{
						dictionary[rewardDto.ConfigId] = rewardDto;
					}
					else
					{
						rewardDto2.Count += rewardDto.Count;
						dictionary[rewardDto.ConfigId] = rewardDto2;
					}
				}
			}
			reward.Clear();
			foreach (KeyValuePair<uint, RewardDto> keyValuePair in dictionary)
			{
				reward.Add(keyValuePair.Value);
			}
		}
	}
}
