﻿using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Disfarm.Data;
using Disfarm.Data.Enums;
using Disfarm.Services.Extensions;
using Disfarm.Services.Game.Localization.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Disfarm.Services.Game.Localization.Queries
{
    public record GetLocalizationByLocalizedNameQuery(
            LocalizationCategory Category,
            string LocalizedName)
        : IRequest<LocalizationDto>;

    public class GetLocalizationByLocalizedNameHandler
        : IRequestHandler<GetLocalizationByLocalizedNameQuery, LocalizationDto>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public GetLocalizationByLocalizedNameHandler(
            DbContextOptions options,
            IMapper mapper)
        {
            _db = new AppDbContext(options);
            _mapper = mapper;
        }

        public async Task<LocalizationDto> Handle(GetLocalizationByLocalizedNameQuery request, CancellationToken ct)
        {
            var entity = await _db.Localizations
                .FirstOrDefaultAsync(x =>
                    x.Category == request.Category &&
                    (EF.Functions.ILike(x.Single, $"%{request.LocalizedName}%") ||
                     EF.Functions.ILike(x.Double, $"%{request.LocalizedName}%") ||
                     EF.Functions.ILike(x.Multiply, $"%{request.LocalizedName}%")));

            if (entity is null)
            {
                throw new ExceptionExtensions.GameUserExpectedException(
                    "никогда не слышала о предмете с таким названием.");
            }

            return _mapper.Map<LocalizationDto>(entity);
        }
    }
}