﻿using Microsoft.EntityFrameworkCore;
using zadanie5_APBD.Context;
using zadanie5_APBD.Controllers;
using zadanie5_APBD.Models;

namespace zadanie5_APBD.Services
{
    public class DbService : IDbService
    {

        private readonly MasterContext _Mastercontext;
        public DbService(MasterContext masterContext)
        {
            _Mastercontext = masterContext;
        }


        public async Task<IEnumerable<object>> GetTrips()
        {
            IEnumerable<object> tripList = await _Mastercontext.Trips.Select(e => new
            {
                e.Name,
                e.Description,
                e.DateFrom,
                e.DateTo,
                e.MaxPeople,
                Country = e.CountryTrips.Select(d => d.IdCountryNavigation)
                                        .Select(d => new { Name = d.Name })
                                        .ToList(),
                
                Client = e.ClientTrips.Select(d => d.IdClientNavigation)
                                      .Select(d => new { d.FirstName, d.LastName })
                                      .ToList()

            }).OrderByDescending(d => d.DateFrom)
              .ToListAsync();

            return tripList;
        }
        public async Task<bool> DeleteClient(int idClient)
        {                           
            if (_Mastercontext.ClientTrips.AnyAsync(e => e.IdClient == idClient).Result==true)
            {
                return false;
            }
            if (_Mastercontext.Clients.AnyAsync(e => e.IdClient == idClient).Result==false)
            {
                return false;
            }                            
            
            _Mastercontext.Clients.Remove(_Mastercontext.Clients.Where(e => e.IdClient == idClient).First());
            await _Mastercontext.SaveChangesAsync();
            return true;

        }

        public async Task AddClientToTrip(int idTrip, ClientDto clientDto)
        {
            var trip = await _Mastercontext.Trips.FindAsync(idTrip);
            if (trip == null)
            {
                throw new Exception("Trip not found");
            }
            
            var client = await _Mastercontext.Clients.SingleOrDefaultAsync(c => c.Pesel == clientDto.Pesel);
            if (client == null)
            {
                client = new Client 
                { 
                    FirstName = $"{clientDto.FirstName}",
                    LastName = $"{clientDto.LastName}",
                    Email = clientDto.Email,
                    Telephone = clientDto.Telephone,
                    Pesel = clientDto.Pesel
                };
                _Mastercontext.Clients.Add(client);
                await _Mastercontext.SaveChangesAsync();
            }
            var existingClientTrip = await _Mastercontext.ClientTrips
                .SingleOrDefaultAsync(ct => ct.IdTrip == idTrip && ct.IdClient == client.IdClient);
            if (existingClientTrip != null)
            {
                throw new Exception("Client is already registered for this trip");
            }
            
            var clientTrip = new ClientTrip
            {
                IdTrip = idTrip,
                IdClient = client.IdClient,
                RegisteredAt = DateTime.UtcNow,
                PaymentDate = clientDto.PaymentDate
            };

            _Mastercontext.ClientTrips.Add(clientTrip);
            await _Mastercontext.SaveChangesAsync();
        }

    }
}
