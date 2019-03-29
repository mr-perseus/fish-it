using System.Collections.Generic;
using System.Linq;
using Fishit.Dal;
using Fishit.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fishit.Common
{
    public class FishingTripDao : DaoBase
    {
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
            return GetList().FirstOrDefault(entry => entry.Id == id);
        }

        private static IEnumerable<FishingTrip> GetList()
        {
            using (FishitContext context = new FishitContext())
            {
                return context.FishingTrips.ToList();
            }
        }
    }
}