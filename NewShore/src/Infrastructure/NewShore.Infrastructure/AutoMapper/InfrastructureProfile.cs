using NewShore.Application.flights.Queries.Getflights;
using AutoMapper;
using NewShore.Domain.Entities;
using NewShore.Domain.ValueObjects;
using System;
using NewShoreAIR.Api.Client.Contracts;

namespace NewShore.Infrastructure.AutoMapper
{
    public class InfrastructureProfile : Profile
    {
        public InfrastructureProfile()
        {
            CreateMap<FlightRepresentation, FlightModel>()
                .ForMember(x => x.Id,
                    opt => opt.MapFrom(src => new Guid()))
                .ForMember(x => x.Origin,
                    opt => opt.MapFrom(src => src.DepartureStation))
                .ForMember(x => x.Destination,
                    opt => opt.MapFrom(src => src.ArrivalStation))
                .ForMember(x => x.Transport,
                    opt => opt.MapFrom(src => new Transport(src.FlightCarrier, src.FlightNumber)))
                .ForMember(x => x.Price,
                    opt => opt.MapFrom(src => src.Price));

            CreateMap<FlightModel, Flight>()
                .ForMember(x => x.Origin,
                    opt => opt.MapFrom(src => src.Origin))
                .ForMember(x => x.Destination,
                    opt => opt.MapFrom(src => src.Destination))
                .ForMember(x => x.Transport,
                    opt => opt.MapFrom(src => src.Transport))
                .ForMember(x => x.Price,
                    opt => opt.MapFrom(src => src.Price));

        }
    }
}
