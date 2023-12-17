using MediatR;
using NewShore.Domain.Entities;
using System.Collections.Generic;

namespace NewShore.Application.Events
{
    public record UpdateDB(List<Journey> journeys) : INotification;
}
