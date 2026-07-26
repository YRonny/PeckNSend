using PeckNSend.Presenters;
using UnityEngine;

namespace PeckNSend.Presenters
{
    public static class MailScoreReporter
    {
        public static void RegisterSuccessfulDelivery(GameObject scoringObject, int amount)
        {
            PlayerOwnership ownership = scoringObject.GetComponentInParent<PlayerOwnership>();
            GameSessionPresenter.Instance.Model.RegisterDeliveredMail(ownership.UnityPlayerIndex, amount);
        }
    }
}