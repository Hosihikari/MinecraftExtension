﻿using Hosihikari.Minecraft.Extension.Events.Implements.Player;

namespace Hosihikari.Minecraft.Extension.Events;

public static class Events
{
    private static readonly Lazy<ChatEvent> PlayerChatEvent = new(() => new());
    public static ChatEvent PlayerChat => PlayerChatEvent.Value;

    private static readonly Lazy<InitializedEvent> PlayerInitializedEvent = new(() => new());
    public static InitializedEvent PlayerInitialized => PlayerInitializedEvent.Value;
    
    private static readonly Lazy<JoinEvent> PlayerJoinEvent = new(() => new());
    public static JoinEvent PlayerJoin => PlayerJoinEvent.Value;
}