using System.Collections.Generic;
using System.Linq;
using Fishit.Dal;
using Fishit.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fishit.BusinessLayer
{
    public class FishingTripManager : ManagerBase
    {
        public IList<FishingTrip> List
        {
            get
            {
                using (FishitContext context = new FishitContext())
                {
                    return context.FishingTrips.ToList();
                }
            }
        }

        public void Add(FishingTrip fishingTrip)
        {
            using (FishitContext context = new FishitContext())
            {
                try
                {
                    context.Entry(fishingTrip).State = EntityState.Added;
                    context.SaveChanges();
                }
                catch (DbUpdateException exception)
                {
                    HandleDbUpdateException(exception, context, fishingTrip);
                }
            }
        }

        public FishingTrip GetById(int id)
        {
            return List.FirstOrDefault(entry => entry.Id == id);
        }
    }
}
