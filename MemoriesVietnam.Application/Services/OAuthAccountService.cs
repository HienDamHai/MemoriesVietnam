using MemoriesVietnam.Application.DTOs;
using MemoriesVietnam.Application.Interfaces;
using MemoriesVietnam.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.Services
{
    public class OAuthAccountService : IOAuthAccountService
    {
        private readonly IOAuthAccountRepository _repository;
        public OAuthAccountService(IOAuthAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<OAuthAccountDto.OAuthAccountResponseDto> GetByIdAsync(string id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if(entity == null) return null;

            return new OAuthAccountDto.OAuthAccountResponseDto
            {
                Id = entity.Id,
                Provider = entity.Provider,
                ProviderUserId = entity.ProviderUserId,
                AccessToken = entity.AccessToken,
                RefreshToken = entity.RefreshToken,
                ExpireAt = entity.ExpireAt,
                LoginId = entity.LoginId
            };


        }
    }
}
