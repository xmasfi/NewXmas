using MediatR;
using System.Threading.Tasks;
using System.Threading;
using NewShore.Application.Events;
using NewShore.Application.Interfaces;

namespace NewShore.Application.Handlers
{
    public class OnUpdateDB : INotificationHandler<UpdateDB>
    {
        private readonly INewShoreDbContext _dbContext;

        public OnUpdateDB(INewShoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(UpdateDB notification, CancellationToken cancellationToken)
        {
            var journeys = notification.journeys;


            // TODO: Hay que insertar SOLO fligths que no existan, si ya existen reutilizarlos            
            foreach (var journey in journeys)
            {
                foreach(var fligth in journey.Flights) 
                {
                    _dbContext.flights.Add(fligth);
                }
                _dbContext.journeys.Add(journey);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
