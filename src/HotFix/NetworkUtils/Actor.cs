
public class Actor
{
    public static void DoActorLevelUpRequest(Action<bool, ActorLevelUpResponse> callback)
    {
        ActorLevelUpRequest actorLevelUpRequest = new ActorLevelUpRequest();
        actorLevelUpRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(actorLevelUpRequest, delegate (IMessage response)
        {
            ActorLevelUpResponse actorLevelUpResponse = response as ActorLevelUpResponse;
            if (actorLevelUpResponse != null && actorLevelUpResponse.Code == 0)
            {
                Action<bool, ActorLevelUpResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, actorLevelUpResponse);
                return;
            }
            else
            {
                Action<bool, ActorLevelUpResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, actorLevelUpResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }

    public static void DoActorAdvanceUpRequest(Action<bool, ActorAdvanceUpResponse> callback)
    {
        ActorAdvanceUpRequest actorAdvanceUpRequest = new ActorAdvanceUpRequest();
        actorAdvanceUpRequest.CommonParams = NetworkUtils.GetCommonParams();
        GameApp.NetWork.Send(actorAdvanceUpRequest, delegate (IMessage response)
        {
            ActorAdvanceUpResponse actorAdvanceUpResponse = response as ActorAdvanceUpResponse;
            if (actorAdvanceUpResponse != null && actorAdvanceUpResponse.Code == 0)
            {
                Action<bool, ActorAdvanceUpResponse> callback2 = callback;
                if (callback2 == null)
                {
                    return;
                }
                callback2(true, actorAdvanceUpResponse);
                return;
            }
            else
            {
                Action<bool, ActorAdvanceUpResponse> callback3 = callback;
                if (callback3 == null)
                {
                    return;
                }
                callback3(false, actorAdvanceUpResponse);
                return;
            }
        }, true, false, string.Empty, true);
    }
}
