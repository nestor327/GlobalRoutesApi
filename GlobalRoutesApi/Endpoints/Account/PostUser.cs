using Ardalis.ApiEndpoints;
using Ardalis.Result;
using AutoMapper;
using GlobalRoutes.Api.Requests.Account;
using GlobalRoutes.Core.Dtos.Account;
using GlobalRoutes.Core.Entities.Users;
using GlobalRoutes.Core.Interfaces.Account;
using GlobalRoutes.SharedKernel.Helpers;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GlobalRoutes.Api.Endpoints.Account
{
    public class PostUser : EndpointBaseAsync.WithRequest<PostUserRequest>.WithActionResult<UserDto>
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<PostUser> _logger;
        private readonly IMapper _mapper;

        public PostUser(IAccountService accountService, IMapper mapper, ILogger<PostUser> logger)
        {
            _accountService = accountService;
            _mapper = mapper;
            _logger = logger;
        }

        [ProducesResponseType(500, Type = typeof(ProblemDetails))]
        [ProducesResponseType(400, Type = typeof(ProblemDetails))]
        [ProducesResponseType(201, Type = typeof(UserDto))]
        [HttpPost(Routes.Account.PostUser)]
        [SwaggerOperation(
            Summary = "Post a new user.",
            Description = "Creation of a new application user who will have access to the system.",
            OperationId = "Account.PostUser",
            Tags = new[] { "Account" }
        )]
        public override async Task<ActionResult<UserDto>> HandleAsync(PostUserRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation(LoggerHelper.EndpointRequest(Routes.Account.PostUser, request));

                var applicationUser = _mapper.Map<PostUserRequest, User>(request);

                var result = await _accountService.CreateUser(applicationUser, request.Password);

                if (result.IsSuccess)
                {
                    _logger.LogInformation(LoggerHelper.EndpointRequestSuccessfully(Routes.Account.PostUser, request));

                    var mapped = _mapper.Map<User, UserDto>(result.Value);

                    return Created($"Created {nameof(Core.Entities.Users.User)}", mapped);
                }

                if (result.Status == ResultStatus.Invalid)
                {
                    var invalidError = ErrorHelper.GetValidationErrors(result.ValidationErrors);

                    _logger.LogWarning(LoggerHelper.EndpointRequestError(Routes.Account.PostUser, request, invalidError));

                    return Problem(invalidError, Routes.Account.PostUser, 400);
                }

                var error = ErrorHelper.GetErrors(result.Errors.ToList());

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.PostUser, request, error));

                return Problem(error, Routes.Account.PostUser, 500);

            }
            catch (Exception ex)
            {
                var error = ErrorHelper.GetExceptionError(ex);

                _logger.LogError(LoggerHelper.EndpointRequestError(Routes.Account.PostUser, request, error));

                return Problem(error, Routes.Account.PostUser, 500);
            }
        }
    }
}
