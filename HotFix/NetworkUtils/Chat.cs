
public class Chat
{
    public static void DoRequest_GetMessageRecords(long msgId, SocketGroupType groupType, string groupId, Action<bool, ChatGetMessageRecordsResponse> callback)
    {
        if (GameApp.SocketNet.Connected)
        {
            ChatGroupData group = Singleton<ChatManager>.Instance.GetGroup(groupType, groupId);
            ChatGetMessageRecordsRequest chatGetMessageRecordsRequest = new ChatGetMessageRecordsRequest
            {
                CommonParams = NetworkUtils.GetCommonParams(),
                MsgId = (ulong)msgId,
                PageIndex = (uint)group.HistoryPage,
                GroupType = (uint)groupType,
                GroupId = groupId
            };
            GameApp.NetWork.Send(chatGetMessageRecordsRequest, delegate (IMessage response)
            {
                ChatGetMessageRecordsResponse chatGetMessageRecordsResponse = response as ChatGetMessageRecordsResponse;
                if (chatGetMessageRecordsResponse != null && chatGetMessageRecordsResponse.Code == 0)
                {
                    ChatProxy.Common.OnRecvGuildChatRecords(group, chatGetMessageRecordsResponse);
                    Action<bool, ChatGetMessageRecordsResponse> callback3 = callback;
                    if (callback3 == null)
                    {
                        return;
                    }
                    callback3(true, chatGetMessageRecordsResponse);
                    return;
                }
                else
                {
                    Action<bool, ChatGetMessageRecordsResponse> callback4 = callback;
                    if (callback4 == null)
                    {
                        return;
                    }
                    callback4(false, chatGetMessageRecordsResponse);
                    return;
                }
            }, false, false, string.Empty, false);
            return;
        }
        GameApp.SocketNet.CheckReconnect(string.Format("{0} chat", groupType));
        Action<bool, ChatGetMessageRecordsResponse> callback2 = callback;
        if (callback2 == null)
        {
            return;
        }
        callback2(false, null);
    }
}
