using Ardalis.ApiEndpoints;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.w9wen.dotnet.Template.Web.Endpoints.Employee;
using Swashbuckle.AspNetCore.Annotations;
using w9wen.dotnet.Template.Infrastructure.Data;
using w9wen.dotnet.Template.Web.Configurations;
using w9wen.dotnet.Template.Web.Extensions;

namespace w9wen.dotnet.Template.Web.Endpoints.Employee
{
  public class List : EndpointBaseAsync
    .WithRequest<EmployeeListRequest>
    .WithActionResult<IEnumerable<EmployeeDto>>
  {
    private readonly AppUserManager _appUserManager;
    private readonly IMapper _mapper;

    public List(AppUserManager appUserManager, IMapper mapper)
    {
      _appUserManager = appUserManager;
      _mapper = mapper;
    }
    [HttpGet(EmployeeListRequest.Route)]
    [SwaggerOperation(
        Summary = "Gets a list of all employees",
        Description = "Gets a list of all employees",
        OperationId = "Employee.List",
        Tags = new[] { "EmployeeEndpoints" })
    ]
    public override async Task<ActionResult<IEnumerable<EmployeeDto>>> HandleAsync([FromQuery] EmployeeListRequest request, CancellationToken cancellationToken = default)
    {
      var query = _appUserManager.Users;
      // .Include(x => x.AppUserRoles)
      // .ThenInclude(x => x.AppRole)
      // .AsQueryable();

      #region filter

      if (!string.IsNullOrEmpty(request.Gender))
        query = query.Where(x => !string.IsNullOrEmpty(x.Gender) && x.Gender.ToLower() == request.Gender.ToLower());

      if (!string.IsNullOrEmpty(request.City))
        query = query.Where(x => !string.IsNullOrEmpty(x.City) && x.City.ToLower() == request.City.ToLower());

      if (!string.IsNullOrEmpty(request.Country))
        query = query.Where(x => !string.IsNullOrEmpty(x.Country) && x.Country.ToLower() == request.Country.ToLower());

      #endregion filter

      #region order

      query = request.OrderBy switch
      {
        "createdDateTime"
          => query.OrderByDescending(x => x.CreatedDateTime),
        _ => query.OrderByDescending(x => x.UserName)

      };

      #endregion order

      var employeeListPaged = await PagedList<EmployeeDto>.CreateAsync(
        query.ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider).AsNoTracking(),
        request.PageNumber,
        request.PageSize);

      Response.AddPaginationHeader(employeeListPaged.CurrentPage,
                                   employeeListPaged.PageSize,
                                   employeeListPaged.TotalCount,
                                   employeeListPaged.TotalPages);

      return Ok(employeeListPaged);
      // var employeeDtoList = _mapper.Map<List<EmployeeDto>>(query.ToList());

      // Response.AddPaginationHeader(query.cu)
      // return Ok(employeeDtoList);

      // var employeeDtoQuery = query.ProjectTo<List<EmployeeDto>>(_mapper.ConfigurationProvider)
      //                             .AsQueryable();

      // if (employeeDtoQuery == null || request == null)
      // {
      //   return BadRequest();
      // }


      // return await PagedList<List<EmployeeDto>>.CreateAsync(employeeDtoQuery, request.PageNumber, request.PageSize);
    }
  }
}