using System;
using PolyhydraGames.Core.Interfaces;

namespace PolyhydraGames.Core.Models;

public interface IPortListener : IStart
{
    IObservable<PortReply> OnPortReplyChanged { get; }
    void SendResult(string result);
}
