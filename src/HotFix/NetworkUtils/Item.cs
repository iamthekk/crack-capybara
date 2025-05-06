
public class Item
{
    public static void SendItemUseRequest(ulong rowId, uint count, uint index, Action<bool, ItemUseResponse> callback)
    {
        ItemUseRequest itemUseRequest = new ItemUseRequest();
        itemUseRequest.CommonParams = NetworkUtils.GetCommonParams();
        itemUseRequest.RowId = rowId;
        itemUseRequest.Count = count;
        itemUseRequest.Index = index;
        GameApp.NetWork.Send(itemUseRequest, delegate (IMessage response)
        {
            ItemUseResponse itemUseResponse = response as ItemUseResponse;
            if (itemUseResponse != null && itemUseResponse.Code == 0)
            {
                Action<bool, ItemUseResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, itemUseResponse);
                return;
            }
            else
            {
                Action<bool, ItemUseResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, itemUseResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }
}
