
public class Ticket
{
    public static void DoShopBuyTicketsRequest(UserTicketKind ticketType, int ticketCount, Action<bool, ShopBuyTicketsResponse> callback)
    {
        ShopBuyTicketsRequest shopBuyTicketsRequest = new ShopBuyTicketsRequest
        {
            CommonParams = NetworkUtils.GetCommonParams(),
            TicketType = (uint)ticketType,
            TicketNum = (uint)ticketCount
        };
        GameApp.NetWork.Send(shopBuyTicketsRequest, delegate (IMessage response)
        {
            ShopBuyTicketsResponse shopBuyTicketsResponse = response as ShopBuyTicketsResponse;
            if (shopBuyTicketsResponse != null && shopBuyTicketsResponse.Code == 0)
            {
                if (shopBuyTicketsResponse.CommonData != null && shopBuyTicketsResponse.CommonData.Reward != null && shopBuyTicketsResponse.CommonData.Reward.Count > 0)
                {
                    DxxTools.UI.OpenRewardCommon(shopBuyTicketsResponse.CommonData.Reward, null, true);
                }
                Action<bool, ShopBuyTicketsResponse> callback2 = callback;
                if (callback2 != null)
                {
                    callback2(true, shopBuyTicketsResponse);
                }
                int num = (int)(1121800 + ticketType);
                GameApp.SDK.Analyze.Track_Get(GameTGATools.GetSourceName(num), shopBuyTicketsResponse.CommonData.Reward, null, null, null, null);
                GameApp.SDK.Analyze.Track_Cost(GameTGATools.CostSourceName(num), shopBuyTicketsResponse.CommonData.CostDto, null);
                return;
            }
            Action<bool, ShopBuyTicketsResponse> callback3 = callback;
            if (callback3 == null)
            {
                return;
            }
            callback3(false, shopBuyTicketsResponse);
        }, false, false, string.Empty, true);
    }

    public static void DoTicketsGetListRequest(Action<bool, TicketsGetListResponse> callback)
    {
        TicketsGetListRequest ticketsGetListRequest = new TicketsGetListRequest();
        ticketsGetListRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(ticketsGetListRequest, delegate (IMessage response)
        {
            TicketsGetListResponse ticketsGetListResponse = response as TicketsGetListResponse;
            if (ticketsGetListResponse != null && ticketsGetListResponse.Code == 0)
            {
                Action<bool, TicketsGetListResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, ticketsGetListResponse);
                return;
            }
            else
            {
                Action<bool, TicketsGetListResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, ticketsGetListResponse);
                return;
            }
        }, false, false, string.Empty, true);
    }
}
