using zadanie5_APBD.Controllers;

namespace zadanie5_APBD.Services
{
    public interface IDbService
    {
        public Task<IEnumerable<object>> GetTrips();
        public Task<bool> DeleteClient(int idClient);
        public Task AddClientToTrip(int idTrip,ClientDto clientDto);

    }
}
