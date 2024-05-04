using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Backend.Common.AuthorizationDetails;

namespace UserManagement.Backend.Application.Core.Authentication.Queries.LoginQuery;

public record LoginQuery(LoginQueryRequestDTO Data, AuthorizationDetails AuthorizationDetails):IRequest<ErrorOr<LoginQueryResponseDTO>>;
