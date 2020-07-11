using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidyaSadhan_API.Entities;
using VidyaSadhan_API.Extensions;
using VidyaSadhan_API.Models;

namespace VidyaSadhan_API.Services
{
    public class AddressService
    {
        private VSDbContext _dbContext;
        IMapper _map;

        public AddressService(VSDbContext dbContext, IMapper map)
        {
            _dbContext = dbContext;
            _map = map;
        }


        public async Task<AddressViewModel> GetAddressByUserId(string userId)
        {
            try
            {
                var result = await _dbContext.AccountAddress.FirstOrDefaultAsync(x => x.UserId == userId).ConfigureAwait(false);
                return _map.Map<AddressViewModel>(result);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<int> SaveAddress(AddressViewModel address)
        {
            try
            {
                _dbContext.AccountAddress.Add(_map.Map<Address>(address));
                return await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateAddress(AddressViewModel address)
        {
            try
            {
                _dbContext.AccountAddress.Update(_map.Map<Address>(address));
                return await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
