﻿using AutoMapper;
using GlobalRoutes.Core.Entities.Buses;
using GlobalRoutes.Core.Entities.Countries;
using GlobalRoutes.Core.Entities.Languages;
using GlobalRoutes.Core.Entities.Roles;
using GlobalRoutes.Core.Entities.Schedules;
using GlobalRoutes.Core.Entities.Stops;
using GlobalRoutes.Core.Entities.Subscriptions;
using GlobalRoutes.Core.Entities.WeekDays;
using GlobalRoutes.SharedKernel.Helpers;
using GlobalRoutes.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace GlobalRoutes.Infrastructure.Importer
{
    /// <summary>
    /// Seed values class needed by the GlobalRoutes Core
    /// </summary>
    public class Importer
    {
        //Repositories
        private readonly IAsyncRepository<Bus> _busRepository;
        private readonly IAsyncRepository<BusType> _busTypeRepository;
        private readonly IAsyncRepository<City> _cityRepository;
        private readonly IAsyncRepository<Country> _countryRepository;
        private readonly IAsyncRepository<Language> _languageRepository;
        private readonly IAsyncRepository<UserLanguage> _userLanguageRepository;
        private readonly RoleManager<Role> _roleManager;
        private readonly IAsyncRepository<Schedule> _scheduleRepository;
        private readonly IAsyncRepository<ScheduleWeekDay> _scheduleWeekDayRepository;
        private readonly IAsyncRepository<Stop> _stopRepository;
        private readonly IAsyncRepository<Subscription> _subscriptionRepository;
        private readonly IAsyncRepository<Core.Entities.TimeZones.TimeZone> _timeZoneRepository;
        private readonly IAsyncRepository<WeekDay> _weekDayRepository;

        //Reference global variables for seed value creations and updates
        private readonly DateTime _currentDateUtcZero = DateTime.UtcNow;
        private readonly string _defaultUserId = "SYSTEM";

        //General
        private readonly ILogger<Importer> _logger;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public Importer(
            IAsyncRepository<Bus> busRepository,
            IAsyncRepository<BusType> busTypeRepository,
            IAsyncRepository<City> cityRepository,
            IAsyncRepository<Country> countryRepository,
            IAsyncRepository<Language> languageRepository,
            IAsyncRepository<UserLanguage> userLanguageRepository,
            RoleManager<Role> roleManager,
            IAsyncRepository<Schedule> scheduleRepository,
            IAsyncRepository<ScheduleWeekDay> scheduleWeekDayRepository,
            IAsyncRepository<Stop> stopRepository,
            IAsyncRepository<Subscription> subscriptionRepository,
            IAsyncRepository<Core.Entities.TimeZones.TimeZone> timeZoneRepository,
            IAsyncRepository<WeekDay> weekDayRepository,
            ILogger<Importer> logger,
            IMapper mapper,
            IWebHostEnvironment env

            )
        {
            _busRepository = busRepository;
            _busTypeRepository = busTypeRepository;
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
            _languageRepository = languageRepository;
            _userLanguageRepository = userLanguageRepository;
            _roleManager = roleManager;
            _scheduleRepository = scheduleRepository;
            _scheduleWeekDayRepository = scheduleWeekDayRepository;
            _stopRepository = stopRepository;
            _subscriptionRepository = subscriptionRepository;
            _timeZoneRepository = timeZoneRepository;
            _weekDayRepository = weekDayRepository;
            _logger = logger;
            _mapper = mapper;
            _env = env;
        }

        public async Task RunAsync()
        {
            //Execute First
            await CreateLanguages(".\\languages.csv");
            await CreateCountries(".\\countries.csv");

            //Execure Second

            await CreateCities(".\\cities.csv");
        }

        #region Execute First
        private async Task CreateLanguages(string path)
        {
            try
            {
                _logger.LogInformation($"Start of the process of importing seed values for {nameof(Language)}");

                path = Path.Combine(_env.ContentRootPath.Replace("GlobalRoutesApi", ""), "GlobalRoutesApi", "GlobalRoutes.Infrastructure.Importe", "Resources", path);

                var languagesCSV = CsvImporter<Language>.FromCsvPath(path);
                var languagesBD = await _languageRepository.ListAsync();

                foreach (var newLanguage in languagesCSV)
                {
                    var existingLanguage = languagesBD.FirstOrDefault(languageBD => languageBD.Id == newLanguage.Id);

                    if (existingLanguage != null)
                    {
                        if (existingLanguage.Code != newLanguage.Code || existingLanguage.Name != newLanguage.Name ||
                            existingLanguage.Description != newLanguage.Description || existingLanguage.IsActive != newLanguage.IsActive)
                        {
                            existingLanguage.Code = newLanguage.Code;
                            existingLanguage.Name = newLanguage.Name;
                            existingLanguage.Description = newLanguage.Description;
                            existingLanguage.IsActive = newLanguage.IsActive;

                            existingLanguage.UpdatedAt = _currentDateUtcZero;
                            existingLanguage.UpdatedBy = _defaultUserId;

                            await _languageRepository.UpdateAsync(existingLanguage);
                        }
                    }
                    else
                    {
                        newLanguage.CreatedAt = _currentDateUtcZero;
                        newLanguage.CreatedBy = _defaultUserId;

                        await _languageRepository.AddAsync(newLanguage);
                    }
                }

                _logger.LogInformation($"End of the process of importing seed values for {nameof(Language)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in the process of importing seed values for {nameof(Language)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
            }
        }

        private async Task CreateCountries(string path)
        {
            try
            {
                _logger.LogInformation($"Start of the process of importing seed values for {nameof(Country)}");
                path = Path.Combine(_env.ContentRootPath.Replace("GlobalRoutesApi", ""), "GlobalRoutesApi", "GlobalRoutes.Infrastructure.Importe", "Resources", path);

                var parsed = CsvImporter<Country>.FromCsvPath(path);
                var countries = await _countryRepository.ListAsync();

                foreach (var country in parsed)
                {
                    var existingCountry = countries.FirstOrDefault(countries => countries.Id == country.Id);

                    if (existingCountry != null)
                    {
                        if (existingCountry.Name != country.Name || existingCountry.IsoCode != country.IsoCode ||
                            existingCountry.CountryCode != country.CountryCode ||
                            existingCountry.OfficialName != country.OfficialName)
                        {
                            existingCountry.Name = country.Name;
                            existingCountry.OfficialName = country.OfficialName;
                            existingCountry.IsoCode = country.IsoCode;
                            existingCountry.CountryCode = country.CountryCode;

                            existingCountry.UpdatedAt = _currentDateUtcZero;
                            existingCountry.UpdatedBy = _defaultUserId;

                            await _countryRepository.UpdateAsync(existingCountry);
                        }
                    }
                    else
                    {
                        country.CreatedAt = _currentDateUtcZero;
                        country.CreatedBy = _defaultUserId;

                        await _countryRepository.AddAsync(country);
                    }
                }

                _logger.LogInformation($"End of the process of importing seed values for {nameof(Country)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in the process of importing seed values for {nameof(Country)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
            }
        }


        #endregion

        #region Execute Second 

        private async Task CreateCities(string path)
        {
            try
            {
                _logger.LogInformation($"Start of the process of importing seed values for {nameof(City)}");

                path = Path.Combine(_env.ContentRootPath.Replace("GlobalRoutesApi", ""), "GlobalRoutesApi", "GlobalRoutes.Infrastructure.Importe", "Resources", path);

                var parsed = CsvImporter<City>.FromCsvPath(path);
                var cities = await _cityRepository.ListAsync();

                foreach (var city in parsed)
                {
                    var existingCity = cities.FirstOrDefault(cities => cities.Id == city.Id);

                    if (existingCity != null)
                    {
                        if (existingCity.Name != city.Name || existingCity.IsoCode != city.IsoCode ||
                            existingCity.CountryId != city.CountryId)
                        {
                            existingCity.Name = city.Name;
                            existingCity.IsoCode = city.IsoCode;
                            existingCity.CountryId = city.CountryId;

                            existingCity.UpdatedAt = _currentDateUtcZero;
                            existingCity.UpdatedBy = _defaultUserId;

                            existingCity.Country = await _countryRepository.GetByIdAsync(city.CountryId);

                            await _cityRepository.UpdateAsync(existingCity);
                        }
                    }
                    else
                    {
                        city.CreatedAt = _currentDateUtcZero;
                        city.CreatedBy = _defaultUserId;

                        city.Country = await _countryRepository.GetByIdAsync(city.CountryId);

                        await _cityRepository.AddAsync(city);
                    }
                }

                _logger.LogInformation($"End of the process of importing seed values for {nameof(City)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in the process of importing seed values for {nameof(City)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
            }
        }

        #endregion

    }

}
