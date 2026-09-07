using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HRMS.domain.Entities;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Services;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.DTOs.Request;
using HRMS.Application.Exceptions;
using HRMS.domain.Enums;
using HRMS.Application.Interfaces;


namespace HRMS.Application.Services
{
    public class RequestService : IRequestService
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;
        private readonly IRequestRepository _requestRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly INotificationService _notificationService;
        public RequestService(IMapper mapper, IRequestRepository requestRepository, IEmployeeRepository employeeRepository,ICurrentUserService currentUser
            , INotificationService notificationService)
        {
            _mapper = mapper;
            _requestRepository = requestRepository;
            _employeeRepository = employeeRepository;
            _currentUser = currentUser;
            _notificationService = notificationService;
        }
        public async Task<RequestResponseDto?> GetByIdAsync(int id) 
        {
            var request = await _requestRepository.GetByIdAsync(id);
            return request is null ? null : _mapper.Map<RequestResponseDto?>(request);
        }
        public async Task<IEnumerable<RequestResponseDto>> GetAllAsync()
        {
            var Requests = await _requestRepository.GetAllAsync();
            var visible = FilterVisible(Requests);
            return _mapper.Map<IEnumerable<RequestResponseDto>>(visible);
        }
        public async Task<IEnumerable<RequestResponseDto>> GetByEmployeeIdAsync(int id)
        {
            var employee = await _employeeRepository.GetbyIdAsync(id);
            if (employee == null)
            {
                throw new NotFoundException(nameof(employee),id);
            }
            var requests = await _requestRepository.GetByEmployeeIdAsync(id);
            var filtered = FilterVisible(requests);
            return _mapper.Map<IEnumerable<RequestResponseDto>>(filtered);
        }
        public async Task<IEnumerable<RequestResponseDto>> GetWithCompanyIdAsync(int companyId)
        {
            var requests = await _requestRepository.GetByCompanyIdAsync(companyId);
            return _mapper.Map<IEnumerable<RequestResponseDto>>(requests);
        }
        public async Task<IEnumerable<RequestResponseDto>> GetWithDepartmentIdAsync(int departmentId)
        {
            var requests = await _requestRepository.GetByDepartmentIdAsync(departmentId);
            return _mapper.Map<IEnumerable<RequestResponseDto>>(requests);
        }
        public async Task<IEnumerable<RequestResponseDto>> GetWithStatusAsync(RequestStatus status)
        {
            var requests = await _requestRepository.GetWithStatusAsync(status);
            var visible = FilterVisible(requests);
            return _mapper.Map<IEnumerable<RequestResponseDto>>(visible);
        }
        public async Task<IEnumerable<RequestResponseDto>> GetWithTypeAsync(RequestType type)
        {
            var requests = await _requestRepository.GetWithTypeAsync(type);
            var visible = FilterVisible(requests);
            return _mapper.Map<IEnumerable<RequestResponseDto>>(visible); 
        }
        public async Task<IEnumerable<RequestResponseDto>> GetWithCustomDataAsync(int? EmployeeId, RequestStatus? Status, RequestType? Type)
        {
            var requests = await _requestRepository.GetWithCustomDataAsync(EmployeeId, Status, Type);
            var filtered = FilterVisible(requests);
            return _mapper.Map<IEnumerable<RequestResponseDto>>(filtered);
        }
        public async Task<RequestResponseDto> CreateAsync(CreateRequestDto requestDto)
        {
            var employee = await _employeeRepository.GetByIdWithDepartmentsAsync(requestDto.EmployeeId);
            if (employee == null)
            {
                throw new NotFoundException("Employee is not found!");
            }
            var membership = employee.EmployeeDepartments.FirstOrDefault(ed => ed.DepartmentID == requestDto.DepartmentId);
            if (membership == null)
            {
                throw new ConflictException("This Employee doesnt work in the selected department!");
            }
            bool isOwn = _currentUser.EmployeeId == requestDto.EmployeeId;
            bool hasSufficientRole = (_currentUser.IsSuperAdmin ||
                _currentUser.CompanyRoles.TryGetValue(membership.Department.CompanyId, out var RoleValues) && Enum.TryParse<CompanyRole>(RoleValues, out var Role)
                && Role <= CompanyRole.HREmployee);
            if (!isOwn && !hasSufficientRole)
            {
                throw new ForbiddenException("You dont have authorizaion to add a request!");
            }
            var request = _mapper.Map<Request>(requestDto);
            if (request.EndDate < DateTime.Today)
            {
                throw new ConflictException("The End Date is before Today!");
            }

            request.EmployeeId = employee.Id;
            request.Status = RequestStatus.Pending;
            await _requestRepository.AddAsync(request);
           bool saved =  await _requestRepository.SaveChangesAsync();
            if (!saved)
            {
                throw new Exception("Couldnt save the new request!");
            }
            await _notificationService.NotifyAsync(request.Employee.UserId,
                $"A new Request has been made by {request.EmployeeId} With the Subject: {request.Type}",
                NotificationType.RequestSubmitted,attendanceId: null, requestId: request.Id);
            return _mapper.Map<RequestResponseDto>(request);
        }
        public async Task<bool> UpdateAsync(int id,UpdateRequestDto requestDto)
        {
            NotificationType type = new();
            var request = await _requestRepository.GetByIdAsync(id);
            if (request is null)
            {
                throw new NotFoundException(nameof(request),id);
            }
            request.ReviewedAt = requestDto.ReviewedAt;
            if ((requestDto.Status != null))
            {
                request.Status = requestDto.Status.Value;
            }
            if ((requestDto.StartDate != null))
            {
                request.StartDate = requestDto.StartDate.Value;
            }
            if (requestDto.EndDate != null)
            {
                request.EndDate = requestDto.EndDate.Value;
            }
            if (requestDto.Description != null)
            {
                request.Description = requestDto.Description;
            }
            
            _requestRepository.Update(request);
            var isupdated = await _requestRepository.SaveChangesAsync();
            if (!isupdated)
            {
                throw new Exception("Could not update request!");
            }
            if (request.Status == RequestStatus.Accepted)
            {
                type = NotificationType.RequestApproved;
            }
            else
            {
                type = NotificationType.RequestDenied;
            }
            await _notificationService.NotifyAsync(request.Employee.UserId,
                $"The request with Id {request.Id} has been updated by user {_currentUser.UserId}",
                type,requestId: request.Id);
            return isupdated;
        }
        public async Task UpdateByEmployeeAsync(int id, UpdateRequestDto dto)
        {
            var request = await _requestRepository.GetByIdAsync(id);
            if (request is null)
            {
                throw new NotFoundException(nameof(Request),id);
            }
            bool isown = _currentUser.EmployeeId == request.EmployeeId;
            //bool hasSufficientRole = (_currentUser.IsSuperAdmin || (_currentUser.CompanyRoles.TryGetValue(request.Department.CompanyId,out var RoleValues)
              //  && Enum.TryParse<CompanyRole>(RoleValues,out var Role) && Role <= CompanyRole.HREmployee));
            if (!isown)
            {
                throw new ForbiddenException("You cannot change this request!");
            }
            if (dto.StartDate != null)
            {
                request.StartDate = dto.StartDate.Value;
            }
            if (dto.EndDate != null)
            {
                request.EndDate = dto.EndDate.Value;
            }
            if (dto.Description != null)
            {
                request.Description = dto.Description;
            }
            if (dto.RequestType != null)
            {
                request.Type = dto.RequestType.Value;
            }
            _requestRepository.Update(request);
            var isupdated = await _requestRepository.SaveChangesAsync();
            if (!isupdated)
            {
                throw new Exception("Could not update request!");
            }
            await _notificationService.NotifyAsync(request.Employee.UserId,
                $"The request with id {request.Id} has been updated by User {_currentUser.UserId}",
                NotificationType.RequestUpdated,requestId : request.Id);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var request = await _requestRepository.GetByIdAsync(id);
            if (request is null)
            {
                throw new NotFoundException(nameof(request), id);
            }
            _requestRepository.Delete(request);
            return await _requestRepository.SaveChangesAsync();
        }
        private IEnumerable<Request> FilterVisible(IEnumerable<Request> requests)
        {
            if (_currentUser.IsSuperAdmin)
            {
                return requests;
            }
            return requests.Where(r => r.EmployeeId == _currentUser.EmployeeId ||
            (_currentUser.CompanyRoles.TryGetValue(r.DepartmentId != 0 ? r.Department.CompanyId : -1, out var role) &&
            Enum.TryParse<CompanyRole>(role, out var parsedRole) &&
            parsedRole <= CompanyRole.HREmployee)).ToList();
        }
    }
}
