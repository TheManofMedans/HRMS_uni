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

namespace HRMS.Application.Services
{
    public class RequestService : IRequestService
    {
        private readonly IMapper _mapper;
        private readonly IRequestRepository _requestRepository;
        private readonly IEmployeeRepository _employeeRepository;
        public RequestService(IMapper mapper, IRequestRepository requestRepository, IEmployeeRepository employeeRepository)
        {
            _mapper = mapper;
            _requestRepository = requestRepository;
            _employeeRepository = employeeRepository;
        }
        public async Task<RequestResponseDto?> GetByIdAsync(int id) 
        {
            var request = await _requestRepository.GetByIdAsync(id);
            return request is null ? null : _mapper.Map<RequestResponseDto?>(request);
        }
        public async Task<IEnumerable<RequestResponseDto>> GetAllAsync()
        {
            var Requests = await _requestRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<RequestResponseDto>>(Requests);
        }
        public async Task<IEnumerable<RequestResponseDto>> GetByEmployeeIdAsync(int id)
        {
            var employee = await _employeeRepository.GetbyIdAsync(id);
            if (employee == null)
            {
                throw new NotFoundException(nameof(employee),id);
            }
            var requests = await _requestRepository.GetByEmployeeIdAsync(id);
            return _mapper.Map<IEnumerable<RequestResponseDto>>(requests);
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
            return _mapper.Map<IEnumerable<RequestResponseDto>>(requests);
        }
        public async Task<IEnumerable<RequestResponseDto>> GetWithTypeAsync(RequestType type)
        {
            var requests = await _requestRepository.GetWithTypeAsync(type);
            return _mapper.Map<IEnumerable<RequestResponseDto>>(requests); 
        }
        public async Task<IEnumerable<RequestResponseDto>> GetWithCustomDataAsync(int? EmployeeId, RequestStatus? Status, RequestType? Type)
        {
            var requests = await _requestRepository.GetWithCustomDataAsync(EmployeeId, Status, Type);
            return _mapper.Map<IEnumerable<RequestResponseDto>>(requests);
        }
        public async Task<RequestResponseDto> CreateAsync(CreateRequestDto requestDto)
        {
            var request = _mapper.Map<Request>(requestDto);
            var employee = await _employeeRepository.GetbyIdAsync(requestDto.EmployeeId);
            if (employee == null)
            {
                throw new NotFoundException("Employee is not found!");
            }
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
            return _mapper.Map<RequestResponseDto>(request);
        }
        public async Task<bool> UpdateAsync(int id,UpdateRequestDto requestDto)
        {
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
            return await _requestRepository.SaveChangesAsync();
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
    }
}
