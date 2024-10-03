using AutoMapper;
using GlobalRoutes.Core.Entities.Buses;
using GlobalRoutes.Core.Entities.Countries;
using GlobalRoutes.Core.Entities.Languages;
using GlobalRoutes.Core.Entities.Roles;
using GlobalRoutes.Core.Entities.Routes;
using GlobalRoutes.Core.Entities.Schedules;
using GlobalRoutes.Core.Entities.Stops;
using GlobalRoutes.Core.Entities.Subscriptions;
using GlobalRoutes.Core.Entities.WeekDays;
using GlobalRoutes.SharedKernel.Helpers;
using GlobalRoutes.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly IAsyncRepository<Route> _routeRepository;
        private readonly IAsyncRepository<Coordinate> _coordinateRepository;

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
            IAsyncRepository<Coordinate> coordinateRepository,
            IAsyncRepository<Route> routeRepository,
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
            _routeRepository = routeRepository;
            _coordinateRepository = coordinateRepository;
            _logger = logger;
            _mapper = mapper;
            _env = env;
        }

        public async Task RunAsync()
        {
            //Execute First
            await CreateLanguages(".\\languages.csv");
            await CreateCountries(".\\countries.csv");
            await CreateBusTypes(".\\bustypes.csv");
            await CreateAuthorizationRoles(".\\authorization_roles.csv");
            await CreateWeekDays(".\\week_days.csv");
            await CreateTimeZones(".\\time_zones.csv");
            await CreateRoutes(".\\routes.csv");

            //Execure Second
            await CreateCities(".\\cities.csv");
            await CreateBuses(".\\buses.csv");
            await CreateSchedules(".\\schedules.csv");
            await CreateScheduleWeekDays(".\\schedules_week_days.csv");
            await CreateCoordinates(".\\coordinates.csv");
            await CreateStops(".\\stops.csv");
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

        private async Task CreateBusTypes(string path)
        {
            try
            {
                _logger.LogInformation($"Start of the process of importing seed values for {nameof(BusType)}");

                path = Path.Combine(_env.ContentRootPath.Replace("GlobalRoutesApi", ""), "GlobalRoutesApi", "GlobalRoutes.Infrastructure.Importe", "Resources", path);

                var parsed = CsvImporter<BusType>.FromCsvPath(path);
                var busTypes = await _busTypeRepository.ListAsync();

                Console.WriteLine("LA CANTIDAD DE TIPOS ES: " + parsed.Count);

                foreach (var busType in parsed)
                {
                    var existingBusType = busTypes.FirstOrDefault(bustypes => bustypes.Id == busType.Id);

                    if (existingBusType != null)
                    {
                        if (existingBusType.Name != busType.Name || existingBusType.Description != busType.Description)
                        {
                            existingBusType.Name = busType.Name;
                            existingBusType.Description = busType.Description;

                            existingBusType.UpdatedAt = _currentDateUtcZero;
                            existingBusType.UpdatedBy = _defaultUserId;

                            await _busTypeRepository.UpdateAsync(existingBusType);
                        }
                    }
                    else
                    {
                        busType.CreatedAt = _currentDateUtcZero;
                        busType.CreatedBy = _defaultUserId;

                        await _busTypeRepository.AddAsync(busType);
                    }
                }

                _logger.LogInformation($"End of the process of importing seed values for {nameof(BusType)}");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in the process of importing seed values for {nameof(BusType)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
            }


        }

        private async Task CreateAuthorizationRoles(string path)
        {
            try
            {
                _logger.LogInformation($"Start of the process of importing seed values for {nameof(Role)}");

                path = Path.Combine(_env.ContentRootPath.Replace("GlobalRoutesApi", ""), "GlobalRoutesApi", "GlobalRoutes.Infrastructure.Importe", "Resources", path);

                var authorizationRolesCSV = CsvImporter<Role>.FromCsvPath(path);
                var authorizationRolesDB = await _roleManager.Roles.ToListAsync();

                foreach (var newRole in authorizationRolesCSV)
                {
                    var existingRole = authorizationRolesDB.FirstOrDefault(roleDB => roleDB.Id == newRole.Id);

                    if (existingRole != null)
                    {
                        if (existingRole.Name != newRole.Name)
                        {
                            existingRole.Name = newRole.Name;

                            existingRole.UpdatedAt = _currentDateUtcZero;
                            existingRole.UpdatedBy = _defaultUserId;

                            await _roleManager.UpdateAsync(existingRole);
                        }
                    }
                    else
                    {
                        newRole.CreatedAt = _currentDateUtcZero;
                        newRole.CreatedBy = _defaultUserId;

                        await _roleManager.CreateAsync(newRole);
                    }
                }

                _logger.LogInformation($"End of the process of importing seed values for {nameof(Role)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in the process of importing seed values for {nameof(Role)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
            }
        }

        private async Task CreateWeekDays(string path)
        {
            try
            {
                _logger.LogInformation($"Start of the process of importing seed values for {nameof(WeekDay)}");

                path = Path.Combine(_env.ContentRootPath.Replace("GlobalRoutesApi", ""), "GlobalRoutesApi", "GlobalRoutes.Infrastructure.Importe", "Resources", path);

                var parsed = CsvImporter<WeekDay>.FromCsvPath(path);
                var existingWeekDays = await _weekDayRepository.ListAsync();

                foreach (var newWeekDay in parsed)
                {
                    var existingWeekDay = existingWeekDays.FirstOrDefault(existing => existing.Id == newWeekDay.Id);

                    if (existingWeekDay != null)
                    {
                        if (existingWeekDay.Name != newWeekDay.Name)
                        {
                            existingWeekDay.Name = newWeekDay.Name;

                            existingWeekDay.UpdatedAt = _currentDateUtcZero;
                            existingWeekDay.UpdatedBy = _defaultUserId;

                            await _weekDayRepository.UpdateAsync(existingWeekDay);
                        }
                    }
                    else
                    {
                        newWeekDay.CreatedAt = _currentDateUtcZero;
                        newWeekDay.CreatedBy = _defaultUserId;

                        await _weekDayRepository.AddAsync(newWeekDay);
                    }
                }

                _logger.LogInformation($"End of the process of importing seed values for {nameof(WeekDay)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in the process of importing seed values for {nameof(WeekDay)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
            }
        }

        private async Task CreateTimeZones(string path)
        {
            try
            {
                _logger.LogInformation($"Start of the process of importing seed values for {nameof(Core.Entities.TimeZones.TimeZone)}");

                path = Path.Combine(_env.ContentRootPath.Replace("GlobalRoutesApi", ""), "GlobalRoutesApi", "GlobalRoutes.Infrastructure.Importe", "Resources", path);

                var parsed = CsvImporter<Core.Entities.TimeZones.TimeZone>.FromCsvPath(path);
                var timeZones = await _timeZoneRepository.ListAsync();

                foreach (var timeZone in parsed)
                {
                    var existingTimeZone = timeZones.FirstOrDefault(time => time.Id == timeZone.Id);

                    if (existingTimeZone != null)
                    {
                        if (existingTimeZone.Offset != timeZone.Offset || existingTimeZone.Text != timeZone.Text)
                        {
                            existingTimeZone.Offset = timeZone.Offset;
                            existingTimeZone.Text = timeZone.Text;

                            existingTimeZone.UpdatedAt = _currentDateUtcZero;
                            existingTimeZone.UpdatedBy = _defaultUserId;

                            await _timeZoneRepository.UpdateAsync(existingTimeZone);
                        }
                    }
                    else
                    {
                        timeZone.CreatedAt = _currentDateUtcZero;
                        timeZone.CreatedBy = _defaultUserId;

                        await _timeZoneRepository.AddAsync(timeZone);
                    }
                }

                _logger.LogInformation($"End of the process of importing seed values for {nameof(Core.Entities.TimeZones.TimeZone)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in the process of importing seed values for {nameof(Core.Entities.TimeZones.TimeZone)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
            }
        }
        
        private async Task CreateRoutes(string path)
        {
            try
            {
                _logger.LogInformation($"Start of the process of importing seed values for {nameof(Route)}");

                path = Path.Combine(_env.ContentRootPath.Replace("GlobalRoutesApi", ""), "GlobalRoutesApi", "GlobalRoutes.Infrastructure.Importe", "Resources", path);

                var parsed = CsvImporter<Route>.FromCsvPath(path);
                var routes = await _routeRepository.ListAsync();

                foreach (var route in parsed)
                {
                    var existingroute = routes.FirstOrDefault(rou => rou.Id == route.Id);

                    if (existingroute != null)
                    {
                        if (existingroute.OriginName != route.OriginName || existingroute.DestinationName != route.DestinationName)
                        {
                            existingroute.OriginName = route.OriginName;
                            existingroute.DestinationName = route.DestinationName;

                            existingroute.UpdatedAt = _currentDateUtcZero;
                            existingroute.UpdatedBy = _defaultUserId;

                            await _routeRepository.UpdateAsync(existingroute);
                        }
                    }
                    else
                    {
                        route.CreatedAt = _currentDateUtcZero;
                        route.CreatedBy = _defaultUserId;

                        await _routeRepository.AddAsync(route);
                    }
                }

                _logger.LogInformation($"End of the process of importing seed values for {nameof(Route)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in the process of importing seed values for {nameof(Route)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
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

        private async Task CreateBuses(string path)
        {
            try
            {
                _logger.LogInformation($"Start of the process of importing seed values for {nameof(Bus)}");

                path = Path.Combine(_env.ContentRootPath.Replace("GlobalRoutesApi", ""), "GlobalRoutesApi", "GlobalRoutes.Infrastructure.Importe", "Resources", path);

                var parsed = CsvImporter<Bus>.FromCsvPath(path);
                var buses = await _busRepository.ListAsync();

                foreach (var bus in parsed)
                {
                    var existingBus = buses.FirstOrDefault(eleBus => eleBus.Id == bus.Id);

                    if (existingBus != null)
                    {
                        if (existingBus.Name != bus.Name || existingBus.BusTypeId != bus.BusTypeId ||
                            existingBus.Frecuency != bus.Frecuency || existingBus.IsActive != bus.IsActive)
                        {
                            existingBus.Name = bus.Name;
                            existingBus.Frecuency = bus.Frecuency;
                            existingBus.IsActive = bus.IsActive;
                            existingBus.BusTypeId = bus.BusTypeId;

                            existingBus.UpdatedAt = _currentDateUtcZero;
                            existingBus.UpdatedBy = _defaultUserId;

                            existingBus.BusType = await _busTypeRepository.GetByIdAsync(bus.BusTypeId);

                            await _busRepository.UpdateAsync(existingBus);
                        }
                    }
                    else
                    {
                        bus.CreatedAt = _currentDateUtcZero;
                        bus.CreatedBy = _defaultUserId;

                        bus.BusType = await _busTypeRepository.GetByIdAsync(bus.BusTypeId);

                        await _busRepository.AddAsync(bus);
                    }
                }
                _logger.LogInformation($"End of the process of importing seed values for {nameof(Bus)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in the process of importing seed values for {nameof(Bus)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
            }
        }

        private async Task CreateSchedules(string path)
        {

            try
            {
                _logger.LogInformation($"Start of the process of importing seed values for {nameof(Schedule)}");

                path = Path.Combine(_env.ContentRootPath.Replace("GlobalRoutesApi", ""), "GlobalRoutesApi", "GlobalRoutes.Infrastructure.Importe", "Resources", path);

                var parsed = CsvImporter<Schedule>.FromCsvPath(path);
                var schedules = await _scheduleRepository.ListAsync();

                foreach (var schedule in parsed)
                {
                    var existingSchedule = schedules.FirstOrDefault(eleSchedule => eleSchedule.Id == schedule.Id);

                    if (existingSchedule != null)
                    {
                        if (existingSchedule.Alias != schedule.Alias || existingSchedule.Name != schedule.Name
                            || existingSchedule.DepartureTime != schedule.DepartureTime || existingSchedule.ArrivalTime != schedule.ArrivalTime
                            || existingSchedule.Duration != schedule.Duration || existingSchedule.OrigenLatitude != schedule.OrigenLatitude
                            || existingSchedule.OrigenLogitude != schedule.OrigenLogitude || existingSchedule.DestinoLatitude != schedule.DestinoLatitude
                            || existingSchedule.DestinoLogitude != schedule.DestinoLogitude || existingSchedule.IsActive != schedule.IsActive
                            || existingSchedule.BusId != schedule.BusId)
                        {
                            existingSchedule.Alias = schedule.Alias;
                            existingSchedule.Name = schedule.Name;
                            existingSchedule.DepartureTime = schedule.DepartureTime;
                            existingSchedule.ArrivalTime = schedule.ArrivalTime;
                            existingSchedule.Duration = schedule.Duration;
                            existingSchedule.OrigenLatitude = schedule.OrigenLatitude;
                            existingSchedule.OrigenLogitude = schedule.OrigenLogitude;
                            existingSchedule.DestinoLatitude = schedule.DestinoLatitude;
                            existingSchedule.DestinoLogitude = schedule.DestinoLogitude;
                            existingSchedule.IsActive = schedule.IsActive;
                            existingSchedule.BusId = schedule.BusId;

                            existingSchedule.UpdatedAt = _currentDateUtcZero;
                            existingSchedule.UpdatedBy = _defaultUserId;

                            existingSchedule.Bus = await _busRepository.GetByIdAsync(existingSchedule.BusId);

                            await _scheduleRepository.UpdateAsync(existingSchedule);
                        }
                    }
                    else
                    {
                        schedule.CreatedAt = _currentDateUtcZero;
                        schedule.CreatedBy = _defaultUserId;

                        schedule.Bus = await _busRepository.GetByIdAsync(schedule.BusId);

                        await _scheduleRepository.AddAsync(schedule);
                    }
                }
                _logger.LogInformation($"End of the process of importing seed values for {nameof(Schedule)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in the process of importing seed values for {nameof(Schedule)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
            }
        }

        private async Task CreateScheduleWeekDays(string path)
        {
            try
            {
                _logger.LogInformation($"Start of the process of importing seed values for {nameof(ScheduleWeekDay)}");

                path = Path.Combine(_env.ContentRootPath.Replace("GlobalRoutesApi", ""), "GlobalRoutesApi", "GlobalRoutes.Infrastructure.Importe", "Resources", path);

                var parsed = CsvImporter<ScheduleWeekDay>.FromCsvPath(path);
                var scheduleWeekDays = await _scheduleWeekDayRepository.ListAsync();

                foreach (var scheduleWeekDay in parsed)
                {
                    var existingScheduleWeekDay = scheduleWeekDays.FirstOrDefault(eleSchedule => eleSchedule.Id == scheduleWeekDay.Id);

                    if (existingScheduleWeekDay != null)
                    {

                        if (
                            existingScheduleWeekDay.IsActive != scheduleWeekDay.IsActive || existingScheduleWeekDay.ScheduleId != scheduleWeekDay.ScheduleId
                            || existingScheduleWeekDay.WeekDayId != scheduleWeekDay.WeekDayId
                            )
                        {
                            existingScheduleWeekDay.IsActive = scheduleWeekDay.IsActive;
                            existingScheduleWeekDay.ScheduleId = scheduleWeekDay.ScheduleId;
                            existingScheduleWeekDay.WeekDayId = scheduleWeekDay.WeekDayId;

                            existingScheduleWeekDay.UpdatedAt = _currentDateUtcZero;
                            existingScheduleWeekDay.UpdatedBy = _defaultUserId;

                            existingScheduleWeekDay.Schedule = await _scheduleRepository.GetByIdAsync(existingScheduleWeekDay.ScheduleId);
                            existingScheduleWeekDay.WeekDay = await _weekDayRepository.GetByIdAsync(existingScheduleWeekDay.WeekDayId);

                            await _scheduleWeekDayRepository.UpdateAsync(existingScheduleWeekDay);
                        }
                    }
                    else
                    {
                        scheduleWeekDay.CreatedAt = _currentDateUtcZero;
                        scheduleWeekDay.CreatedBy = _defaultUserId;

                        scheduleWeekDay.Schedule = await _scheduleRepository.GetByIdAsync(scheduleWeekDay.ScheduleId);
                        scheduleWeekDay.WeekDay = await _weekDayRepository.GetByIdAsync(scheduleWeekDay.WeekDayId);

                        await _scheduleWeekDayRepository.AddAsync(scheduleWeekDay);
                    }
                }

                _logger.LogInformation($"End of the process of importing seed values for {nameof(ScheduleWeekDay)}");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in the process of importing seed values for {nameof(ScheduleWeekDay)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
            }
        }

        private async Task CreateCoordinates(string path)
        {
            try
            {
                _logger.LogInformation($"Start of the process of importing seed values for {nameof(Coordinate)}");

                path = Path.Combine(_env.ContentRootPath.Replace("GlobalRoutesApi", ""), "GlobalRoutesApi", "GlobalRoutes.Infrastructure.Importe", "Resources", path);

                var parsed = CsvImporter<Coordinate>.FromCsvPath(path);
                var coordinates = await _coordinateRepository.ListAsync();

                foreach (var coordinate in parsed)
                {
                    var existingCoordinate = coordinates.FirstOrDefault(coord => coord.Id == coordinate.Id);

                    if (existingCoordinate != null)
                    {
                        if (existingCoordinate.Latitude != coordinate.Latitude || existingCoordinate.Longitude != coordinate.Longitude
                            || existingCoordinate.RouteId != coordinate.RouteId
                            )
                        {
                            existingCoordinate.Latitude = coordinate.Latitude;
                            existingCoordinate.Longitude = coordinate.Longitude;
                            existingCoordinate.RouteId = coordinate.RouteId;

                            existingCoordinate.UpdatedAt = _currentDateUtcZero;
                            existingCoordinate.UpdatedBy = _defaultUserId;

                            existingCoordinate.Route = await _routeRepository.GetByIdAsync(existingCoordinate.RouteId);

                            await _coordinateRepository.UpdateAsync(existingCoordinate);
                        }
                    }
                    else
                    {
                        coordinate.CreatedAt = _currentDateUtcZero;
                        coordinate.CreatedBy = _defaultUserId;

                        coordinate.Route = await _routeRepository.GetByIdAsync(coordinate.RouteId);

                        await _coordinateRepository.AddAsync(coordinate);
                    }
                }

                _logger.LogInformation($"End of the process of importing seed values for {nameof(Coordinate)}");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in the process of importing seed values for {nameof(Coordinate)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
            }
        }

        private async Task CreateStops(string path)
        {
            try
            {
                _logger.LogInformation($"Start of the process of importing seed values for {nameof(Stop)}");

                path = Path.Combine(_env.ContentRootPath.Replace("GlobalRoutesApi", ""), "GlobalRoutesApi", "GlobalRoutes.Infrastructure.Importe", "Resources", path);

                var parsed = CsvImporter<Stop>.FromCsvPath(path);
                var stops = await _stopRepository.ListAsync();

                foreach (var stop in parsed)
                {
                    var existingStop = stops.FirstOrDefault(eleStop => eleStop.Id == stop.Id);

                    if (existingStop != null)
                    {

                        if (
                            existingStop.Name != stop.Name || existingStop.ScheduleId != stop.ScheduleId
                            || existingStop.TotalArrivalTime != stop.TotalArrivalTime || existingStop.Latitude != stop.Latitude
                            || existingStop.Longitude != stop.Longitude || existingStop.CoordinateId != stop.CoordinateId
                            )
                        {
                            existingStop.Name = stop.Name;
                            existingStop.ScheduleId = stop.ScheduleId;
                            existingStop.TotalArrivalTime = stop.TotalArrivalTime;
                            existingStop.Latitude = stop.Latitude;
                            existingStop.Longitude = stop.Longitude;
                            existingStop.CoordinateId = stop.CoordinateId;

                            existingStop.UpdatedAt = _currentDateUtcZero;
                            existingStop.UpdatedBy = _defaultUserId;

                            existingStop.Schedules = await _scheduleRepository.GetByIdAsync(existingStop.ScheduleId);
                            existingStop.Coordinate = await _coordinateRepository.GetByIdAsync(existingStop.CoordinateId);
                         
                            await _stopRepository.UpdateAsync(existingStop);
                        }
                    }
                    else
                    {
                        stop.CreatedAt = _currentDateUtcZero;
                        stop.CreatedBy = _defaultUserId;

                        stop.Schedules = await _scheduleRepository.GetByIdAsync(stop.ScheduleId);
                        stop.Coordinate = await _coordinateRepository.GetByIdAsync(stop.CoordinateId);

                        await _stopRepository.AddAsync(stop);
                    }

                }

                _logger.LogInformation($"End of the process of importing seed values for {nameof(Stop)}");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in the process of importing seed values for {nameof(Stop)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
            }
        }

        #endregion

    }

}
