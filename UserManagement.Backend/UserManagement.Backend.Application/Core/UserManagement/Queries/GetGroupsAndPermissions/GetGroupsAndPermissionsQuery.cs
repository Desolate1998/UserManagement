using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Backend.Common.AuthorizationDetails;

namespace UserManagement.Backend.Application.Core.UserManagement.Queries.GetGroupsAndPermissions;

public record GetGroupsAndPermissionsQuery(AuthorizationDetails AuthorizationDetails):IRequest<ErrorOr<GetGroupsAndPermissionsQueryResponseDTO>>;
