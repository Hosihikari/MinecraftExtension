﻿using Hosihikari.Minecraft.Extension.Events.Implements;
using Hosihikari.NativeInterop.Hook.ObjectOriented;

namespace Hosihikari.Minecraft.Extension.Events;

public delegate Task AsyncEventHandler<TEventArgs>(object? sender, TEventArgs e) where TEventArgs : EventArgsBase;

public abstract class HookEventBase<TEventArgs, THookDelegate> : HookBase<THookDelegate>
    where TEventArgs : EventArgsBase
    where THookDelegate : Delegate
{
    protected HookEventBase(string symbol) : base(symbol) { }
    private event EventHandler<TEventArgs>? InternalBefore;
    private event EventHandler<TEventArgs>? InternalAfter;
    private event AsyncEventHandler<TEventArgs>? InternalAsync;

    public event EventHandler<TEventArgs> Before
    {
        add
        {
            CheckEventAdded();
            InternalBefore += value;
        }
        remove
        {
            InternalBefore -= value;
            CheckEventRemoved();
        }
    }

    public event EventHandler<TEventArgs> After
    {
        add
        {
            CheckEventAdded();
            InternalAfter += value;
        }
        remove
        {
            InternalAfter -= value;
            CheckEventRemoved();
        }
    }

    public event AsyncEventHandler<TEventArgs> Async
    {
        add
        {
            CheckEventAdded();
            InternalAsync += value;
        }
        remove
        {
            InternalAsync -= value;
            CheckEventRemoved();
        }
    }

    private void CheckEventAdded()
    {
        if (InternalBefore is null && InternalAfter is null && InternalAsync is null)
            LevelTick.RunInTick(BeforeEventAdded);
    }

    private void CheckEventRemoved()
    {
        if (InternalBefore is null && InternalAfter is null && InternalAsync is null)
            LevelTick.RunInTick(AfterEventAllRemoved);
    }

    protected virtual void BeforeEventAdded()
    {
        //install hook when first event added
        if (!this.HasInstalled)
            this.Install();
    }

    protected virtual void AfterEventAllRemoved()
    {
        //uninstall hook when all event removed
        if (this.HasInstalled)
            this.Uninstall();
    }

    protected virtual void OnEventBefore(TEventArgs e) => InternalBefore?.Invoke(this, e);

    protected virtual void OnEventAfter(TEventArgs e)
    {
        InternalAfter?.Invoke(this, e);
        InternalAsync?.Invoke(this, e);
    }
}

public abstract class HookCancelableEventBase<TEventArgs, THookDelegate> : HookEventBase<TEventArgs, THookDelegate>
    where TEventArgs : CancelableEventArgsBase
    where THookDelegate : Delegate
{
    protected HookCancelableEventBase(string symbol) : base(symbol) { }

    protected override void OnEventAfter(TEventArgs e)
    {
        //if the event canceled in before-event,the will not pass to after-event
        if (e.IsCanceled) return;
        base.OnEventAfter(e);
    }
}