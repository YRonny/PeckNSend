using UnityEngine;
using System;

public static class MailEvents
{
    public static event Action<MailboxType> OnMailboxSpawned;

    public static void NotifyMailboxSpawned(MailboxType type)
    {
        OnMailboxSpawned?.Invoke(type);
    }
}
